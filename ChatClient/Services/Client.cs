using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Threading;
using ChatClient.Models;
using ChatClient.Services.Client;
using ChatClient.Services.Client.Parsers;

namespace ChatClient.Service
{
    internal class Client : IDisposable
    {

        public string Name { get; set; }

        public event Action<ObservableCollection<Message>> MessageListChanged;
        public event Action<ObservableCollection<User>> UserListChanged;

        private ObservableCollection<Message> _messageCollection;
        private ObservableCollection<User> _userCollection;
        private string _host;
        private int _port;
        private TcpClient _tcpClient;
        private NetworkStream _netStream;
        private Dispatcher _dispatcher;

        public Client(string host, int port, Dispatcher dispatcher)
        {
            _userCollection = new ObservableCollection<User>();
            InitializeMessageCollection();
            _tcpClient = new TcpClient();

            _host = host;
            _port = port;

            _dispatcher = dispatcher;
        }

        private void InitializeMessageCollection()
        {
            _messageCollection = new ObservableCollection<Message>()
            {
                new Message()
                {
                    Text = $"Connection finished"
                },
                new Message()
                {
                    Text = $"Welcome, {Name}"
                }
            };
        }

        public void Start()
        {
            _tcpClient.Connect(_host, _port);
            _netStream = _tcpClient.GetStream();
            Procces();
        }

        private void Procces()
        {
            Message msg = new Message() { Text = Name };
            SendMessage(msg);
            SetUsers();
            Task task = new Task(ReceiveMessages);
            task.Start();
        }

        public void SendMessage(Message message)
        {
            MessageWriter mr = new MessageWriter(_netStream);
            mr.WriteMessage(message);
        }

        private void SetUsers()
        {
            UserCollectionParser parser = new UserCollectionParser();
            MessageReader mr = new MessageReader(_netStream);
            Message jsonMsg = mr.ReadMessage();
            ICollection<User> col = parser.Parse(jsonMsg);
            _userCollection = new ObservableCollection<User>(col);
            UserListChanged(_userCollection);
        }

        private void ReceiveMessages()
        {
            var mr = new MessageReader(_netStream);

            while(true)
            {
                try
                {
                    Message msg = mr.ReadMessage();

                    if(new MessageParser(new ConnectMessageParser()).Parse(msg))
                    {
                        User user = new User();
                        user.Name = msg.Text[2..];

                        Message message = new Message();
                        message.Text = $"{user.Name} connected to chat";
                        AddToCollectionWithDispatcher(user);
                        AddToCollectionWithDispatcher(message);
                    }
                    else if(new MessageParser(new DisconnectMessageParser()).Parse(msg))
                    {
                        User user = FindUserByName(msg.Text[2..]);

                        Message message = new Message();
                        message.Text = $"{user.Name} leave from chat";

                        AddToCollectionWithDispatcher(message);
                        RemoveFromCollectionWithDispatcher(user);
                    }
                    else
                    {
                        AddToCollectionWithDispatcher(msg);
                    }
                }
                catch(IOException)
                {
                    Stop();
                    return;
                }
            }
        }

        //private MessageAnalizer

        private void AddToCollectionWithDispatcher(User user)
        {
            _dispatcher.Invoke(new Action(() =>
            {
                _userCollection.Add(user);
            }));
            UserListChanged(_userCollection);
        }

        private void AddToCollectionWithDispatcher(Message message)
        {
            _dispatcher.Invoke(new Action(() =>
            {
                _messageCollection.Add(message);
            }));
            MessageListChanged(_messageCollection);
        }

        private User FindUserByName(string name)
        {
            foreach(var user in _userCollection)
            {
                if(user.Name == name)
                    return user;
            }
            throw new Exception("Не удалось найти пользователя по имени");
        }

        private void RemoveFromCollectionWithDispatcher(User user)
        {
            _dispatcher.Invoke(new Action(() =>
            {
                _userCollection.Remove(user);
            }));
            UserListChanged(_userCollection);
        }

        public void Stop()
        {
            if(_netStream != null)
                _netStream.Close();
            if(_tcpClient != null)
                _tcpClient.Close();
        }
              
        public void Dispose()
        {
            _netStream.Dispose();
            _tcpClient.Dispose();
        }
    }
}
