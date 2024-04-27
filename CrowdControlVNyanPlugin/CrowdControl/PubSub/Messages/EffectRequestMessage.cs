using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class EffectRequestMessage : BasePrivateBroadcastMessage
    {
        public new string type { get; set; } = "effect-request";
        public new Payloads.EffectRequestPayload payload { get; set; }
    }
}
