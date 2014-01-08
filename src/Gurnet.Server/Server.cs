using Gurnet.Core;
using Gurnet.Core.Log;
using Gurnet.Server.Enums;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gurnet.Server
{
    public class GurnetServer
    {
        private string serverName;
        private int port;
        private ILogger logger;
        private Thread gameThread;
        private Game game;

        public StatusEnum Status { get; private set; }
        public bool IsGameRunning
        {
            get
            {
                if (this.game != null)
                {
                    return this.game.IsRunning;
                }
                return false;
            }
        }

        public GurnetServer(string serverName, int port, ILogger logger)
        {
            this.serverName = serverName;
            this.port = port;
            this.logger = logger;

            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }   
        }

        public void Start()
        {
            this.logger.Log("Starting server...");

            var peerConfig = new NetPeerConfiguration(this.serverName);
            peerConfig.Port = this.port;

            var server = new NetServer(peerConfig);
            server.RegisterReceivedCallback(new SendOrPostCallback(HandleIncomingMessages));
            server.Start();

            this.logger.Log("Server is running...");

            this.Status = StatusEnum.Running;

            this.gameThread = new Thread(new ThreadStart(RunGame));
            this.gameThread.Start();
            Thread.Sleep(500);
        }

        private void RunGame()
        {
            this.game = new Game();

            var scenario = new Scenario(10, 10, new List<Position>()
            {
                new Position(1, 1),
                new Position(8, 8)
            });
            this.game.SetScenario(scenario);

            this.game.Start();
        }

        private void HandleIncomingMessages(object state)
        {
            NetIncomingMessage inMsg;
            NetServer server = (NetServer)state;
            if ((inMsg = server.ReadMessage()) == null) return;

            string text;
            switch (inMsg.MessageType)
            {
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.VerboseDebugMessage:
                    text = inMsg.ReadString();
                    this.logger.Log(text);
                    break;
                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)inMsg.ReadByte();
                    string reason = inMsg.ReadString();
                    this.logger.Log(NetUtility.ToHexString(inMsg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);
                    break;
                case NetIncomingMessageType.Data:
                    text = string.Format("{0} said: {1}", NetUtility.ToHexString(inMsg.SenderConnection.RemoteUniqueIdentifier), inMsg.ReadString());
                    this.logger.Log(text);

                    foreach (var con in server.Connections)
                    {
                        var outMsg = server.CreateMessage(text);
                        con.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered, 0);
                    }

                    break;
                default:
                    this.logger.Log("Unhandled type: " + inMsg.MessageType + " " + inMsg.LengthBytes + " bytes " + inMsg.DeliveryMethod + "|" + inMsg.SequenceChannel);
                    break;
            }
            server.Recycle(inMsg);
        }
    }
}
