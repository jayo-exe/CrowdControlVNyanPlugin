using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Payloads
{
    public class GameSessionStartPayload : BaseEventPayload
    {
        public string gameSessionID { get; set; }
    }
}
