using Gurnet.Core.Log;
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
            string name = "gurnet";
            int port = 14242;
            ILogger logger = new ConsoleLogger();
            GurnetServer server = new GurnetServer(name, port, logger);
            server.Start();

            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
                Thread.Sleep(500);
        }
    }
}
