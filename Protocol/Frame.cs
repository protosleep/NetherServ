using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class Frame
    {
        public enum ReliabilityStatus : byte
        {
            UNRELIABLE = 0,
            UNRELIABLE_SEQUENCED = 1,
            RELIABLE = 2,
            RELIABLE_ORDERED = 3,
            RELIABLE_SEQUENCED = 4,
            UNRELIABLE_WITH_ACK_RECEIPT = 5,
            RELIABLE_WITH_ACK_RECEIPT = 6,
            RELIABLE_ORDERED_WITH_ACK_RECEIPT = 7
        }

        public Frame() { }

        public ReliabilityStatus Reliability
        {
            get;
            set;
        }

        public bool IsSplit
        {
            get;
            set;
        }

        public int MessageIndex
        {
            get;
            set;
        }

        public int OrderIndex
        {
            get;
            set;
        }

        public byte OrderChannel
        {
            get;
            set;
        }

        public int SplitCount
        {
            get;
            set;
        }

        public int SplitID
        {
            get;
            set;
        }

        public int SplitIndex
        {
            get;
            set;
        }

        public byte[] Payload
        {
            get;
            set;
        }
        

        public void Decode(ref IncomingMessageBuffer buffer)
        {
            MessageIndex = -1;

            byte flags = buffer.NextByte;

            Reliability = (ReliabilityStatus)((flags & 0xE0) >> 5);
            IsSplit = (flags & 0x10) > 0;

            int length = (int)Math.Ceiling((double)buffer.NextUShort / 8.0);

            if (Reliability == ReliabilityStatus.RELIABLE ||
                Reliability == ReliabilityStatus.RELIABLE_SEQUENCED ||
                Reliability == ReliabilityStatus.RELIABLE_ORDERED)
            {
                MessageIndex = buffer.NextIntTriad;
            }

            if (Reliability == ReliabilityStatus.UNRELIABLE_SEQUENCED ||
                Reliability == ReliabilityStatus.RELIABLE_SEQUENCED ||
                Reliability == ReliabilityStatus.RELIABLE_ORDERED)
            {
                OrderIndex = buffer.NextIntTriad;
                OrderChannel = buffer.NextByte;
            }

            if (IsSplit)
            {
                SplitCount = buffer.NextInt;
                SplitID = buffer.NextUShort;
                SplitIndex = buffer.NextInt;
            }

            Payload = buffer.NextBytes(length);

        }


        public byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            byte flags = (byte)(((byte)Reliability << 5) | (IsSplit ? 0x10 : 0));
            buffer.InsertValue(flags);

            short payloadBits = (short)(Payload.Length * 8);
            buffer.InsertValue(payloadBits);

            if (Reliability == ReliabilityStatus.RELIABLE ||
                Reliability == ReliabilityStatus.RELIABLE_SEQUENCED ||
                Reliability == ReliabilityStatus.RELIABLE_ORDERED)
            {
                buffer.InsertValue(MessageIndex, true);
            }

            if (Reliability == ReliabilityStatus.UNRELIABLE_SEQUENCED ||
                Reliability == ReliabilityStatus.RELIABLE_SEQUENCED ||
                Reliability == ReliabilityStatus.RELIABLE_ORDERED)
            {
                buffer.InsertValue(OrderIndex, true);
                buffer.InsertValue(OrderChannel);
            }

            if (IsSplit)
            {
                buffer.InsertValue(SplitCount);
                buffer.InsertValue(SplitID);
                buffer.InsertValue(SplitIndex);
            }

            buffer.InsertValue(Payload);
            return buffer.GetBytes();
        }
    }
}
