using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class TimedEffectUpdateMessage : BasePrivateBroadcastMessage
    {
        public new string type { get; set; } = "timed-effect-update";
        public new Payloads.TimedEffectUpdatePayload payload { get; set; }
    }
}
