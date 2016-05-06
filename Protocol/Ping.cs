using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class Ping : ProtocolMessage
    {
        public Ping()
            : base(MessageType.MC_PING)
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
