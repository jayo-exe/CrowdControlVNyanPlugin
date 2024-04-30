using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Payloads
{
    public class EffectFailurePayload : EffectRequestPayload
    {
        public bool? temporary { get; set; }
        public string message { get; set; }
    }
}
