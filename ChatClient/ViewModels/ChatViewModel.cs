using ChatClient.Models;
using ChatClient.Service;
using System.Windows.Input;
using ChatClient.Framework.Commands;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using ChatClient.Services;

namespace ChatClient.ViewModels
{
    internal class ChatViewModel : ViewModel
    {
        private string _title = "QuickChat: v 0.0.1";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string MessageText
        {
            get => _messageText;
            set => Set(ref _messageText, value);
        }
        private string _messageText;


        private Dispatcher _dispatcher;

        private string _name;
        private int _port;
        private string _adress;
        private Client _client;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => Set(ref _messages, value);
        }

        public ICommand DisconnectWindowCommand
        {
            get
            {
                return new RelayCommand((p) =>
                {
                    _client.Stop();
                    Application.Current.MainWindow.Show();
                }
                , (p) => true);
            }
        }
        public ICommand DisconnectCommand { get; }
        public ICommand SendMessageCommand { get; }

        private CloseWindowCommand _closeWindow;

        public bool DisconnectCommandCanExecute(object parameter) => _closeWindow.CanExecute(parameter);
        public void DisconnectCommandExecute(object parameter)
        {
            _client.Stop();
            Application.Current.MainWindow.Show();
            _closeWindow.Execute(parameter);
        }

        public void SendMessageCommandExecute(object parameter)
        {
            Message msg = new Message()
            {
                Text = $"{_name}: {MessageFormatter.SplitMessageOnLines(MessageText, 40, _name)}"
            };
            if(string.IsNullOrEmpty(MessageText)) return;

            _client.SendMessage(msg);
            Messages.Add(msg);
            MessageText = string.Empty;
        }

        public bool SendMessageCommandCanExecute(object parameter) => true;

        public ChatViewModel()
        {

            _closeWindow = new CloseWindowCommand();
            _dispatcher = Dispatcher.CurrentDispatcher;
            _port = ConnectionData.Port;
            _adress = ConnectionData.Adress;
            _name = ConnectionData.Name;

            Messages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    Text = $"Connection finished"
                },
                new Message()
                {
                    Text = $"Welcome, {_name}"
                }
            };
            SendMessageCommand = new RelayCommand(SendMessageCommandExecute, SendMessageCommandCanExecute);
            DisconnectCommand = new RelayCommand(DisconnectCommandExecute, DisconnectCommandCanExecute);

            _client = new Client(_adress, _port, _dispatcher);
            _client.Name = _name;
            _client.MessageListChanged += OnMessageListChanged;
            _client.UserListChanged += OnUserListChanged;
            _client.ClientStopped += OnClientStopped;
            _client.Start();
        }

        private void OnClientStopped()
        {
            MessageBox.Show("Problem with connection or host, application closed", "Error");
            Application.Current.Shutdown();
        }

        private void OnUserListChanged(ObservableCollection<User> userList)
        {
            Users = userList;
        }

        private void OnMessageListChanged(Message message)
        {
            _dispatcher.Invoke(() => Messages.Add(message));
        }
    }
}
