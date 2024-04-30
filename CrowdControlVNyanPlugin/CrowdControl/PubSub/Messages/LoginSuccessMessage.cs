using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages
{
    public class LoginSuccessMessage : BaseDirectBroadcastMessage
    {
        public new string type { get; set; } = "login-success";
        public new Payloads.LoginSuccessPayload payload { get; set; }
    }
}
