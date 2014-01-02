using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Core
{
    public class Player
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public Position Position { get; private set; }

        public Player(string name, string uniqueId)
        {
            this.Name = name;
            this.Id = uniqueId;
            this.Position = new Position();
        }
    }
}
