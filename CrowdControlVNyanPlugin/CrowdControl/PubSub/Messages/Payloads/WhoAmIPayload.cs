using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Payloads
{
    public class WhoAmIPayload : BaseEventPayload
    {
        public string connectionID { get; set; }
    }
}
