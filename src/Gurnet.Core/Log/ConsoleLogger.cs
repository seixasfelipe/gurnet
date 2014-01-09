using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Core.Log
{
    public class ConsoleLogger : ILogger
    {
        public string ContextName { get; private set; }

        public ConsoleLogger()
        {
            this.ContextName = "null";
        }

        public void Log(string message)
        {
            Log(message, null);
        }

        public void Log(string message, params object[] args)
        {
            if(args != null) 
            {
                message = string.Format(message, args);
            }
            Console.WriteLine("[{0}][LOG][{1}] {2}", 
                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), this.ContextName, message);
        }

        public void SetContext(string contextName)
        {
            this.ContextName = contextName;
        }
    }
}
