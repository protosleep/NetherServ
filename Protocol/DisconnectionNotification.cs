using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class DisconnectionNotification : ProtocolMessage
    {
        public DisconnectionNotification()
            : base(MessageType.MC_DISCONNECT_NOTIFICATION)
        {

        }

        public override byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            buffer.InsertValue(mMessageType);
            return buffer.GetBytes();
        }
    }
}
