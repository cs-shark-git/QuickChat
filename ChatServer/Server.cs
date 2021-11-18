using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ChatServer
{
    class Server
    {
             
        private const string USER_CONNECT = "UC";
        private const string USER_DISCONNECT = "UD";

        private TcpListener _tcpListener;
        private List<User> _users;

        public Server(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _users = new List<User>();
        }

        public void Listen()
        {
            try
            {
                ListenConnections();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void ListenConnections()
        {
            _tcpListener.Start();
            Console.WriteLine($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] Start listening connections...");
            while(true)
            {
                User user = new User();
                user.TcpClient = _tcpListener.AcceptTcpClient();
                user.NetStream = user.TcpClient.GetStream();
                _users.Add(user);
                Thread thread = new Thread(UserHandler);
                thread.Start();

            }
        }

        private void UserHandler()
        {
            User user = _users.Last();
            try
            {
                string message;
                BinaryReader reader = new BinaryReader(user.NetStream, Encoding.Default, true); 

                user.Name = reader.ReadString();
                message = user.Name;
                Console.WriteLine(message);
                BroadcastMessage(USER_CONNECT + message, user);
                reader.Close();

                string jsonUsers = SerializeUsers();
                SendMessage(jsonUsers, user);

                while(true)
                {
                    try
                    {
                        message = GetUserMessage(user);
                        Console.WriteLine(message);
                        BroadcastMessage(message, user);
                    }
                    catch
                    {
                        message = $"{USER_DISCONNECT}{user.Name}";
                        Console.WriteLine($"{user.Name} leave from chat");
                        DisconnectUser(user);
                        RemoveUser(user);
                        BroadcastMessage(message, user);
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                DisconnectUser(user);
                RemoveUser(user);
            }
        }

        private void BroadcastMessage(string message, User excUser = null)
        {
            BinaryWriter writer;
            foreach(var user in _users)
            {
                if(user != excUser)
                {
                    writer = new BinaryWriter(user.NetStream, Encoding.Default, true);
                    writer.Write(message);
                }
            }
        }

        private string SerializeUsers()
        {
            StringBuilder json = new StringBuilder();
            foreach(var user in _users)
            {
                json.Append(JsonSerializer.Serialize(user) + '/');
            }
            return json.ToString();
        }

        private void SendMessage(string msg, User user)
        {
            BinaryWriter writer = new BinaryWriter(user.NetStream, Encoding.Default, true);
            writer.Write(msg);
        }

        private string GetUserMessage(User user)
        {
            BinaryReader reader = new BinaryReader(user.NetStream, Encoding.Default, true);
            var msg = reader.ReadString();
            reader.Close();
            return msg;
        }

        private void DisconnectUser(User user)
        {
            user.NetStream?.Close();
            user.TcpClient?.Close();
        }

        private void RemoveUser(User user)
        {
            if(user is null) return;
            _users.Remove(user);
        }

        public void Shutdown()
        {
            _tcpListener.Stop();
            CloseAllUsers();
            Environment.Exit(0);
        }

        private void CloseAllUsers()
        {
            foreach(var user in _users)
            {
                user.NetStream?.Close();
                user.TcpClient?.Close();
            }
        }                  
    }
}
