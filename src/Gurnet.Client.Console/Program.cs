using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting client...");

            var config = new NetPeerConfiguration("gurnet");

            NetClient client = new NetClient(config);            

            string input;
            while (!(input = Console.ReadLine()).Equals("quit", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("[d]{0}", input);

                if (input.Equals("connect", StringComparison.InvariantCultureIgnoreCase)
                    && client.ConnectionStatus == NetConnectionStatus.Disconnected)
                {
                    client.Start();
                    NetOutgoingMessage hailMsg = client.CreateMessage("Hail server");
                    client.Connect("localhost", 14242, hailMsg);

                    Console.WriteLine("[d]connecting...");
                }
                else
                {
                    NetOutgoingMessage outMsg = client.CreateMessage(input);
                    client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
                    client.FlushSendQueue();
                    Console.WriteLine("[d]sending msg [{0}]", input);
                }
            }

            if (client.ConnectionStatus == NetConnectionStatus.Connected)
            {
                client.Disconnect("Disconnect requested by user");
            }

        }
    }
}
