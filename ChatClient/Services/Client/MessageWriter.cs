using ChatClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Services.Client
{
    class MessageWriter
    {
        private NetworkStream _netStream;

        public MessageWriter(NetworkStream netStream)
        {
            _netStream = netStream;
        }

        public void WriteMessage(Message message)
        {
            BinaryWriter bw = new BinaryWriter(_netStream, Encoding.Default, true);
            using(bw)
            {
                bw.Write(message.Text);
            }
        }
    }
}
