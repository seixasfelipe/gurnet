using Gurnet.Core.Log;
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
        private ILogger logger;
        public bool IsRunning { get; private set; }
        public Scenario Scenario { get; private set; }

        public Game(ILogger logger)
        {
            this.players = new List<Player>();
            this.logger = logger;
        }

        public void AddPlayer(string name)
        {
            logger.Log("Adding player with name [{0}]", name);
            
            var uniqueId = this.GeneratePlayerId();

            logger.Log("Player ID is [{0}]", uniqueId);

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
            this.logger.Log("Starting game...");

            this.IsRunning = false;

            if (Scenario == null)
            {
                this.logger.Log("Game has not started: missing scenario.");
                return;
            }

            var playersPositioned = this.SetPlayersPositioning();

            this.IsRunning = true;

            this.logger.Log("Game has started.");
        }

        private bool SetPlayersPositioning()
        {
            this.logger.Log("Setting players positioning.");

            var players = this.GetPlayers();
            foreach (var p in players)
            {
                p.MoveTo(this.Scenario.GetNextRespawnPosition());
            }

            this.logger.Log("Players positioned.");
            return true;
        }

        public void SetScenario(Scenario scenario)
        {
            this.Scenario = scenario;
        }

        public void Stop()
        {
            this.logger.Log("Stop requested.");

            this.ResetPlayersPositioning();
            this.IsRunning = false;

            this.logger.Log("Stopped.");
        }

        private void ResetPlayersPositioning()
        {
            this.GetPlayers().ForEach((p) => p.MoveTo(new Position()));
        }
    }
}
