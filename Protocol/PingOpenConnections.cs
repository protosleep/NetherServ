using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NetherServ.Protocol
{
    class PingOpenConnections : ProtocolMessage
    {
        public PingOpenConnections()
            : base(MessageType.ID_PING_OPEN_CONNECTIONS)
        {
        }

        public ulong PingId
        {
            get;
            set;
        }


        public override bool Parse(IncomingMessageBuffer buffer) 
        {
            PingId = buffer.NextULong;
            byte[] magic = buffer.NextBytes(16);
            return IsMagic(magic);
        }

    }
}
