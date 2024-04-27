using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    public class CrowdControlBotSourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "crowd-control-bot";
    }
}
