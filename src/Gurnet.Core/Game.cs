using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Core
{
    public class Game
    {
        private List<Player> players;
        public bool IsRunning { get; private set; }
        public Scenario Scenario { get; private set; }

        public Game()
        {
            this.players = new List<Player>();
        }

        public void AddPlayer(string name)
        {
            var uniqueId = this.GeneratePlayerId();
            var pl = new Player(name, uniqueId);
            this.players.Add(pl);
        }

        public void RemovePlayer(string playerId)
        {
            var player = this.players.Find((p) => p.Id == playerId);
            this.players.Remove(player);
        }

        public List<Player> GetPlayers()
        {
            return this.players;
        }

        private string GeneratePlayerId()
        {
            return new Guid().ToString();
        }

        public void Start()
        {
            this.IsRunning = false;

            if (this.GetPlayers().Count < 2
                || this.Scenario == null)
            {
                return;
            }

            var playersPositioned = this.PlacePlayers();
            if (!playersPositioned)
            {
                return;
            }

            this.IsRunning = true;
        }

        private bool PlacePlayers()
        {
            var players = this.GetPlayers();
            foreach (var p in players)
            {
                p.MoveTo(this.Scenario.GetNextRespawnPosition());
            }

            return true;
        }

        public void SetScenario(Scenario scenario)
        {
            this.Scenario = scenario;
        }
    }
}
