using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class OpenConnectionReply1 : ProtocolMessage
    {
        public OpenConnectionReply1()
            : base(MessageType.ID_OPEN_CONNECTION_REPLY_1)
        {

        }

        public short MTU
        {
            get;
            set;
        }

        public override byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            buffer.InsertValue(mMessageType);
            buffer.InsertValue(mTheMagicNumber);
            buffer.InsertValue(Network.MessageHandler.Instance.ServerId);
            buffer.InsertValue((byte)0);
            buffer.InsertValue(MTU);
            return buffer.GetBytes();
        }

    }
}
