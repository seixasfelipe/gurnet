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
            int port = 14242;

            if (args.Length > 0) 
            {
                port = int.Parse(args[0]);
            }

            Console.WriteLine("Usage: Gurnet.Server.Console.exe");
            Console.WriteLine("       Gurnet.Server.Console.exe port");
            Console.WriteLine("Example: Gurnet.Server.Console.exe  {0}\n", port);

            string name = "gurnet";
            ILogger logger = new ConsoleLogger();
            logger.SetContext("Server");
            GurnetServer server = new GurnetServer(name, port, logger);
            
            server.Start();

            Console.WriteLine("\n Hit 'ESC' to stop server.");
            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Thread.Sleep(500);
            }

            Console.WriteLine(Environment.NewLine);

            server.Stop();
        }
    }
}
