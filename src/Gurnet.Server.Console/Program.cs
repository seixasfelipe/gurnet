using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gurnet.Server
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");

            var config = new NetPeerConfiguration("gurnet");
            config.Port = 14242;

            var server = new NetServer(config);
            server.Start();

            ConsoleKeyInfo key;
            while(true) 
            {
                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Q)
                    {
                        server.Shutdown("Shutdown requested by user");
                        break;
                    }
                }

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
                            Console.WriteLine(text);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)inMsg.ReadByte();
							string reason = inMsg.ReadString();
							Console.WriteLine(NetUtility.ToHexString(inMsg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);
                            break;
                        case NetIncomingMessageType.Data:
                            text = string.Format("{0} said: {1}", NetUtility.ToHexString(inMsg.SenderConnection.RemoteUniqueIdentifier), inMsg.ReadString());
                            Console.WriteLine(text);

                            foreach (var con in server.Connections)
                            {
                                var outMsg = server.CreateMessage(text);
                                con.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered, 0);
                            }

                            break;
                        default:
                            Console.WriteLine("Unhandled type: " + inMsg.MessageType + " " + inMsg.LengthBytes + " bytes " + inMsg.DeliveryMethod + "|" + inMsg.SequenceChannel);
                            break;
                    }
                    server.Recycle(inMsg);
                }

                Thread.Sleep(1);
            }
        }
    }
}
