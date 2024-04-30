using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin
{
    public class TriggerHistoryRecord
    {
        public string timestamp { get; set; } = "";
        public string effectID { get; set; } = "";
        public string effectName { get; set; } = "";
        public string effectSender { get; set; } = "";
        public string triggerName { get; set; } = "";
    }
}
