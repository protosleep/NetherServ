using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NetherServ.Network
{
    public class ConnectionHandler
    {
        private static ConnectionHandler mInstance = null;
        private string mServerName;
        private uint mMaxConnections;

        private Dictionary<IPEndPoint, Client> mClientList = new Dictionary<IPEndPoint, Client>();


        private ConnectionHandler() 
        {
            mMaxConnections = 5;
            mServerName = "work";
        }

        public static ConnectionHandler Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ConnectionHandler();
                }
                return mInstance;
            }
        }

        public string ServerName
        {
            get
            {
                return mServerName;
            }
        }

        public uint NumberOfConnections
        {
            get
            {
                uint connectedClients = 0;
                lock (mClientList)
                {
                    foreach (KeyValuePair<IPEndPoint, Client> item in mClientList)
                    {
                        if (item.Value.State != Client.ConnectionState.Disconnected)
                        {
                            connectedClients++;
                        }
                    }
                }
                return connectedClients;
            }
        }

        public uint MaxConnections
        {
            get
            {
                return mMaxConnections;
            }
        }

        public Client GetClient(IPEndPoint ep)
        {
            lock (mClientList)
            {
                if (!mClientList.ContainsKey(ep))
                {
                    if (NumberOfConnections < MaxConnections)
                    {
                        Client client = new Client(ep);
                        mClientList.Add(ep, client);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return mClientList[ep];
        }

    }
}
