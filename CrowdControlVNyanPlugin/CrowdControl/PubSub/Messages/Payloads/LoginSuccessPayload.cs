using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Payloads
{
    public class LoginSuccessPayload : BaseEventPayload
    {
        public string token { get; set; }
    }
}
