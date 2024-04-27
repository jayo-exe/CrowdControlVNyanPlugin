using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    //Dummy Class for sourceDetails to avoid complexities from inconsistent object shape.  This doesn't exist, in reality "standard" effect payloads don't have a sourceDetails member
    public class ViewerSourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "viewer";
    }
}
