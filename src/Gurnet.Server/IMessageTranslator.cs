using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gurnet.Server
{
    public interface IMessageTranslator
    {
        void TranslateMessage(string stringMessage);
    }
}
