using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities;

namespace CrowdControlVNyanPlugin.CrowdControl.TRPC
{
    public class CrowdControlTRPCClient
    {
        public event EventHandler<object> LogMessage;

        private string ccToken { get; set; } = "";
        private bool active { get; set; } = false;
        private readonly HttpClient client;

        public CrowdControlTRPCClient()
        {
            client = new HttpClient();
            
        }

        public void Log(object message)
        {
            LogMessage(this, message);
        }

        public void Activate(string token)
        {
            if(active)
            {
                return;
            }

            ccToken = token;
            active = true;
            client.DefaultRequestHeaders.Add("Authorization", "cc-auth-token " + ccToken);
        }

        public void Deactivate()
        {
            if (!active)
            {
                return;
            }

            ccToken = "";
            active = false;
            client.DefaultRequestHeaders.Clear();
        }

        public async Task<T> RequestAsync<T>(string method, object payload)
        {
            try
            {
                string url = $"https://trpc.crowdcontrol.live/{method}";
                if (payload != null)
                {
                    var encodedPayload = HttpUtility.UrlEncode(JsonConvert.SerializeObject(payload));
                    url = $"https://trpc.crowdcontrol.live/{method}?input={encodedPayload}";
                }
                
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Log($"TRPC Request {method} Successful");
                    string responseData = await response.Content.ReadAsStringAsync();
                    Log(responseData);
                    var data = JsonConvert.DeserializeObject<T>(responseData);
                    return data;
                }
                else
                {
                    Log($"TRPC Request {method} Failed: {response.ReasonPhrase}");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Log($"TRPC Request {method} Error: {ex.Message}");
                return default(T);
            }
        }

        public async Task<GameSessionEntity> GetActiveGameSession(string ccUID)
        {
            Log($"Trying to get Session for: {ccUID}");
            var result = await RequestAsync<JObject>("gameSession.getUsersActiveGameSession", new { ccUID = ccUID });
            if (result == null) return null;

            return result["result"]["data"]["session"].ToObject<GameSessionEntity>();
        }

        public async Task<List<GameEntity>> getGames()
        {
            Log($"Trying to get Games List");
            var result = await RequestAsync<JObject>("game.getGames", null);
            if (result == null) return null;
            try
            {
                JToken resultObj = result["result"]["data"];
                List<GameEntity> returnObj = new List<GameEntity>();
                foreach(JToken gameItem in result["result"]["data"])
                {
                    if(gameItem["name"] is JObject)
                    {
                        gameItem["name"] = gameItem["name"]["public"];
                    }
                    returnObj.Add(gameItem.ToObject<GameEntity>());
                }
                return returnObj;
            } catch(Exception e)
            {
                Log($"{e.Message} {e.StackTrace}");
                return null;
            }
            
        }

        public async Task<List<Entities.GamePackEntity>> getGamePacks(string gameID)
        {
            Log($"Trying to get Game Packs for: {gameID}");
            var result = await RequestAsync<JObject>("game.getGamePacks", new { gameID = gameID });
            if (result == null) return null;

            try
            {
                JToken resultObj = result["result"]["data"];
                List<Entities.GamePackEntity> returnObj = new List<Entities.GamePackEntity>();
                foreach (JToken gamePackItem in result["result"]["data"])
                {
                    returnObj.Add(gamePackItem.ToObject<Entities.GamePackEntity>());
                }
                Log(JsonConvert.SerializeObject(returnObj));
                return returnObj;
            }
            catch (Exception e)
            {
                Log($"{e.Message} {e.StackTrace}");
                return null;
            }
        }
    }
}
