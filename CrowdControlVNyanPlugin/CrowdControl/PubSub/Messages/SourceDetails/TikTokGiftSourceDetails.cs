using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    public class TikTokGiftSourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "tiktok-gift";
        public string giftName { get; set; }
        public int giftID { get; set; }
        public decimal cost { get; set; }
        public string name { get; set; }
        public string userID { get; set; }
    }
}
