using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Gurnet.Core.Log;
using Gurnet.Server;
using Gurnet.Server.Enums;

namespace Test.Gurnet.Server
{
    [TestClass]
    public class ServerTest
    {
        [TestMethod]
        public void TestServerStarts()
        {
            string name = "gurnet";
            int port = 14242;
            ILogger logger = new ConsoleLogger();
            GurnetServer server = new GurnetServer(name, port, logger);

            server.Start();
            Assert.AreEqual(StatusEnum.Running, server.Status);
        }
    }
}
