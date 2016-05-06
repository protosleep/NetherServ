using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetherServ.Protocol
{
    public class IncomingMessageBuffer
    {
        private byte[] mMessageBytes;
        private int mBytesRead;

        public IncomingMessageBuffer(byte[] data)
        {
            mMessageBytes = data;
            mBytesRead = 0;
        }


        public int BytesRemaining
        {
            get
            {
                return mMessageBytes.Length - mBytesRead;
            }
        }

        public byte NextByte
        {
            get
            {
                byte val = 0;
                if (mBytesRead < mMessageBytes.Length)
                {
                    val = mMessageBytes[mBytesRead];
                    mBytesRead++;
                }
                return val;
            }
        }

        public short NextShort
        {
            get
            {
                short val = 0;
                if (mBytesRead + sizeof(short) <= mMessageBytes.Length)
                {
                    byte[] valBuff = new byte[sizeof(short)];
                    Array.Copy(mMessageBytes, mBytesRead, valBuff, 0, sizeof(short));
                    Array.Reverse(valBuff);
                    val = BitConverter.ToInt16(valBuff, 0);
                    mBytesRead += sizeof(short);
                }
                return val;
            }
        }


        public ushort NextUShort
        {
            get
            {
                ushort val = 0;
                if (mBytesRead + sizeof(ushort) <= mMessageBytes.Length)
                {
                    byte[] valBuff = new byte[sizeof(ushort)];
                    Array.Copy(mMessageBytes, mBytesRead, valBuff, 0, sizeof(ushort));
                    Array.Reverse(valBuff);
                    val = BitConverter.ToUInt16(valBuff, 0);
                    mBytesRead += sizeof(ushort);
                }
                return val;
            }
        }

        public int NextInt
        {
            get
            {
                int val = 0;
                if (mBytesRead + sizeof(int) <= mMessageBytes.Length)
                {
                    byte[] valBuff = new byte[sizeof(int)];
                    Array.Copy(mMessageBytes, mBytesRead, valBuff, 0, sizeof(int));
                    Array.Reverse(valBuff);
                    val = BitConverter.ToInt32(valBuff, 0);
                    mBytesRead += sizeof(int);
                }
                return val;
            }
        }

        public int NextIntTriad
        {
            get
            {
                int val = 0;
                if (mBytesRead + 3 <= mMessageBytes.Length)
                {
                    byte[] valBuff = new byte[4];
                    Array.Copy(mMessageBytes, mBytesRead, valBuff, 1, 3);
                    Array.Reverse(valBuff);
                    valBuff[3] = 0;
                    val = BitConverter.ToInt32(valBuff, 0);
                    mBytesRead += 3;
                }
                return val;
            }
        }

        public uint NextUInt
        {
            get
            {
                uint val = 0;
                if (mBytesRead + sizeof(uint) <= mMessageBytes.Length)
                {
                    byte[] valBuff = new byte[sizeof(uint)];
                    Array.Copy(mMessageBytes, mBytesRead, valBuff, 0, sizeof(uint));
                    Array.Reverse(valBuff);
                    val = BitConverter.ToUInt32(valBuff, 0);
                    mBytesRead += sizeof(uint);
                }
                return val;
            }
        }


        public ulong NextULong
        {
            get
            {
                ulong val = 0;
                if (mBytesRead + sizeof(ulong) <= mMessageBytes.Length)
                {
                    byte[] valBuff = new byte[sizeof(ulong)];
                    Array.Copy(mMessageBytes, mBytesRead, valBuff, 0, sizeof(ulong));
                    Array.Reverse(valBuff);
                    val = BitConverter.ToUInt64(valBuff, 0);
                    mBytesRead += sizeof(ulong);
                }
                return val;
            }
        }


        public bool NextBool
        {
            get
            {
                return NextByte > 0;
            }
        }

        public byte[] NextBytes(int length)
        {
            byte[] bytes = new byte[length];
            if (mBytesRead + length <= mMessageBytes.Length)
            {
                Array.Copy(mMessageBytes, mBytesRead, bytes, 0, length);
                mBytesRead += length;
            }
            return bytes;
        }

    }
}
