using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class BaseMessage
    {
        public string domain { get; set; }

        public string type { get; set; }

        public long timestamp { get; set; }

        public object payload { get; set; }
    }
}
