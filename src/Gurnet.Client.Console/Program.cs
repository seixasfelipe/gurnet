using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gurnet.Client
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting client...");

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var config = new NetPeerConfiguration("gurnet");
            config.AutoFlushSendQueue = false;

            NetClient client = new NetClient(config);
            client.RegisterReceivedCallback(new SendOrPostCallback(ReceiveMessage));

            string input;
            while (!(input = Console.ReadLine()).Equals("quit", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("[debug]{0}", input);

                if (input.Equals("connect", StringComparison.InvariantCultureIgnoreCase)
                    && client.ConnectionStatus == NetConnectionStatus.Disconnected)
                {
                    client.Start();
                    NetOutgoingMessage hailMsg = client.CreateMessage("Hail server");
                    client.Connect("localhost", 14242, hailMsg);

                    Console.WriteLine("[debug]connecting...");
                }
                else
                {
                    NetOutgoingMessage outMsg = client.CreateMessage(input);
                    client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
                    client.FlushSendQueue();
                    Console.WriteLine("[debug]sending msg [{0}]", input);
                }
            }

            if (client.ConnectionStatus == NetConnectionStatus.Connected)
            {
                client.Disconnect("Disconnect requested by user");
                client.Shutdown("Disconnect requested by user");
            }

        }

        private static void ReceiveMessage(object state)
        {
            var client = (NetClient)state;
            NetIncomingMessage inMsg;
            string text;
            while ((inMsg = client.ReadMessage()) != null)
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

                        if (status == NetConnectionStatus.Connected)
                            Console.WriteLine("Connected.");

                        if (status == NetConnectionStatus.Disconnected)
                            Console.WriteLine("Disconnected.");

                        string reason = inMsg.ReadString();
                        Console.WriteLine("{0}: {1}", status.ToString(), reason);

                        break;
                    case NetIncomingMessageType.Data:
                        string chat = inMsg.ReadString();
                        Console.WriteLine(chat);
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + inMsg.MessageType + " " + inMsg.LengthBytes + " bytes");
                        break;
                }
                client.Recycle(inMsg);
            }
        }
    }
}
