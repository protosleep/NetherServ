using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NetherServ.Protocol
{
    public class OutgoingMessageBuffer
    {
        private Queue<byte[]> mByteQueue = new Queue<byte[]>();

        public void InsertValue(MessageType val)
        {
            InsertValue((byte)val);
        }
        
        public void InsertValue(byte b)
        {
            mByteQueue.Enqueue(new byte[] { b });
        }

        public void InsertValue(short val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            mByteQueue.Enqueue(bytes);
        }


        public void InsertValue(ushort val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            mByteQueue.Enqueue(bytes);
        }


        public void InsertValue(int val, bool asTriad = false)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);

            if (asTriad)
            {
                byte[] triadBytes = new byte[3];
                Array.Copy(bytes, triadBytes, 3);
                mByteQueue.Enqueue(triadBytes);
            }
            else
            {
                mByteQueue.Enqueue(bytes);
            }
        }

        public void InsertValue(ulong val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            mByteQueue.Enqueue(bytes);
        }


        public void InsertValue(byte[] val)
        {
            mByteQueue.Enqueue(val);
        }


        public void InsertValue(string s)
        {
            short stringLen = (short)s.Length;
            InsertValue(stringLen);
            mByteQueue.Enqueue(System.Text.Encoding.ASCII.GetBytes(s));
        }

        public void InsertValue(IPEndPoint ep)
        {
            InsertValue((byte)4);
            InsertValue(ep.Address.GetAddressBytes());
            InsertValue(ep.Port);
        }

        
        public byte[] GetBytes()
        {
            return mByteQueue.SelectMany(r => r).ToArray();
        }
    }
}
