using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class ClientConnect : ProtocolMessage
    {
        public ClientConnect()
            : base(MessageType.MC_CLIENT_CONNECT)
        {

        }

        public ulong ClientID
        {
            get;
            set;
        }

        public ulong SendPing
        {
            get;
            set;
        }

        public bool UseSecurity
        {
            get;
            set;
        }

        public override bool Parse(IncomingMessageBuffer buffer)
        {
            ClientID = buffer.NextULong;
            SendPing = buffer.NextULong;
            UseSecurity = buffer.NextBool;

            return true;
        }
    }
}
