using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class WhoAmIMessage : BaseDirectBroadcastMessage
    {
        public new string type { get; set; } = "whoami";
        public new Payloads.WhoAmIPayload payload { get; set; }
    }
}
