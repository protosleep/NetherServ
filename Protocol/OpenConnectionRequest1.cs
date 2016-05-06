using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace NetherServ.Protocol
{
    public class OpenConnectionRequest1 : ProtocolMessage
    {
        public OpenConnectionRequest1()
            : base(MessageType.ID_OPEN_CONNECTION_REQUEST_1)
        {
        }

        public short NullPayloadLength
        {
            get;
            set;
        }


        public bool IncompatibleProtocol
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

            byte protocolVersion = buffer.NextByte;
            if (protocolVersion == ProtocolMessage.RaknetProtocolVersion)
            {
                IncompatibleProtocol = false;
            }

            NullPayloadLength = (short)(buffer.BytesRemaining + 18);

            return true;
        }

    }
}
