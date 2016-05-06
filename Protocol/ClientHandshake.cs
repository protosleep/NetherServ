using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class ClientHandshake : ProtocolMessage
    {
        public ClientHandshake()
            : base(MessageType.MC_CLIENT_HANDSHAKE)
        {
        }

    }
}
