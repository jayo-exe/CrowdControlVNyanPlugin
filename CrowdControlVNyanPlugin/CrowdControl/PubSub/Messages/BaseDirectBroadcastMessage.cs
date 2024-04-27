using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class BaseDirectBroadcastMessage : BaseMessage
    {
        public new string domain { get; set; } = "direct";
    }
}
