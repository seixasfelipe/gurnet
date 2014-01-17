﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Gurnet.Server
{
    public interface IMessageProcessor
    {
        void ProcessIncomingMessage(NetIncomingMessage incMsg);
    }
}
