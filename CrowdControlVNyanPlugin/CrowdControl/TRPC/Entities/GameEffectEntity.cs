using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities
{
    public class GameEffectEntity
    {
        public string type { get; set; } = "game";
        public string effectID { get; set; } = "";
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public string image { get; set; } = "";
        public List<string> category { get; set; } = new List<string>();
        public int price { get; set; } = 0;
        public PubSub.Messages.Entities.Effects.EffectQuantityRange quantity { get; set; } = new PubSub.Messages.Entities.Effects.EffectQuantityRange();
        public GameEffectDuration duration { get; set; }
    }
}
