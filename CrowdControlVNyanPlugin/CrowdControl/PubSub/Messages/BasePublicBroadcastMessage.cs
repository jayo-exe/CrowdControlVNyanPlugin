using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class BasePublicBroadcastMessage : BaseMessage
    {
        public new string domain { get; set; } = "pub";
    }
}
