using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class EffectSuccessMessage : BasePrivateBroadcastMessage
    {
        public new string type { get; set; } = "effect-success";
        public new Payloads.EffectRequestPayload payload { get; set; }
    }
}
