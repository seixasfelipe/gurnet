using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Gurnet.Core.Log;
using Gurnet.Server;
using Gurnet.Server.Enums;
using System.Threading;
using Lidgren.Network;
using System.Reflection;
using Gurnet.Core.Networking;

namespace Test.Gurnet.Server
{
    [TestClass]
    public class GurnetServerTest
    {
        //[TestMethod]
        //public void TestServerStartAlsoStartsTheGame()
        //{
        //    string name = "gurnet";
        //    int port = 14242;
        //    ILogger logger = new ConsoleLogger();
        //    logger.SetContext("Server");
        //    GurnetServer server = new GurnetServer(name, port, logger);

        //    Assert.IsFalse(server.IsGameRunning);

        //    server.Start();

        //    // TODO: Improve test to handle multiple threads correctly,
        //    // maybe use asserts as callbacks
        //    Thread.Sleep(1000);

        //    Assert.AreEqual(StatusEnum.Running, server.Status);
        //    Assert.IsTrue(server.IsGameRunning);

        //    server.Stop();
        //}

        //[TestMethod]
        //public void TestWhenPlayerConnectServerShouldAddHimToGame()
        //{
        //    string name = "gurnet";
        //    int port = 14242;
        //    ILogger logger = new ConsoleLogger();
        //    logger.SetContext("Server");
        //    GurnetServer server = new GurnetServer(name, port, logger);

        //    server.Start();

        //    // TODO: Improve test to handle multiple threads correctly,
        //    // maybe use asserts as callbacks
        //    Thread.Sleep(1000);

        //    string playerName = "john";
        //    server.ExecuteAction(ActionType.AddPlayer, playerName);

        //    Assert.IsFalse(true);

        //    server.Stop();
        //}

        [TestMethod]
        public void TestThatServerKeepsTrackOfEveryClientThatConnectToServer()
        {
            string name = "gurnet";
            int port = 14242;
            ILogger logger = new ConsoleLogger();
            logger.SetContext("Server");
            GurnetServer server = new GurnetServer(name, port, logger);
            
            var client = new NetClient(new NetPeerConfiguration("gurnet"));
            var msg = client.CreateMessage(NetConnectionStatus.Connected.ToString());

            var incMsg = CreateIncomingMessage(msg.Data, msg.LengthBits);

            server.Start();

            server.ProcessIncomingMessage(incMsg);

            Assert.AreEqual(1, server.ConnectedClients.Count);
        }

        /// <summary>
        /// Helper method
        /// </summary>
        private NetIncomingMessage CreateIncomingMessage(byte[] fromData, int bitLength)
        {
            NetIncomingMessage inc = (NetIncomingMessage)Activator.CreateInstance(typeof(NetIncomingMessage), true);
            typeof(NetIncomingMessage).GetField("m_data", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inc, fromData);
            typeof(NetIncomingMessage).GetField("m_bitLength", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inc, bitLength);
            return inc;
        }
    }
}
