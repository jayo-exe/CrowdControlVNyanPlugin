using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class EffectRefundMessage : BasePrivateBroadcastMessage
    {
        public new string type { get; set; } = "effect-refund";
        public new Payloads.EffectRequestPayload payload { get; set; }
    }
}
