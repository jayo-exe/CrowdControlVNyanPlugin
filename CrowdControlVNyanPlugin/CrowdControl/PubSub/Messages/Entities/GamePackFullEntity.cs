using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities
{
    public class GamePackFullEntity
    {
        public string gamePackID { get; set; }
        public string name { get; set; }
        public string platform { get; set; }
        public string proExclusive { get; set; }
        public string image { get; set; }
        public Dictionary<string, EffectEntity> effects { get; set; }
    }
}
