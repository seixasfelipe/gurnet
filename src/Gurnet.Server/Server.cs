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
        private Core.Log.ILogger logger;
        private Thread serverInstance;

        public StatusEnum Status { get; private set; }

        public GurnetServer(string serverName, int port, Core.Log.ILogger logger)
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
            param.Port = param.port;

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
                Thread.Sleep(1);
            }
        }
    }
}
