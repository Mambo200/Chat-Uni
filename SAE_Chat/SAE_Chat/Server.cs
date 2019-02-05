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
        private List<Socket> m_allClientSockets = new List<Socket>();
        private CancellationTokenSource m_acceptTokenSource;

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
                while (true)
                {
                    tmp = m_OwnSocket.Accept();

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
    }
}
