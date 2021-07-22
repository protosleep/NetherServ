using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public enum MessageType : byte { 
        ID_PING_OPEN_CONNECTIONS = 0x01,
        ID_OPEN_CONNECTION_REQUEST_1 = 0x05,
        ID_OPEN_CONNECTION_REPLY_1 = 0x06,
        ID_OPEN_CONNECTION_REQUEST_2 = 0x07,
        ID_OPEN_CONNECTION_REPLY_2 = 0x08,
        ID_INCOMPATIBLE_VERSION = 0x1A,
        ID_UNCONNECTED_PING_OPEN_CONNECTIONS  = 0x1C,

        ID_CUSTOM_PACKET_0 = 0x80,
        ID_CUSTOM_PACKET_1 = 0x81,
        ID_CUSTOM_PACKET_2 = 0x82,
        ID_CUSTOM_PACKET_3 = 0x83,
        ID_CUSTOM_PACKET_4 = 0x84,
        ID_CUSTOM_PACKET_5 = 0x85,
        ID_CUSTOM_PACKET_6 = 0x86,
        ID_CUSTOM_PACKET_7 = 0x87,
        ID_CUSTOM_PACKET_8 = 0x88,
        ID_CUSTOM_PACKET_9 = 0x89,
        ID_CUSTOM_PACKET_A = 0x8A,
        ID_CUSTOM_PACKET_B = 0x8B,
        ID_CUSTOM_PACKET_C = 0x8C,
        ID_CUSTOM_PACKET_D = 0x8D,
        ID_CUSTOM_PACKET_E = 0x8E,
        ID_CUSTOM_PACKET_F = 0x8F,

        ACK = 0xC0,
        NACK = 0xA0,

        MC_PING = 0x00,
        MC_PONG = 0x03,

        MC_CLIENT_CONNECT = 0x09,
        MC_SERVER_HANDSHAKE = 0x10,
        MC_CLIENT_HANDSHAKE = 0x13,
        MC_DISCONNECT_NOTIFICATION = 0x15
    }

    public class ProtocolMessage
    {
        public static string MinecraftPEVersion = "1.17.10";
        public static int ProtocolVersion = 448;
        public static byte RaknetProtocolVersion = 7;

        protected static byte[] mTheMagicNumber = new byte[] { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
        protected MessageType mMessageType;

        public ProtocolMessage(MessageType type)
        {
            mMessageType = type;
        }

        public MessageType MessageType
        {
            get
            {
                return mMessageType;
            }
        }

        public bool IsMagic(byte[] bytes)
        {
            if (bytes.Length < 16)
            {
                return false;
            }
            
            for (int i = 0; i < 16; i++)
            {
                if (bytes[i] != mTheMagicNumber[i])
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool Parse(IncomingMessageBuffer buffer) { return false; }
        public virtual byte[] ToByteArray() { return null; }
    }
}
