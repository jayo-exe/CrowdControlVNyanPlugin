using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    public class CrowdControlRetrySourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "crowd-control-retry";
    }
}
