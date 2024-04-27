using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities
{
    public class EffectEntity
    {
        public string type { get; set; } = "game";
        public string effectID { get; set; } = "";
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public string image { get; set; } = "";
        public int price { get; set; } = 0;
        public Effects.EffectQuantityRange quantity { get; set; } = new Effects.EffectQuantityRange();
        public int duration { get; set; } = 0;

    }
}
