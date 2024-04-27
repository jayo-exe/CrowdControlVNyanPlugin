using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class GameSessionStartMessage : BasePublicBroadcastMessage
    {
        public new string type { get; set; } = "game-session-start";
        public new Payloads.GameSessionStartPayload payload { get; set; }
    }
}
