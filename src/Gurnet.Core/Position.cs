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

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position()
        {
            this.X = -1;
            this.Y = -1;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == this.GetType() 
                && ((Position)obj).X == this.X 
                && ((Position)obj).Y == this.Y;
        }

        public override int GetHashCode()
        {
            return (this.X * 251) + this.Y;
        }
    }
}
