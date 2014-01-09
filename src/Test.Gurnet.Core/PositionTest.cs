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
    public class PositionTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            Position position = new Position();
            Assert.IsNotNull(position);
        }

        [TestMethod]
        public void TestXProperty()
        {
            Position position = new Position { X = 1 };
            Assert.AreEqual(1, position.X);
        }

        [TestMethod]
        public void TestYProperty()
        {
            Position position = new Position { Y = 1 };
            Assert.AreEqual(1, position.Y);
        }
    }
}
