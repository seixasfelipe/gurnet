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
        sealed class MockMessageProcessor: IMessageProcessor
        {
            public string Message { get; set; }
            public int MessageBits { get; set; }

            public void ProcessIncomingMessage(NetIncomingMessage incMsg)
            {
                if (incMsg == null)
                    throw new ArgumentNullException("incMsg cannot be null");

                MessageBits = incMsg.LengthBits;
                Message = Encoding.UTF8.GetString(incMsg.Data, 0, incMsg.Data.Length);
            }
        }

        private GurnetServer GetsNewGurnetServer(ILogger logger, IMessageProcessor processor, string name = "gurnet", int port = 14242)
        {
            if (logger == null)
            {
                logger = new ConsoleLogger();
                logger.SetContext("Server");
            }
            
            GurnetServer server = new GurnetServer(name, port, logger, processor);
            
            return server;
        }
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
            GurnetServer server = GetsNewGurnetServer();
            
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
        private NetIncomingMessage CreateIncomingMessage(byte[] fromData, int bitLength, NetIncomingMessageType messageType = NetIncomingMessageType.StatusChanged)
        {
            NetIncomingMessage inc = (NetIncomingMessage)Activator.CreateInstance(typeof(NetIncomingMessage), true);
            typeof(NetIncomingMessage).GetField("m_incomingMessageType", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inc, messageType);
            typeof(NetIncomingMessage).GetField("m_data", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inc, fromData);
            typeof(NetIncomingMessage).GetField("m_bitLength", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inc, bitLength);
            return inc;
        }
    }
}
