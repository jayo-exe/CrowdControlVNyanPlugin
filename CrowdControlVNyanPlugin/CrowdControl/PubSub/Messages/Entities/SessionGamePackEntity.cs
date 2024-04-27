using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities
{
    public class SessionGamePackEntity
    {
        public string gamePackID { get; set; }
        public GameEntity game { get; set; }
        public GamePackMetaEntity meta { get; set; }

    }
}
