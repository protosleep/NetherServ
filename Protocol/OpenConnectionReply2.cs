using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class OpenConnectionReply2 : ProtocolMessage
    {
        public OpenConnectionReply2()
            : base(MessageType.ID_OPEN_CONNECTION_REPLY_2)
        {
        }

        public short MTUSize
        {
            get;
            set;
        }

        public byte[] ClientAddress
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
            buffer.InsertValue(ClientAddress);
            buffer.InsertValue(MTUSize);
            buffer.InsertValue((byte)0);
            return buffer.GetBytes();
        }
    }
}
