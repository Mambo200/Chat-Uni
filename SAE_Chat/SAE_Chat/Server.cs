using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SAE_Chat
{
    public class Server : ABaseNetwork
    {
        private static Random s_rnd = new Random();

        private int GetNewID
        {
            get
            {
                int id;
                do
                {
                    id = s_rnd.Next(1, int.MaxValue);
                } while (m_users.ContainsKey(id));

                return id;
            }
        }
        private Dictionary<User, Socket> m_userSockets
            = new Dictionary<User, Socket>();
        private Dictionary<int, User> m_users
            = new Dictionary<int, User>();

        private int m_nextID = 0;
        private List<Socket> m_allClientSockets = new List<Socket>();
        private CancellationTokenSource m_acceptTokenSource;
        private CancellationTokenSource m_reveiceTokenSource;

        public override bool Connect()
        {
            m_OwnSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any,
                1025);
            try
            {
                m_OwnSocket.Bind(endPoint);
                m_OwnSocket.Listen(23);

                m_acceptTokenSource = new CancellationTokenSource();
                CancellationToken token = m_acceptTokenSource.Token;

                Task acceptTask = new Task(AcceptTask, token);
                acceptTask.Start();

                return true;
            }
            catch (SocketException _ex)
            {
#if DEBUG
                throw _ex;
#else
                Console.WriteLine("Upps, something went wrong! Error 1");
                return false;
#endif

            }
        }

        public override bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public override bool Update()
        {
            throw new NotImplementedException();
        }

        private void AcceptTask()
        {
            try
            {
                Socket tmp;
                User user;
                while (true)
                {
                    tmp = m_OwnSocket.Accept();
                    user = new User(GetNewID);

                    Message setID = Message.Encode(ECommands.SET_ID, 0, user.ID);

                    tmp.Send(setID.Packet);

                    byte[] buffer = new byte[1024 * 10];
                    Message setNickname;
                    do
                    {
                        int receivedBytes = tmp.Receive(buffer);
                        setNickname = Message.Decode(buffer);

                    } while (setNickname.Command != ECommands.SET_NICKNAME);

                    Message ownJoinMessage;
                    if (setNickname.Parameter.Length > 0)
                    {
                        if (user.ID == setNickname.SenderID)
                        {
                            user.ChangeNickname(setNickname.Parameter[0]);
                            m_users.Add(user.ID, user);
                            m_userSockets.Add(user, tmp);
                            Message isValid = Message.Encode(ECommands.IS_VALID, 0);
                            tmp.Send(isValid.Packet);

                            Message joinedMessage = Message.Encode(ECommands.USER_JOINED, 0, user.ID, user.Nickname);
                            foreach (KeyValuePair<User, Socket> userSocket in m_userSockets)
                            {
                                userSocket.Value.Send(joinedMessage.Packet);
                                ownJoinMessage = Message.Encode(ECommands.USER_JOINED, 0, userSocket.Key.ID, userSocket.Key.Nickname);
                                tmp.Send(ownJoinMessage.Packet);
                            }

                            Task task = new Task(() => ReceiveThread(user.ID),
                                m_reveiceTokenSource.Token);
                            task.Start();
                        }
                    }
                }
            }
            catch (Exception _ex) when (_ex is SocketException
                                        || _ex is ObjectDisposedException)
            {
#if DEBUG
                Console.WriteLine(_ex);
#else
                Console.WriteLine("Upps, something went wrong! Error 2");
#endif

            }
        }

        private void ReceiveThread(int _userID)
        {
            int userID = (int)_userID;
            User user = m_users[userID];
            Socket userSocket = m_userSockets[user];

            try
            {
                int receivedBytes;
                while (true)
                {
                    byte[] buffer = new byte[Message.PACKET_SIZE];
                    receivedBytes = userSocket.Receive(buffer);

                    // TODO:
                    // Interpretieren
                }
            }
            catch (Exception _ex) when (_ex is SocketException)
            {
#if DEBUG
                throw;
#else

#endif
            }
        }
    }
}
