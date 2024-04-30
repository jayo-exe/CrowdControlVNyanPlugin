using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    public class StreamLabsDonationSourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "stream-labs-donation";
        public string donationID { get; set; }
        public string cost { get; set; }
        public string currency { get; set; }
        public string name { get; set; }
        public string message { get; set; }
    }
}
