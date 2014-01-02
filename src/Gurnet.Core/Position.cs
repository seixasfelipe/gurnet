using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Core
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {
            this.X = -1;
            this.Y = -1;
        }
    }
}
