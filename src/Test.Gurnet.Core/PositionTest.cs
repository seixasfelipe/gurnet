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
            Assert.AreEqual(position.X, -1);
            Assert.AreEqual(position.Y, -1);
        }

        [TestMethod]
        public void TestConstructorParams()
        {
            Position position = new Position(1, 10);
            Assert.IsNotNull(position);
            Assert.AreEqual(position.X, 1);
            Assert.AreEqual(position.Y, 10);
        }

        [TestMethod]
        public void TestEquals()
        {
            Position firstPosition = new Position { X = 10, Y = 20 };
            Position secondPosition = new Position { X = 10, Y = 20 };

            Assert.IsTrue(firstPosition.Equals(secondPosition));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            Position position = new Position { X = 10, Y = 20 };
            Assert.AreEqual(2530, position.GetHashCode());
        }
    }
}
