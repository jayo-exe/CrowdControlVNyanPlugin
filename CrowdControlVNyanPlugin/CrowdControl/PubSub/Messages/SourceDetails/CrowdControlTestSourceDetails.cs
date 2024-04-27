using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    public class CrowdControlTestSourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "crowd-control-test";
    }
}
