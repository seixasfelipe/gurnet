using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Gurnet.Core.Log;
using Gurnet.Server;
using Gurnet.Server.Enums;
using System.Threading;

namespace Test.Gurnet.Server
{
    [TestClass]
    public class ServerTest
    {
        [TestMethod]
        public void TestServerStartAlsoStartsTheGame()
        {
            string name = "gurnet";
            int port = 14242;
            ILogger logger = new ConsoleLogger();
            GurnetServer server = new GurnetServer(name, port, logger);

            Assert.IsFalse(server.IsGameRunning);

            server.Start();

            // TODO: Improve test to handle multiple threads correctly,
            // maybe use asserts as callbacks
            Thread.Sleep(1000);

            Assert.AreEqual(StatusEnum.Running, server.Status);
            Assert.IsTrue(server.IsGameRunning);
        }
    }
}
