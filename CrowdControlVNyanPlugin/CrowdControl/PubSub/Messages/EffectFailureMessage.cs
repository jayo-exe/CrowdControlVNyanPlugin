using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class EffectFailureMessage : BasePrivateBroadcastMessage
    {
        public new string type { get; set; } = "effect-failure";
        public new Payloads.EffectFailurePayload payload { get; set; }
    }
}
