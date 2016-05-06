using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NetherServ.Protocol
{
    public class OpenConnectionRequest2 : ProtocolMessage
    {
        public OpenConnectionRequest2()
            : base(MessageType.ID_OPEN_CONNECTION_REQUEST_2)
        {
        }

        public byte ClientNetVersion
        {
            get;
            set;
        }

        public byte[] ClientAddress
        {
            get;
            set;
        }

        public short ClientPort
        {
            get;
            set;
        }

        public short MTUSize
        {
            get;
            set;
        }

        public ulong ClientID
        {
            get;
            set;
        }

        public override bool Parse(IncomingMessageBuffer buffer)
        {
            byte[] magic = buffer.NextBytes(16);
            if (!IsMagic(magic))
            {
                return false;
            }

            ClientNetVersion = buffer.NextByte;
            if (ClientNetVersion != 4)
            {
                return false;
            }

            ClientAddress = buffer.NextBytes(4);
            ClientPort = buffer.NextShort;
            MTUSize = buffer.NextShort;
            ClientID = buffer.NextULong;

            return true;
        }

    }
}
