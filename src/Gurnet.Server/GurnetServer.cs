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
        private NetServer serverInstance;
        public IMessageProcessor MessageProcessor { get; private set; }
        public IMessageTranslator MessageTranslator { get; private set; }

        public List<string> ConnectedClients { get; private set; }
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

        public GurnetServer(string serverName, int port, ILogger logger, IMessageProcessor processor, IMessageTranslator translator)
        {
            this.serverName = serverName;
            this.port = port;
            this.logger = logger;
            this.ConnectedClients = new List<string>();
            this.MessageProcessor = processor;
            this.MessageTranslator = translator;

            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            var peerConfig = new NetPeerConfiguration(this.serverName);
            peerConfig.Port = this.port;

            this.serverInstance = new NetServer(peerConfig);
            serverInstance.RegisterReceivedCallback(new SendOrPostCallback(HandleIncomingMessages));

        }

        public void Start()
        {
            this.logger.Log("Starting server...");

            serverInstance.Start();

            this.logger.Log("Server is running...");

            this.Status = StatusEnum.Running;

            this.gameThread = new Thread(new ThreadStart(RunGame));
            this.gameThread.Start();
            Thread.Sleep(500);
        }

        private void RunGame()
        {
            this.game = new Game(this.logger);

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

        public void ProcessIncomingMessage(NetIncomingMessage incMsg)
        {
            if (MessageProcessor == null)
                return;

            MessageProcessor.ProcessIncomingMessage(incMsg, MessageTranslator);
        }

        public void ExecuteAction(Core.Networking.PacketType actionType, object obj)
        {
            switch (actionType)
            {
                case Core.Networking.PacketType.AddPlayer:
                    var name = obj as string;
                    this.game.AddPlayer(name);
                    break;
            }
        }

        public void Stop()
        {
            this.logger.Log("Stop server requested.");
            
            this.game.Stop();
            this.serverInstance.Shutdown("Server Shutdown requested.");

            this.logger.Log("Server has stopped.");
        }
    }
}
