using ChatClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.Services.Client
{
    internal class MessageReader
    {
        private NetworkStream _netStream;

        public MessageReader(NetworkStream netStream)
        {
            _netStream = netStream;
        }

        public Message ReadMessage()
        {
            Message msg = new Message();
            BinaryReader br = new BinaryReader(_netStream, Encoding.Default, true);
            using(br)
            {
                msg.Text = br.ReadString();
            }
            return msg;
        }      
    }
}
