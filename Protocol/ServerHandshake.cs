using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NetherServ.Protocol
{
    public class ServerHandshake : ProtocolMessage
    {
        public ServerHandshake()
            : base(MessageType.MC_SERVER_HANDSHAKE)
        {

        }

        public IPEndPoint Address
        {
            get;
            set;
        }

        public ulong SendPing
        {
            get;
            set;
        }

        public ulong SendPong
        {
            get;
            set;
        }

        public override byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            buffer.InsertValue(mMessageType);
            buffer.InsertValue(new byte[] { 0, 0 });
            buffer.InsertValue(new IPEndPoint(IPAddress.Loopback, 0));
            for (int i = 0; i < 9; i++)
            {
                buffer.InsertValue(new IPEndPoint(0, 0));
            }

            buffer.InsertValue(SendPing);
            buffer.InsertValue(SendPong);
            return buffer.GetBytes();
        }
    }
}
