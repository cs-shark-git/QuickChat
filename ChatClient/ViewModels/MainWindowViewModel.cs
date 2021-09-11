using ChatClient.Framework.Commands;
using ChatClient.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatClient.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public ICommand OpenConnectDialogCommand { get; }

        private void OpenConnectDialogCommandExecute(object p) => new ConnectDialog().ShowDialog();

        private bool OpenConnectDialogCommandCanExecute(object p) => true;

        public MainWindowViewModel()
        {
            _title = "QuickChat: v 0.0.1";
            OpenConnectDialogCommand = new RelayCommand(OpenConnectDialogCommandExecute, OpenConnectDialogCommandCanExecute);
        }
    }
}
