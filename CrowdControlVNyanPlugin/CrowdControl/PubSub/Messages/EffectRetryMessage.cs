using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class EffectRetryMessage : BasePrivateBroadcastMessage
    {
        public new string type { get; set; } = "effect-retry";
        public new Payloads.EffectRequestPayload payload { get; set; }
    }
}
