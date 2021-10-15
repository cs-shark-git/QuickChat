using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatServer
{
    class User
    {
        public string Name { get; set; }

        [JsonIgnore]
        public TcpClient TcpClient { get; set; }

        [JsonIgnore]
        public NetworkStream NetStream { get; set; }
    }
}
