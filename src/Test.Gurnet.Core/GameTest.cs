using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gurnet.Core;
using System.Collections.Generic;
using Gurnet.Core.Log;

namespace Test.Gurnet.Core
{
    [TestClass]
    public class GameTest
    {
        private Game game;
        private Scenario scenario;

        [TestInitialize]
        public void BeforeEach()
        {
            var logger = new ConsoleLogger();
            logger.SetContext("Game");
            var respawnPositions = new List<Position>() { 
                new Position() { X = 1, Y = 1 }, 
                new Position() { X = 8, Y = 8 }
            };
            this.scenario = new Scenario(10, 10, respawnPositions);
            this.game = new Game(logger);
            game.SetScenario(scenario);
            game.AddPlayer("john");
            game.AddPlayer("moe");
        }

        [TestMethod]
        public void TestAddPlayer()
        {
            var players = game.GetPlayers();

            Assert.IsTrue(players.Count == 2);
            Assert.IsFalse(string.IsNullOrEmpty(players[0].Id));
            Assert.IsFalse(string.IsNullOrEmpty(players[1].Id));
        }

        [TestMethod]
        public void TestRemovePlayer()
        {
            var players = game.GetPlayers();
            var p1 = players[0];
            var p2 = players[1];
            game.RemovePlayer(p1.Id);

            Assert.IsTrue(players.Count == 1);
            Assert.IsTrue(p2.Id == players[0].Id);
        }

        [TestMethod]
        public void TestStartGameWithNoPlayers()
        {
            var p1 = game.GetPlayers()[0];
            var p2 = game.GetPlayers()[1];
            game.RemovePlayer(p1.Id);
            game.RemovePlayer(p2.Id);

            game.Start();

            Assert.IsTrue(game.IsRunning);
        }

        [TestMethod]
        public void TestStartGameWithoutAScenario()
        {
            game.Start();
            Assert.IsTrue(game.IsRunning);

            game.SetScenario(null);
            game.Start();

            Assert.IsFalse(game.IsRunning);
        }

        [TestMethod]
        public void TestStartGamePlacingPlayersInsideScenario()
        {
            game.SetScenario(this.scenario);
            game.Start();

            var players = game.GetPlayers();
            var p1 = players[0];
            var p2 = players[1];

            Assert.IsTrue(game.IsRunning);
            Assert.IsTrue(p1.Position.Equals(new Position() { X = 1, Y = 1 }));
            Assert.IsTrue(p2.Position.Equals(new Position() { X = 8, Y = 8 }));
        }

        [TestMethod]
        public void TestStopGame()
        {
            game.Start();

            game.Stop();

            var players = game.GetPlayers();
            var p1 = players[0];
            var p2 = players[1];

            var unsetPosition = new Position();

            Assert.IsFalse(game.IsRunning);
            Assert.IsTrue(p1.Position.Equals(unsetPosition));
            Assert.IsTrue(p2.Position.Equals(unsetPosition));
        }
    }
}
