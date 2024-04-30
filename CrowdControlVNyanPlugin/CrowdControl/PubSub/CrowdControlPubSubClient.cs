using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Web;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub
{
    public class CrowdControlPubSubClient
    {
        public event EventHandler PubSubStart;
        public event EventHandler PubSubFailure;
        public event EventHandler PubSubClose;
        public event EventHandler PubSubReady;
        public event EventHandler<string> LoginRequested;
        public event EventHandler<string> TokenObtained;
        public event EventHandler<TokenData> TokenDataObtained;
        public event EventHandler<string> UIDObtained;

        public event EventHandler<object> LogMessage;

        public event EventHandler<GameSessionStartMessage> GameSessionStart;
        public event EventHandler<GameSessionStopMessage> GameSessionStop;
        public event EventHandler<EffectRequestMessage> EffectRequest;
        public event EventHandler<EffectRetryMessage> EffectRetry;
        public event EventHandler<EffectRefundMessage> EffectRefund;
        public event EventHandler<EffectSuccessMessage> EffectSuccess;
        public event EventHandler<EffectFailureMessage> EffectFailure;
        public event EventHandler<TimedEffectUpdateMessage> TimedEffectUpdate;

        private WebSocket socket;
        private string socketAddress = "wss://pubsub.crowdcontrol.live";
        private string connectionID;
        private TokenData tokenData;
        private string ccToken = "";
        private string ccUID = "";
        private string gameSessionID = "";
        private Timer heartbeat;

        public void Log(object message)
        {
            LogMessage(this, message);
        }

        public CrowdControlPubSubClient() 
        {
            
            socket = new WebSocket(socketAddress);
            socket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            socket.OnOpen += OnSocketOpen;
            socket.OnMessage += OnSocketMessage;
            socket.OnError += OnSocketError;
            socket.OnClose += OnSocketClose;
            heartbeat = new Timer();
            heartbeat.Interval = (1000 * 60 * 9);
            heartbeat.Elapsed += OnHeartbeat;
        }

        public void initSocket(string existingCcToken = "")
        {
            Log($"Initializing CC Socket...");
            try
            {
                if (existingCcToken == null)
                {
                    existingCcToken = "";
                }
                ccToken = existingCcToken;

                socket.Connect();
                Log($"CC Socket Initialized!");
            }
            catch (Exception e)
            {
                Log($"Couldn't initialize CC Socket: {e.Message}");
                PubSubFailure(this, EventArgs.Empty);
            }
        }

        public void deInitSocket()
        {
            ccToken = "";
            socket.Close();
        }

        private void OnSocketOpen(object sender, EventArgs e)
        {
            Log("Connected to Crowd Control! Getting identity...");   
            var whoAmIPacket = JsonConvert.SerializeObject(new { action = "whoami" });
            socket.Send(whoAmIPacket);

            PubSubStart(this, EventArgs.Empty);
        }

        private void OnSocketError(object sender, ErrorEventArgs e)
        {
            Log($"Socket Error: {e.Exception.Message}");
        }

        private void OnSocketClose(object sender, CloseEventArgs e)
        {
            Log($"Disconnected from Crowd Control: {e.Reason}");
            ccUID = "";
            gameSessionID = "";
            heartbeat.Enabled = false;

            PubSubClose(this, EventArgs.Empty);
        }

        private void OnHeartbeat(object sender, EventArgs e)
        {
            var pingPacket = JsonConvert.SerializeObject(new { action = "ping" });
            socket.Send(pingPacket);
        }

        private void OnSocketMessage(object sender, MessageEventArgs e)
        {
            if (e.Data == "pong")
            {
                Log("Got ping response!");
                return;
            }
            Log($"Got socket message: {e.Data}");

            JObject message = JsonConvert.DeserializeObject<JObject>(e.Data);
            string messageDomain = message["domain"].ToString();

            if (messageDomain == "direct") OnDirectMessage(message);
            else if (messageDomain == "prv") OnPrivateMessage(message);
            else if (messageDomain == "pub") OnPublicMessage(message);
        }



        private void OnDirectMessage(JObject message)
        {
            string messageType = message["type"].ToString();

            if (messageType == "subscription-result") OnSubscriptionResult(message.ToObject<SubscriptionResultMessage>());
            else if (messageType == "whoami") OnWhoAmI(message.ToObject<WhoAmIMessage>());
            else if (messageType == "login-success") OnLoginSuccess(message.ToObject<LoginSuccessMessage>());
        }

        private void OnPublicMessage(JObject message)
        {
            string messageType = message["type"].ToString();

            if (messageType == "game-session-start") OnGameSessionStart(message.ToObject<GameSessionStartMessage>());
            else if (messageType == "game-session-stop") OnGameSessionStop(message.ToObject<GameSessionStopMessage>());
        }

        private void OnPrivateMessage(JObject message)
        {
            string messageType = message["type"].ToString();

            if (messageType == "effect-request") OnEffectRequest(message.ToObject<EffectRequestMessage>());
            else if (messageType == "effect-retry") OnEffectRetry(message.ToObject<EffectRetryMessage>());
            else if (messageType == "effect-refund") OnEffectRefund(message.ToObject<EffectRefundMessage>());
            else if (messageType == "effect-success") OnEffectSuccess(message.ToObject<EffectSuccessMessage>());
            else if (messageType == "effect-failure") OnEffectFailure(message.ToObject<EffectFailureMessage>());
            else if (messageType == "timed-effect-update") OnTimedEffectUpdate(message.ToObject<TimedEffectUpdateMessage>());
        }

        private void OnWhoAmI(WhoAmIMessage message)
        {
            connectionID = message.payload.connectionID;
            Log($"Connection ID Determined: {connectionID}");

            Log(ccToken);
            if (ccToken.Length > 0)
            {
                Log($"Token exists, Beginning subscribe...");
                LoadTokenData();
                BeginSubscribe();
                return;
            }
            Log($"No Token, Requesting Auth...");
            RequestLogin();
        }

        private void OnSubscriptionResult(SubscriptionResultMessage message)
        {
            var successItems = message.payload.success;
            if (successItems.Length > 0)
            {
                heartbeat.Enabled = true;
                PubSubReady(this, EventArgs.Empty);
            }
            else
            {
                Log("Subscription failure!  Please confirm your Crowd Control Authorization is correct!");
                socket.Close();
            }
        }

        private void OnLoginSuccess(LoginSuccessMessage message)
        {
            ccToken = message.payload.token;
            Log($"Successful Crowd Control Login!");
            LoadTokenData();
            BeginSubscribe();
            return;
        }

        private void OnGameSessionStart(GameSessionStartMessage message)
        {
            Log($"Game Session {message.payload.gameSessionID} Started!");
            gameSessionID = message.payload.gameSessionID;
            GameSessionStart(this, message);
        }

        private void OnGameSessionStop(GameSessionStopMessage message)
        {
            Log($"Game Session {message.payload.gameSessionID} Stopped!");
            gameSessionID = "";
            GameSessionStop(this, message);
        }

        private void OnEffectRequest(EffectRequestMessage message)
        {
            Log($"Effect {message.payload.effect.effectID} Requested!");
            EffectRequest(this, message);
        }

        private void OnEffectRetry(EffectRetryMessage message)
        {
            Log($"Effect {message.payload.effect.effectID} Retried!");
            EffectRetry(this, message);
        }

        private void OnEffectRefund(EffectRefundMessage message)
        {
            Log($"Effect {message.payload.effect.effectID} Refunded!");
            EffectRefund(this, message);
        }

        private void OnEffectSuccess(EffectSuccessMessage message)
        {
            Log($"Effect {message.payload.effect.effectID} Succeeded!");
            EffectSuccess(this, message);
        }

        private void OnEffectFailure(EffectFailureMessage message)
        {
            Log($"Effect {message.payload.effect.effectID} Failed!");
            EffectFailure(this, message);
        }

        private void OnTimedEffectUpdate(TimedEffectUpdateMessage message)
        {
            Log($"Timed Effect {message.payload.effect.effectID} Updated!");
            TimedEffectUpdate(this, message);
        }

        private void RequestLogin()
        {
            var encodedConnectionID = HttpUtility.UrlEncode(connectionID);
            LoginRequested(this, "https://auth.crowdcontrol.live/?connectionID=" + encodedConnectionID);
        }

        private void LoadTokenData()
        {
            var token = ccToken;
            try
            {
                string[] tokenParts = token.Split('.');
                string base64Payload = tokenParts.Length > 1 ? tokenParts[1] : string.Empty;

                byte[] base64Bytes = Convert.FromBase64String(base64Payload);
                string decodedJson = Encoding.UTF8.GetString(base64Bytes);

                tokenData = JsonConvert.DeserializeObject<TokenData>(decodedJson);
                ccUID = tokenData.ccUID;
                TokenObtained(this, ccToken);
                TokenDataObtained(this, tokenData);
                UIDObtained(this, ccUID);
                Log($"Logged in as {tokenData.name}({tokenData.originID}) from {tokenData.profileType} | ccUID {tokenData.ccUID}");

            }
            catch (Exception ex)
            {
                Log("Error Parsing Token: " + ex.ToString());
            }
        }

        public TokenData getTokenData()
        {
            return tokenData;
        }

        private void BeginSubscribe()
        {
            Log("Subscribing to Events Feed");
            var subscribeData = JsonConvert.SerializeObject(new
            {
                token = ccToken,
                topics = new List<string> { "prv/self", "pub/self" }
            });
            var subscribePayload = JsonConvert.SerializeObject(new
            {
                action = "subscribe",
                data = subscribeData
            });
            socket.Send(subscribePayload);
        }


    }
}
