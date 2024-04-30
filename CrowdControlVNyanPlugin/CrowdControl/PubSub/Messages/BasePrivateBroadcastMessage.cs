using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class BasePrivateBroadcastMessage : BaseMessage
    {
        public new string domain { get; set; }  = "prv";
    }
}
