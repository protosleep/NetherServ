using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class Pong : ProtocolMessage
    {
        public Pong()
            : base(MessageType.MC_PONG)
        {
        }

        public ulong PingID
        {
            get;
            set;
        }

        public override bool Parse(IncomingMessageBuffer buffer)
        {
            PingID = buffer.NextULong;
            return true;
        }

        public override byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            buffer.InsertValue(PingID);
            return buffer.GetBytes();
        }
    }
}
