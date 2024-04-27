using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl
{
    public class TokenData
    {
        public string type = "user";
        public string jti { get; set; }
        public string ccUID { get; set; }
        public string originID { get; set; }
        public string profileType { get; set; }
        public string name { get; set; }
        public List<string> subscriptions { get; set; }
        public List<string> roles { get; set; }
        public long exp { get; set; }
        public string ver { get; set; }
    }
}
