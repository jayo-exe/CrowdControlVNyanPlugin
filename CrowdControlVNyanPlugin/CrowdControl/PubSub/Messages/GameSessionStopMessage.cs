using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class GameSessionStopMessage : BasePublicBroadcastMessage
    {
        public new string type { get; set; } = "game-session-stop";
        public new Payloads.GameSessionStopPayload payload { get; set; }
    }
}
