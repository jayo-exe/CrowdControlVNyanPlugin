using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities
{
    public class GameSessionEntity
    {
        public string gameSessionID { get; set; } = "";
        public string createdAt { get; set; } = "";
        public string updatedAt { get; set; } = "";
        public string profileType { get; set; } = "";
        public string menuName { get; set; } = "";
        public string bankID { get; set; } = "";
        public string gamePackID { get; set; } = "";
        public string ccUID { get; set; } = "";
        public UserEntity owner { get; set; } = new UserEntity();
        public SessionGamePackEntity gamePack { get; set; } = new SessionGamePackEntity();
    }
}
