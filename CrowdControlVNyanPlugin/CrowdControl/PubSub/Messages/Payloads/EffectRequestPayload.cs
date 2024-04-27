using System;
using System.Collections.Generic;
using System.Text;
using CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.SourceDetails;
using CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities.Effects;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Payloads
{
    public class EffectRequestPayload : BaseEventPayload
    {
        public bool? anonymous { get; set; } = false;
        public Entities.EffectEntity effect { get; set; }
        public Entities.GameEntity game { get; set; }
        public Entities.EffectRequestGamePackEntity gamePack { get; set; }
        public Entities.UserEntity origin { get; set; } = new Entities.UserEntity();
        public Dictionary<string, EffectParameter> parameters { get; set; }
        public bool? pooled { get; set; } = false;
        public int price { get; set; } = 0;
        public int unitPrice { get; set; } = 0;
        public int? quantity { get; set; } = 0;
        public Entities.UserEntity requester { get; set; } = new Entities.UserEntity();
        public string requestID { get; set; } = "";
        public Entities.UserEntity target { get; set; }
        public BaseSourceDetails sourceDetails { get; set; } = new ViewerSourceDetails();
        public long? timestamp { get; set; } = 0;
        public string status { get; set; } = "";
        public decimal timeRemaining { get; set; } = 0;

    }
}
