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
        private OpenChatCommand _openChatCommand { get; }

        public void OnOpenChatCommandExecute(object parameter)
        {
            ConnectDataModelStatic.Name = _name;
            ConnectDataModelStatic.Adress = Adress;
            if (ConnectDataModelStatic.ValidationStatus)
            {
                _openChatCommand.Execute(parameter);
                Application.Current.MainWindow.Hide();
                _closeWindowCommand.Execute(parameter);
            }

            ConnectDataModelStatic.ValidationStatus = true;
        }
        public bool OnOpenChatCommandCanExecute(object parameter) => _openChatCommand.CanExecute(parameter);

        private CloseWindowCommand _closeWindowCommand { get; }


        public ConnectDialogViewModel()
        {
            _openChatCommand = new OpenChatCommand();
            _closeWindowCommand = new CloseWindowCommand();
            OnOpenChatCommand = new RelayCommand(OnOpenChatCommandExecute, OnOpenChatCommandCanExecute);
        }
    }
}
