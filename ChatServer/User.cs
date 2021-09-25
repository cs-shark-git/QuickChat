using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class User
    {
        public string Name { get; set; }
        public TcpClient TcpClient { get; set; }
        public NetworkStream NetStream { get; set; }
    }
}
