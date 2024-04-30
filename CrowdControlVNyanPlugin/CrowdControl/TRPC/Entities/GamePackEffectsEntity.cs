using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities
{
    public class GamePackEffectsEntity
    {
        public Dictionary<string, GameEffectEntity> game { get; set; }
    }
}
