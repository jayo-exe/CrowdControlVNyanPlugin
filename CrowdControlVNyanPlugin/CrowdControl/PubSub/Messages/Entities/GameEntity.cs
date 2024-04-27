using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdControlVNyanPlugin.CrowdControl.PubSub.Messages.Entities
{
    public class GameEntity
    {
        public string gameID { get; set; } = "";
        public object name { get; set; } = "";
        public string releaseDate { get; set; } = "";
        public bool recommended { get; set; } = false;
        public List<string> keywords { get; set; } = new List<string>();
        public List<string> packs { get; set; } = new List<string>();
        public List<string> platforms { get; set; } = new List<string>();
        public string image { get; set; } = "";
    }
}
