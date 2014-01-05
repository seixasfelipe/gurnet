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
            string host = "localhost";
            int port = 14242;
            ILogger logger = new ConsoleLogger();
            GurnetServer server = new GurnetServer(host, port, logger);

            server.Start();
            Assert.AreEqual(StatusEnum.Running, server.Status);
        }
    }
}
