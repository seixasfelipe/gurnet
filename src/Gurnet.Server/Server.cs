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
        private Thread serverInstance;

        public StatusEnum Status { get; private set; }

        public GurnetServer(string serverName, int port, ILogger logger)
        {
            this.serverName = serverName;
            this.port = port;
            this.logger = logger;
        }

        public void Start()
        {
            this.serverInstance = new Thread(RunServer);

            var param = new {
                serverName = this.serverName, 
                port = this.port, 
                logger = this.logger 
            };
            this.serverInstance.Start(param);
        }

        private void RunServer(dynamic param)
        {
            param.logger.Log("Starting server...");

            var peerConfig = new NetPeerConfiguration(param.serverName);
            peerConfig.Port = param.port;

            var server = new NetServer(peerConfig);
            server.Start();

            while(server.Status != NetPeerStatus.Running) 
            {
                Thread.Sleep(10);
            }

            param.logger.Log("Server is running...");

            this.Status = StatusEnum.Running;

            while (true)
            {
                NetIncomingMessage inMsg;
                string text;
                while ((inMsg = server.ReadMessage()) != null)
                {
                    // handle incoming message
                    switch (inMsg.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            text = inMsg.ReadString();
                            param.logger.Log(text);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)inMsg.ReadByte();
                            string reason = inMsg.ReadString();
                            param.logger.Log(NetUtility.ToHexString(inMsg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);
                            break;
                        case NetIncomingMessageType.Data:
                            text = string.Format("{0} said: {1}", NetUtility.ToHexString(inMsg.SenderConnection.RemoteUniqueIdentifier), inMsg.ReadString());
                            param.logger.Log(text);

                            foreach (var con in server.Connections)
                            {
                                var outMsg = server.CreateMessage(text);
                                con.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered, 0);
                            }

                            break;
                        default:
                            param.logger.Log("Unhandled type: " + inMsg.MessageType + " " + inMsg.LengthBytes + " bytes " + inMsg.DeliveryMethod + "|" + inMsg.SequenceChannel);
                            break;
                    }
                    server.Recycle(inMsg);
                }

                Thread.Sleep(1);
            }
        }
    }
}
