using ChatClient.Models;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.Services.Client
{
    internal class MessageWriter
    {
        private NetworkStream _netStream;

        public MessageWriter(NetworkStream netStream)
        {
            _netStream = netStream;
        }

        public void WriteMessage(Message message, Action action)
        {
            try
            {
                Write(message);
            }
            catch
            {
                action.Invoke();
            }
        }
        private void Write(Message message)
        {
            BinaryWriter bw = new BinaryWriter(_netStream, Encoding.Default, true);
            using(bw)
            {
                bw.Write(message.Text);
            }
        }
    }
}
