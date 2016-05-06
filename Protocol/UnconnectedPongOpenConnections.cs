using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    class UnconnectedPongOpenConnections : ProtocolMessage
    {
        public UnconnectedPongOpenConnections()
            : base(MessageType.ID_UNCONNECTED_PING_OPEN_CONNECTIONS)
        {
        }

        public ulong PingId
        {
            get;
            set;
        }

        public ulong ServerId
        {
            get
            {
                return Network.MessageHandler.Instance.ServerId;
            }
        }

        public string Identifier
        {
            get
            {
                string idString = "MCPE;";
                idString += Network.ConnectionHandler.Instance.ServerName + ";";
                idString += ProtocolMessage.ProtocolVersion + ";";
                idString += ProtocolMessage.MinecraftPEVersion + ";";
                idString += Network.ConnectionHandler.Instance.NumberOfConnections + ";";
                idString += Network.ConnectionHandler.Instance.MaxConnections + ";";
                idString += Network.MessageHandler.Instance.ServerId;
                return idString;
            }
        }

        public override byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            buffer.InsertValue(mMessageType);
            buffer.InsertValue(PingId);
            buffer.InsertValue(ServerId);
            buffer.InsertValue(mTheMagicNumber);
            buffer.InsertValue(Identifier);
            return buffer.GetBytes();
        }
    }
}
