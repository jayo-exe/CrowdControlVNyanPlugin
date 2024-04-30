using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.TRPC.Entities
{
    public class GamePackMetaEntity
    {
        public string name { get; set; }
        public string releaseDate { get; set; }
        public string platform { get; set; }
        public string visibility { get; set; }
        public string guide { get; set; }
        public bool patch { get; set; }
        public string emulator { get; set; }
        public List<string> connector { get; set; }
    }
}
