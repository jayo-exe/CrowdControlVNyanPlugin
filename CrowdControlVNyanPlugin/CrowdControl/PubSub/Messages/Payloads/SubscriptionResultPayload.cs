using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Payloads
{
    public class SubscriptionResultPayload : BaseEventPayload
    {
        public string[] success { get; set; }
        public string[] failure { get; set; }
    }
}
