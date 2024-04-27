using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails
{
    public class TwitchChannelRewardSourceDetails : BaseSourceDetails
    {
        public string type { get; set; } = "twtich-channel-reward";
        public string requestID { get; set; }
        public string rewardID { get; set; }
        public string redemptionID { get; set; }
        public string twitchID { get; set; }
        public int cost { get; set; }
    }
}
