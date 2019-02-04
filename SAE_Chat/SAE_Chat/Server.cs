using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace SAE_Chat
{
    public class Server : ABaseNetwork
    {
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

            }
            catch (SocketException _ex)
            {
#if DEBUG
                throw _ex;
#else
                Console.WriteLine("Upps, something went wrong! Error 1");
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
    }
}
