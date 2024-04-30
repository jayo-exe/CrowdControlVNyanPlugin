using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities
{
    public class UserEntity
    {
        public string ccUID { get; set; } = "";
        public string name { get; set; } = "";
        public string profile { get; set; } = "";
        public string originID { get; set; } = "";
        public List<string> subscriptions { get; set; } = new List<string>();
        public List<string> roles { get; set; } = new List<string>();
        public string image { get; set; } = "";
    }
}
