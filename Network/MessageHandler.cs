using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace NetherServ.Network
{
    class MessageHandler
    {
        private struct QueuedMessage
        {
            public IPEndPoint source;
            public byte[] messageData;
        }

        private static MessageHandler mInstance;
        private ConcurrentQueue<QueuedMessage> mMessageQueue = new ConcurrentQueue<QueuedMessage>();
        private bool mTerminate;
        private DateTime mServerStartTime;
        private ulong mServerId;
       


        private MessageHandler() 
        {
            byte[] serverId = new byte[8];
            (new Random()).NextBytes(serverId);
            mServerId = BitConverter.ToUInt64(serverId, 0);
            mServerStartTime = DateTime.Now;
            Thread thread = new Thread(new ThreadStart(this.Run));
            thread.Start();
        }

        public void Stop()
        {
            mTerminate = true;
        }

        public static MessageHandler Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new MessageHandler();
                }
                return mInstance;
            }
        }


        public void NewMessage(byte[] message, IPEndPoint endpoint)
        {
            QueuedMessage queuedMessage = new QueuedMessage();
            queuedMessage.messageData = message;
            queuedMessage.source = endpoint;
            mMessageQueue.Enqueue(queuedMessage);
        }


        public static DateTime StartTime
        {
            get
            {
                return Instance.mServerStartTime;
            }
        }


        public ulong ServerId
        {
            get
            {
                return mServerId;
            }
        }


        public void Run()
        {
            while (!mTerminate)
            {
                QueuedMessage queuedMessage;
                if (mMessageQueue.TryDequeue(out queuedMessage))
                {
                    ProcessMessage(queuedMessage);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }


        private void ProcessMessage(QueuedMessage queuedMessage)
        {
            Protocol.IncomingMessageBuffer buffer = new Protocol.IncomingMessageBuffer(queuedMessage.messageData);
            Protocol.MessageType messageType = (Protocol.MessageType)buffer.NextByte;


            switch (messageType)
            {
                case Protocol.MessageType.ID_PING_OPEN_CONNECTIONS:
                    Protocol.PingOpenConnections message = new Protocol.PingOpenConnections();
                    if (!message.Parse(buffer))
                    {
                        return;
                    }

                    Protocol.UnconnectedPongOpenConnections response = new Protocol.UnconnectedPongOpenConnections();
                    response.PingId = message.PingId;
                    Network.UdpReceiver.Instance.Broadcast(response, queuedMessage.source);
                    break;
                default:
                    Client client = ConnectionHandler.Instance.GetClient(queuedMessage.source);
                    if (client != null)
                    {
                        client.ProcessMessage(messageType, buffer);
                    }
                    break;
            }            
        }

    }
}
