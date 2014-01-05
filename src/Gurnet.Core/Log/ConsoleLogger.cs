using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Core.Log
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Log(message, null);
        }

        public void Log(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
    }
}
