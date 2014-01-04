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
        private List<Position> respawnPositions;
        private int lastUsedRespawnPositionIndex;

        public Scenario(int lines, int columns, List<Position> respawnPositions)
        {
            this.lines = lines;
            this.columns = columns;
            this.respawnPositions = respawnPositions;
            this.lastUsedRespawnPositionIndex = -1;
        }

        internal Position GetNextRespawnPosition()
        {
            this.lastUsedRespawnPositionIndex = ++lastUsedRespawnPositionIndex;
            if(lastUsedRespawnPositionIndex == this.respawnPositions.Count) 
            {
                this.lastUsedRespawnPositionIndex = 0;
            }
            return this.respawnPositions[this.lastUsedRespawnPositionIndex];
        }
    }
}
