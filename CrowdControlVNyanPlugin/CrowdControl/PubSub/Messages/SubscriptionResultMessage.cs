using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class SubscriptionResultMessage : BaseDirectBroadcastMessage
    {
        public new string type { get; set; } = "subscription-result";
        public new Payloads.SubscriptionResultPayload payload { get; set; }
    }
}
