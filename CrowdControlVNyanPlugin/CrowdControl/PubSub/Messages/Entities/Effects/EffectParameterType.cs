using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities.Effects
{
    public class EffectParameterType
    {
        public string type { get; set; }
        public Dictionary<string, EffectParameterOption> options { get; set; }
    }
}
