using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using NetherServ;

namespace NetherServ.Network
{
    class UdpReceiver
    {
        private static UdpReceiver mInstance;
        private bool mTerminate = true;
        private UdpClient mClient;
        private IPEndPoint mServerEndpoint;

        public static UdpReceiver Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new UdpReceiver();
                }
                return mInstance;
            }
        }

        public void Broadcast(Protocol.ProtocolMessage message, IPEndPoint source)
        {
            byte[] bytesToSend = message.ToByteArray();
            mClient.Send(bytesToSend, bytesToSend.Length, source);
        }

        public bool IsRunning
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public IPEndPoint ServerEndpoint
        {
            get
            {
                return mServerEndpoint;
            }
        }

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            mTerminate = false;
            Thread thread = new Thread(new ThreadStart(Instance.Run));
            thread.Start();
        }

        public void Stop()
        {
            mTerminate = true;
        }

        public void Run()
        {
            IsRunning = true;
            
            try
            {
                while (!mTerminate)
                {
                    mServerEndpoint = new IPEndPoint(IPAddress.Any, Port);
                    byte[] bytes = mClient.Receive(ref mServerEndpoint);
                    MessageHandler.Instance.NewMessage(bytes, mServerEndpoint);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                mClient.Close();
            }

            IsRunning = false;
        }


        private UdpReceiver()
        {
            Port = 19132;
            mClient = new UdpClient(Port);
            mServerEndpoint = new IPEndPoint(IPAddress.Any, Port);
        }
    }
}
