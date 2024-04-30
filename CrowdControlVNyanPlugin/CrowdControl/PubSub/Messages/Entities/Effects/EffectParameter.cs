using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities.Effects
{
    public class EffectParameter
    {
        public string name { get; set; }
        public string title { get; set; }
        public string value { get; set; }
        public EffectParameterType type { get; set; }
    }
}
