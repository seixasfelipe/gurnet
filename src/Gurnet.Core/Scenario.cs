using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Core
{
    public class Scenario
    {
        private int lines;
        private int columns;

        public Scenario(int lines, int columns)
        {
            this.lines = lines;
            this.columns = columns;
        }

        public bool IsPositionValid(Position p)
        {
            return (p.X >= 0 && p.X < this.columns)
                && (p.Y >= 0 && p.Y < this.lines);
        }
    }
}
