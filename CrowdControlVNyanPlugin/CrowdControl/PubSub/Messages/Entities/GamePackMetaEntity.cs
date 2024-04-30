using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities
{
    public class GamePackMetaEntity
    {
        public string name { get; set; } = "";
        public string visibility { get; set; } = "";
        public string releaseDate { get; set; } = "";
        public string platform { get; set; } = "";
        public string note { get; set; } = "";
        public string description { get; set; } = "";
        public bool recommended { get; set; } = false;
        public string emulator { get; set; } = "";
        public string guide { get; set; } = "";
        public List<string> connector { get; set; } = new List<string>();
    }
}
