using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using NetherServ.Protocol;

namespace NetherServ.Network
{
    public class Client
    {
        private int mFrameSetIndex = 0;
        private int mLastSequenceNumber = -1;
        private IPEndPoint mSource;

        public enum ConnectionState
        {
            Disconnected,
            Waiting,
            Connecting,
            Handshaking,
            Connected
        }


        public Client(IPEndPoint source)
        {
            mSource = source;
            State = ConnectionState.Waiting;
        }


        public ConnectionState State
        {
            get;
            set;
        }



        public void ProcessMessage(MessageType messageType, Protocol.IncomingMessageBuffer buffer)
        {
            switch (messageType)
            {
                case MessageType.ID_OPEN_CONNECTION_REQUEST_1:
                    if (State == ConnectionState.Waiting || State == ConnectionState.Connecting)
                    {
                        OpenConnectionRequest1 message = new OpenConnectionRequest1();
                        if (!message.Parse(buffer))
                        {
                            return;
                        }

                        if (message.IncompatibleProtocol)
                        {
                            IncompatibleVersion incompatibleVersion = new IncompatibleVersion();
                            Network.UdpReceiver.Instance.Broadcast(incompatibleVersion, mSource);
                        }

                        OpenConnectionReply1 response = new OpenConnectionReply1();
                        response.MTU = message.NullPayloadLength;
                        UdpReceiver.Instance.Broadcast(response, mSource);

                        State = ConnectionState.Connecting;
                    }
                    break;
                case Protocol.MessageType.ID_OPEN_CONNECTION_REQUEST_2:
                    if (State == ConnectionState.Connecting)
                    {
                        OpenConnectionRequest2 message = new OpenConnectionRequest2();
                        if (!message.Parse(buffer))
                        {
                            return;
                        }

                        OpenConnectionReply2 response = new OpenConnectionReply2();
                        response.MTUSize = message.MTUSize;
                        response.ClientAddress = message.ClientAddress;
                        Network.UdpReceiver.Instance.Broadcast(response, mSource);

                        State = ConnectionState.Handshaking;
                    }
                    break;
                case Protocol.MessageType.ID_CUSTOM_PACKET_0:
                case Protocol.MessageType.ID_CUSTOM_PACKET_1:
                case Protocol.MessageType.ID_CUSTOM_PACKET_2:
                case Protocol.MessageType.ID_CUSTOM_PACKET_3:
                case Protocol.MessageType.ID_CUSTOM_PACKET_4:
                case Protocol.MessageType.ID_CUSTOM_PACKET_5:
                case Protocol.MessageType.ID_CUSTOM_PACKET_6:
                case Protocol.MessageType.ID_CUSTOM_PACKET_7:
                case Protocol.MessageType.ID_CUSTOM_PACKET_8:
                case Protocol.MessageType.ID_CUSTOM_PACKET_9:
                case Protocol.MessageType.ID_CUSTOM_PACKET_A:
                case Protocol.MessageType.ID_CUSTOM_PACKET_B:
                case Protocol.MessageType.ID_CUSTOM_PACKET_C:
                case Protocol.MessageType.ID_CUSTOM_PACKET_D:
                case Protocol.MessageType.ID_CUSTOM_PACKET_E:
                case Protocol.MessageType.ID_CUSTOM_PACKET_F:
                    if (State == ConnectionState.Connected || State == ConnectionState.Handshaking)
                    {
                        FrameSet message = new FrameSet(messageType);
                        if (!message.Parse(buffer))
                        {
                            return;
                        }

                        foreach (Frame frame in message.Frames)
                        {
                            ProcessFrame(frame);
                        }
                    }
                    break;

                default:
                    //Unknown message
                    break;
            }
        }


        public void ProcessFrame(Frame frame)
        {
            IncomingMessageBuffer buffer = new IncomingMessageBuffer(frame.Payload);
            MessageType messageType = (MessageType)buffer.NextByte;

            switch (messageType)
            {
                case Protocol.MessageType.MC_DISCONNECT_NOTIFICATION:
                    Disconnect();
                    break;
                case Protocol.MessageType.MC_CLIENT_CONNECT:
                    ClientConnecting(buffer);
                    break;
                case Protocol.MessageType.MC_CLIENT_HANDSHAKE:
                    State = ConnectionState.Connected;
                    PingClient();
                    break;
                case Protocol.MessageType.MC_PING:
                    OnReceivePing(buffer);
                    break;
                case Protocol.MessageType.MC_PONG:
                    break;
                default:
                    ProcessGameMessage(buffer);
                    break;
            }


        }
        

        public void Disconnect()
        {
            Frame frame = new Frame();
            frame.Reliability = Protocol.Frame.ReliabilityStatus.UNRELIABLE;

            DisconnectionNotification discoNotify = new Protocol.DisconnectionNotification();
            frame.Payload = discoNotify.ToByteArray();

            SendFrame(frame);

            //add to BLACKLIST
            State = Client.ConnectionState.Disconnected;
        }


        public void ClientConnecting(IncomingMessageBuffer buffer)
        {
            ClientConnect message = new ClientConnect();
            message.Parse(buffer);

            ServerHandshake handshake = new ServerHandshake();
            handshake.Address = UdpReceiver.Instance.ServerEndpoint;
            handshake.SendPing = message.SendPing;
            handshake.SendPong = message.SendPing + 1000L;

            Frame frame = new Frame();
            frame.Payload = handshake.ToByteArray();
            frame.Reliability = Frame.ReliabilityStatus.UNRELIABLE;

            SendFrame(frame);
        }


        public void PingClient()
        {
            Ping ping = new Ping();
            ping.PingID = (ulong)DateTime.Now.Ticks * 10000;

            Frame frame = new Frame();
            frame.Payload = ping.ToByteArray();
            frame.Reliability = Frame.ReliabilityStatus.UNRELIABLE;
            SendFrame(frame);
        }


        public void OnReceivePing(IncomingMessageBuffer buffer)
        {
            Ping ping = new Ping();
            ping.Parse(buffer);

            Pong pong = new Pong();
            pong.PingID = ping.PingID;

            Frame frame = new Frame();
            frame.Reliability = Frame.ReliabilityStatus.UNRELIABLE;
            frame.Payload = pong.ToByteArray();
            SendFrame(frame);
        }


        public void ProcessGameMessage(IncomingMessageBuffer buffer)
        {
            int todo = 23;
            //TODO
        }
        

        public void SendFrame(Frame frame)
        {
            FrameSet message = new FrameSet(Protocol.MessageType.ID_CUSTOM_PACKET_0);
            message.Frames.Add(frame);
            message.FrameSetIndex = mFrameSetIndex;
            mFrameSetIndex++;

            UdpReceiver.Instance.Broadcast(message, mSource);

            //put message.FrameSetIndex in recovery queue??
        }
    }
}
