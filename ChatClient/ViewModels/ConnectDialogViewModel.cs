using ChatClient.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChatClient.Framework;
using ChatClient.Framework.Commands;
using System.Windows.Input;
using ChatClient.Models;

namespace ChatClient.ViewModels
{
    internal class ConnectDialogViewModel : ViewModel
    {

        private string _adress;
        private string _name;
        public string Adress
        {
            get
            {
                if (_adress is null)
                    return string.Empty;
                return _adress;
            }
            set
            {
                Set(ref _adress, value);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                Set(ref _name, value);
            }
        }

        public ICommand OnOpenChatCommand { get; }

        public void OnOpenChatCommandExecute(object parameter)
        {
            ConnectDataModelStatic.Name = _name;
            ConnectDataModelStatic.Adress = Adress;
            if (ConnectDataModelStatic.ValidationStatus)
                OpenChatCommand.Execute(parameter);

            ConnectDataModelStatic.ValidationStatus = true;
        }
        public bool OnOpenChatCommandCanExecute(object parameter) => OpenChatCommand.CanExecute(parameter);

        public OpenChatCommand OpenChatCommand { get; }

        public ConnectDialogViewModel()
        {

            OpenChatCommand = new OpenChatCommand();
            OnOpenChatCommand = new RelayCommand(OnOpenChatCommandExecute, OnOpenChatCommandCanExecute);
        }
    }
}
