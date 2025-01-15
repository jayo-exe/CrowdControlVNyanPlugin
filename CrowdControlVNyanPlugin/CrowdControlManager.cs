using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CrowdControlVNyanPlugin.CrowdControl;
using CrowdControlVNyanPlugin.CrowdControl.PubSub;
using CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages;
using CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities;
using CrowdControlVNyanPlugin.CrowdControl.TRPC;
using CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities;
using CrowdControlVNyanPlugin.VNyanPluginHelper;

using VNyanInterface;

namespace CrowdControlVNyanPlugin
{

    

    public class CrowdControlManager : MonoBehaviour
    {

        private CrowdControlVNyanPlugin plugin;
        
        public Material screenMaterial;
        public Texture baseScreenTexture;
        public List<TriggerHistoryRecord> triggerHistory;
        
        private RawImage displayScreenRenderer;

        private CrowdControlPubSubClient pubsub;
        private CrowdControlTRPCClient trpc;
        private Texture currentScreenTexture;
        private GameSessionEntity gameSession;

        private TokenData tokenData;
        private string ccUID;
        public List<GameEntity> games;
        private GamePackEntity gamePack;

        public string GetAccessToken()
        {
            return plugin.ccToken;
        }

        public string GetCcUID()
        {
            return ccUID;
        }

        private void Awake()
        {
            triggerHistory = new List<TriggerHistoryRecord>();
            plugin = GetComponent<CrowdControlVNyanPlugin>();
            screenMaterial = plugin.screenMaterial;
            baseScreenTexture = plugin.baseScreenTexture;
            displayScreenRenderer = plugin.displayScreenRenderer;

            pubsub = new CrowdControlPubSubClient();
            pubsub.PubSubStart += OnPubSubStart;
            pubsub.PubSubFailure += OnPubSubFailure;
            pubsub.PubSubClose += OnPubSubClose;
            pubsub.PubSubReady += OnPubSubReady;
            pubsub.LoginRequested += OnLoginRequested;
            pubsub.TokenObtained += OnTokenObtained;
            pubsub.TokenDataObtained += OnTokenDataObtained;
            pubsub.UIDObtained += OnUIDObtained;

            pubsub.LogMessage += OnPubSubLog;

            pubsub.GameSessionStart += OnGameSessionStart;
            pubsub.GameSessionStop += OnGameSessionStop;
            pubsub.EffectRequest += OnEffectRequest;
            pubsub.EffectRetry += OnEffectRetry;
            pubsub.EffectRefund += OnEffectRefund;
            pubsub.EffectSuccess += OnEffectSuccess;
            pubsub.EffectFailure += OnEffectFailure;
            pubsub.TimedEffectUpdate += OnTimedEffectUpdate;

            trpc = new CrowdControlTRPCClient();
            trpc.LogMessage += OnTRPCLog;

            EditorApplication.playModeStateChanged += ModeChanged;

        }

        private void Start()
        {
            setStringParam("_xcc_message", "Crowd Control Manager Loaded!");
            Debug.Log("[CrowdControlPlugin] Crowd Control Manager Loaded!");
            currentScreenTexture = baseScreenTexture;
            displayScreenRenderer.texture = currentScreenTexture;

            MainThreadDispatcher.Enqueue(() => { plugin.setStatusTitle("Not Authorized"); });
            MainThreadDispatcher.Enqueue(() => { plugin.setConnectionStatusTitle("Not Connected"); });
        }

        public void initCrowdControl()
        {
            pubsub.initSocket(plugin.ccToken);
        }

        public void deInitCrowdControl()
        {
            pubsub.deInitSocket();
            tokenData = null;
            plugin.ccToken = null;
            plugin.savePluginSettings();
            MainThreadDispatcher.Enqueue(() => { 
                plugin.setStatusTitle("Not Authorized");
                plugin.setConnectionStatusTitle("Not Connected");
                plugin.setGameSessionInfo("[No Active Session]", "", "");
            });
        }

        private void Update()
        {
            if (displayScreenRenderer == null) return;
            if (displayScreenRenderer.texture == currentScreenTexture) return;
            displayScreenRenderer.texture = currentScreenTexture;
        }


        private void ModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                Debug.Log("[CrowdControlPlugin] Exiting Play Mode");
                pubsub.deInitSocket();
            }
        }

        private void OnDisable()
        {
            pubsub.deInitSocket();
        }

        private void OnTRPCLog(object sender, object message)
        {
            //Debug.Log($"[CC TRPC] " + message.ToString());
        }
        private void OnPubSubStart(object sender, EventArgs e)
        {
            //currentMaterial = pendingMaterial;
        }

        private void OnPubSubClose(object sender, EventArgs e)
        {
            MainThreadDispatcher.Enqueue(() => { plugin.setConnectionStatusTitle("Closed"); });
            trpc.Deactivate();
        }

        private void OnPubSubReady(object sender, EventArgs e)
        {
            MainThreadDispatcher.Enqueue(() => { plugin.setConnectionStatusTitle("Connected"); });
        }

        private void OnPubSubFailure(object sender, EventArgs e)
        {
            MainThreadDispatcher.Enqueue(() => { plugin.setConnectionStatusTitle("Failed"); });
            trpc.Deactivate();
        }

        private void OnPubSubLog(object sender, object message)
        {
            //Debug.Log($"[CC PubSub] " + message.ToString());
        }

        private void OnLoginRequested(object sender, string loginURL)
        {
            MainThreadDispatcher.Enqueue(() => {
                plugin.setStatusTitle("Waiting for Authorization");
                Application.OpenURL(loginURL);
            });
        }

        private void OnTokenObtained(object sender, string token)
        {
            plugin.ccToken = token;
            trpc.Activate(token);
        }

        private void OnTokenDataObtained(object sender, TokenData tokenDataObject)
        {
            tokenData = tokenDataObject;
            MainThreadDispatcher.Enqueue(() => { plugin.setStatusTitle($"{tokenData.name} ({tokenData.profileType})"); });
        }

        private async void OnUIDObtained(object sender, string UID)
        {
            ccUID = UID;
            var games = await trpc.getGames();
            fetchGameSession();
        }

        private void OnGameSessionStart(object sender, GameSessionStartMessage message)
        {
            Debug.Log($"[CrowdControlPlugin] Game Session {message.payload.gameSessionID} Started!");
            fetchGameSession();
            MainThreadDispatcher.Enqueue(() => {
                plugin.triggerBrowserSessionText.SetActive(false);
                plugin.triggerHistorySessionText.SetActive(false);
                setStringParam("_xcc_gameSessionID", message.payload.gameSessionID);
            });
            

        }

        private void OnGameSessionStop(object sender, GameSessionStopMessage message)
        {
            Debug.Log($"[CrowdControlPlugin] Game Session {message.payload.gameSessionID} Stopped!");
            MainThreadDispatcher.Enqueue(() =>
            {
                setStringParam("_xcc_gameSessionID", "");
            });
            currentScreenTexture = baseScreenTexture;
            clearGameSession();
        }

        private void OnEffectRequest(object sender, EffectRequestMessage message)
        {
            var payload = message.payload;

            MainThreadDispatcher.Enqueue(() => {
                setStringParam("_xcc_erq_effectID", payload.effect.effectID);
                setStringParam("_xcc_erq_name", payload.effect.name);
                setFloatParam("_xcc_erq_quantity", (float)payload.quantity);
                setFloatParam("_xcc_erq_duration", (float)payload.effect.duration);
                setFloatParam("_xcc_erq_price", payload.price);
                setFloatParam("_xcc_erq_remaining", (float)payload.timeRemaining);
                setStringParam("_xcc_erq_sender", payload.requester.name);
            });

            callEffectTrigger("erq", payload.effect.effectID, payload.effect.name, payload.requester.name);
        }

        private void OnEffectRetry(object sender, EffectRetryMessage message)
        {
            var payload = message.payload;

            MainThreadDispatcher.Enqueue(() => {
                setStringParam("_xcc_ert_effectID", payload.effect.effectID);
                setStringParam("_xcc_ert_name", payload.effect.name);
                setFloatParam("_xcc_ert_quantity", (float)payload.quantity);
                setFloatParam("_xcc_ert_duration", (float)payload.effect.duration);
                setFloatParam("_xcc_ert_price", payload.price);
                setFloatParam("_xcc_ert_remaining", (float)payload.timeRemaining);
                setStringParam("_xcc_ert_sender", payload.requester.name);
            });
            
            callEffectTrigger("ert", payload.effect.effectID, payload.effect.name, payload.requester.name);
        }

        private void OnEffectRefund(object sender, EffectRefundMessage message)
        {
            var payload = message.payload;

            MainThreadDispatcher.Enqueue(() => {
                setStringParam("_xcc_erf_effectID", payload.effect.effectID);
                setStringParam("_xcc_erf_name", payload.effect.name);
                setFloatParam("_xcc_erf_quantity", (float)payload.quantity);
                setFloatParam("_xcc_erf_duration", (float)payload.effect.duration);
                setFloatParam("_xcc_erf_price", payload.price);
                setFloatParam("_xcc_erf_remaining", (float)payload.timeRemaining);
                setStringParam("_xcc_erf_sender", payload.requester.name);
            });   

            callEffectTrigger("erf", payload.effect.effectID, payload.effect.name, payload.requester.name);
        }

        private void OnEffectSuccess(object sender, EffectSuccessMessage message)
        {
            var payload = message.payload;

            MainThreadDispatcher.Enqueue(() => {
                setStringParam("_xcc_esc_effectID", payload.effect.effectID);
                setStringParam("_xcc_esc_name", payload.effect.name);
                setFloatParam("_xcc_esc_quantity", (float)payload.quantity);
                setFloatParam("_xcc_esc_duration", (float)payload.effect.duration);
                setFloatParam("_xcc_esc_price", payload.price);
                setFloatParam("_xcc_esc_remaining", (float)payload.timeRemaining);
                setStringParam("_xcc_esc_sender", payload.requester.name);
            });
            
            callEffectTrigger("esc", payload.effect.effectID, payload.effect.name, payload.requester.name);
        }

        private void OnEffectFailure(object sender, EffectFailureMessage message)
        {
            var payload = message.payload;

            MainThreadDispatcher.Enqueue(() => {
                setStringParam("_xcc_efl_effectID", payload.effect.effectID);
                setStringParam("_xcc_efl_name", payload.effect.name);
                setFloatParam("_xcc_efl_quantity", (float)payload.quantity);
                setFloatParam("_xcc_efl_duration", (float)payload.effect.duration);
                setFloatParam("_xcc_efl_price", payload.price);
                setFloatParam("_xcc_efl_remaining", (float)payload.timeRemaining);
                setStringParam("_xcc_efl_sender", payload.requester.name);
            });
            
            callEffectTrigger("efl", payload.effect.effectID, payload.effect.name, payload.requester.name);
        }

        private void OnTimedEffectUpdate(object sender, TimedEffectUpdateMessage message)
        {
            var payload = message.payload;

            MainThreadDispatcher.Enqueue(() => {
                setStringParam("_xcc_teu_effectID", payload.effect.effectID);
                setStringParam("_xcc_teu_name", payload.effect.name);
                setFloatParam("_xcc_teu_quantity", (float)payload.quantity);
                setFloatParam("_xcc_teu_duration", (float)payload.effect.duration);
                setFloatParam("_xcc_teu_price", payload.price);
                setFloatParam("_xcc_teu_remaining", (float)payload.timeRemaining);
                setStringParam("_xcc_teu_sender", payload.requester.name);
                setStringParam("_xcc_teu_status", payload.status);
            });
            
            callEffectTrigger("teu", payload.effect.effectID, payload.effect.name, payload.requester.name, $"_{payload.status}");
        }

        private void callEffectTrigger(string code, string effectID, string effectName, string effectSender, string suffix = "")
        {
            MainThreadDispatcher.Enqueue(() => {
                setStringParam("_xcc_trigger", $"_xcc_{code}_{effectID}{suffix}");
                VNyanInterface.VNyanInterface.VNyanTrigger.callTrigger($"_xcc_{code}_{effectID}{suffix}", 0, 0, 0, "", "", "");
                triggerHistory.Add(new TriggerHistoryRecord
                {
                    timestamp = System.DateTime.Now.ToString("hh:mm:ss"),
                    effectID = effectID,
                    effectName = effectName,
                    effectSender = effectSender,
                    triggerName = $"_xcc_{code}_{effectID}{suffix}"
                });
                plugin.populateHistoryList();
            });
        }

        private void setStringParam(string name, string value)
        {
            VNyanInterface.VNyanInterface.VNyanParameter.setVNyanParameterString(name, value);
        }

        private void setFloatParam(string name, float value)
        {
            VNyanInterface.VNyanInterface.VNyanParameter.setVNyanParameterFloat(name, value);
        }

        private async Task fetchGameSession()
        {
            gameSession = await trpc.GetActiveGameSession(ccUID);

            if (gameSession != null)
            {
                Debug.Log($"[CrowdControlPlugin] Got game session: {gameSession.gameSessionID}! {gameSession.owner.name} is playing {gameSession.gamePack.meta.name}.");
                var gamePacks = await trpc.getGamePacks(gameSession.gamePack.game.gameID);
                foreach(GamePackEntity gp in gamePacks)
                {
                    if(gp.gamePackID == gameSession.gamePack.gamePackID)
                    {
                        gamePack = gp;
                    }
                }
                MainThreadDispatcher.Enqueue(() => { 
                    StartCoroutine(LoadGameTexture(gameSession)); 
                    plugin.setGameSessionInfo(gameSession.gamePack.meta.name, gameSession.gamePack.meta.releaseDate, gameSession.gamePack.meta.platform);
                    plugin.triggerBrowserSessionText.SetActive(false);
                    plugin.triggerHistorySessionText.SetActive(false);
                    plugin.populateBrowserList();
                });
            }
        }

        private async Task clearGameSession()
        {
            gameSession = null;
            gamePack = null;
            MainThreadDispatcher.Enqueue(() => {
                plugin.setGameSessionInfo("[No Active Session]", "", "");
                plugin.triggerBrowserSessionText.SetActive(true);
                plugin.triggerHistorySessionText.SetActive(true);
                plugin.clearBrowserList();
            });
        }

        IEnumerator LoadGameTexture(GameSessionEntity gameSession)
        {

            Debug.Log($"[CrowdControlPlugin] Loading Box Art Texture https://resources.crowdcontrol.live/images/{gameSession.gamePack.game.gameID}/box.jpg");
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture($"https://resources.crowdcontrol.live/images/{gameSession.gamePack.game.gameID}/box.jpg"))
            {
                yield return uwr.SendWebRequest();
                Debug.Log("[CrowdControlPlugin] Texture Request done!");
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    Debug.Log("[CrowdControlPlugin] Loaded External Texture");
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    currentScreenTexture = texture;
                }
            }
            
        }

        public CrowdControlPubSubClient getPubSubClient()
        {
            return pubsub;
        }

        public CrowdControlTRPCClient getTRPCClient()
        {
            return trpc;
        }

        public GamePackEntity getGamePack()
        {
            return gamePack;
        }

    }
}
