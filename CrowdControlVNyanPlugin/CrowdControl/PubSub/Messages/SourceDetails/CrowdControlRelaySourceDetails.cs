using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    public class CrowdControlRelaySourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "crowd-control-relay";
    }
}
