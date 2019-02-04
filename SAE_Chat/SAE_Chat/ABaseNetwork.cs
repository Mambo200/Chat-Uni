using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SAE_Chat
{
    /// <summary>
    /// this is the base network class
    /// It does awesome things.
    /// </summary>
    abstract class ABaseNetwork
    {
        public Socket m_OwnSocket;
        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract bool Update();


    }
}
