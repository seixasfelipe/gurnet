using Gurnet.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Gurnet.Core
{
    [TestClass]
    public class ScenarioTest
    {
        [TestMethod]
        public void TestScenarioRespawnPositions()
        {
            Position expectedFirstPosition = new Position { X = 1, Y = 1};
            Position expectedSecondPosition = new Position { X = 2, Y = 2 };
            
            Scenario scenario = new Scenario(2, 2, new List<Position> { new Position { X = 1, Y = 1 }, new Position { X = 2, Y = 2 } });

            Assert.AreEqual(expectedFirstPosition, scenario.GetNextRespawnPosition());
            Assert.AreEqual(expectedSecondPosition, scenario.GetNextRespawnPosition());
            Assert.AreEqual(expectedFirstPosition, scenario.GetNextRespawnPosition());
        }
    }
}
