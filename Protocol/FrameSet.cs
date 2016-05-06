using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NetherServ.Protocol
{
    public class FrameSet : ProtocolMessage
    {
        private const int MAX_SPLIT_SIZE = 128;
        private const int MAX_SPLIT_COUNT = 4;

        private IPEndPoint mSource;
        private List<int> mNACKQueue = new List<int>();
        private ConcurrentQueue<int> mACKQueue = new ConcurrentQueue<int>();
        private Dictionary<int, Dictionary<int, Protocol.Frame>> mSplitQueue = new Dictionary<int, Dictionary<int, Protocol.Frame>>();



        public FrameSet(MessageType type)
            : base(type)
        {
            Frames = new List<Frame>();
        }

        public int FrameSetIndex
        {
            get;
            set;
        }
        
        public List<Frame> Frames
        {
            get;
            set;
        }


        public override bool Parse(IncomingMessageBuffer buffer)
        {
            FrameSetIndex = buffer.NextIntTriad;

            while (buffer.BytesRemaining >= 4)
            {
                Frame frame = new Frame();
                frame.Decode(ref buffer);
                Frames.Add(frame);
            }

            return true;
        }


        public override byte[] ToByteArray()
        {
            OutgoingMessageBuffer buffer = new OutgoingMessageBuffer();
            buffer.InsertValue(mMessageType);
            buffer.InsertValue(FrameSetIndex, true);

            foreach (Frame f in Frames)
            {
                buffer.InsertValue(f.ToByteArray());
            }

            return buffer.GetBytes();
        }


        //public override void Respond(IPEndPoint source)
        //{
        //    int diff = SequenceNumber - LastSequenceNumber;

        //    lock (mNACKQueue)
        //    {
        //        if (mNACKQueue.Count > 0)
        //        {
        //            mNACKQueue.Remove(SequenceNumber);
        //            if (diff != 1)
        //            {
        //                for (int i = LastSequenceNumber + 1; i < SequenceNumber; i++)
        //                {
        //                    mNACKQueue.Add(i);
        //                }
        //            }
        //        }
        //    }

        //    mACKQueue.Enqueue(SequenceNumber);

        //    if (diff >= 1)
        //    {
        //        LastSequenceNumber = SequenceNumber;
        //    }

        //    foreach (Protocol.Frame ep in Packets)
        //    {
        //        ProcessEncapsulatedPacket(ep);
        //    }


        //    //OpenConnectionReply2 response = new OpenConnectionReply2();
        //    //response.MTUSize = MTUSize;
        //    //response.ClientAddress = ClientAddress;
        //    //Network.UdpReceiver.Instance.Broadcast(response, source);
        //}


        //private void ProcessEncapsulatedPacket(Protocol.Frame packet)
        //{
        //    if (packet.IsSplit)
        //    {
        //        ProcessSplitPacket(packet);
        //    }

        //    Protocol.MessageBuffer buffer = new Protocol.MessageBuffer(packet.Payload);
        //    Protocol.MessageType type = (Protocol.MessageType)buffer.NextByte;

        //    switch (type)
        //    {
        //        case Protocol.MessageType.MC_DISCONNECT_NOTIFICATION:
        //            Network.ConnectionHandler.Instance.Disconnect(mSource);
        //            break;
        //        case Protocol.MessageType.MC_CLIENT_CONNECT:
        //            break;
        //        case Protocol.MessageType.MC_CLIENT_HANDSHAKE:
        //            break;
        //        case Protocol.MessageType.MC_PING:
        //            break;
        //        case Protocol.MessageType.MC_PONG:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //private void ProcessSplitPacket(Protocol.Frame packet)
        //{
        //    if (packet.SplitIndex >= MAX_SPLIT_SIZE || packet.SplitCount >= MAX_SPLIT_SIZE || packet.SplitIndex < 0)
        //    {
        //        return;
        //    }

        //    lock (mSplitQueue)
        //    {
        //        if (!mSplitQueue.ContainsKey(packet.SplitID))
        //        {
        //            if (mSplitQueue.Count >= MAX_SPLIT_COUNT)
        //            {
        //                return;
        //            }

        //            Dictionary<int, Protocol.Frame> m = new Dictionary<int, Protocol.Frame>();
        //            m.Add(packet.SplitIndex, packet);
        //            mSplitQueue.Add(packet.SplitID, m);
        //        }
        //        else
        //        {
        //            Dictionary<int, Protocol.Frame> m = mSplitQueue[packet.SplitID];
        //            m.Add(packet.SplitIndex, packet);
        //            mSplitQueue[packet.SplitID] = m;
        //        }



        //        if (mSplitQueue[packet.SplitID].Count == packet.SplitCount)
        //        {
        //            Queue<byte[]> queue = new Queue<byte[]>();


        //            Protocol.Frame ep = new Protocol.Frame();

        //            Dictionary<int, Protocol.Frame> packets = mSplitQueue[packet.SplitID];
        //            foreach (KeyValuePair<int, Protocol.Frame> p in packets)
        //            {
        //                queue.Enqueue(p.Value.Payload);
        //            }

        //            byte[] payload = queue.SelectMany(r => r).ToArray();

        //            mSplitQueue.Remove(packet.SplitID);
        //            ep.Payload = payload;

        //            ProcessEncapsulatedPacket(ep);
        //        }
        //    }
        //}

    }
}
