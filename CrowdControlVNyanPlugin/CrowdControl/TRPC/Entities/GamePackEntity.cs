using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities
{
    public class GamePackEntity
    {
        public string gamePackID { get; set; }
        public PubSub.Messages.Entities.GameEntity game { get; set; }
        public GamePackMetaEntity meta { get; set; }
        public GamePackEffectsEntity effects { get; set; }
        public string proExclusive { get; set; }
        public string image { get; set; }
    }
}
