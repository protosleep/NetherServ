using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    class IncompatibleVersion : ProtocolMessage
    {
        public IncompatibleVersion()
            : base(MessageType.ID_INCOMPATIBLE_VERSION)
        {

        }

        public override byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            buffer.InsertValue(mMessageType);
            buffer.InsertValue(RaknetProtocolVersion);
            buffer.InsertValue(mTheMagicNumber);
            buffer.InsertValue(Network.MessageHandler.Instance.ServerId);

            return buffer.GetBytes();
        }
    }
}
