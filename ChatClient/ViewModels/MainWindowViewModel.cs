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
        public ICommand OpenConnectDialogCommand { get; }

        private void OpenConnectDialogCommandExecute(object p) => new ConnectDialog().ShowDialog();

        private bool OpenConnectDialogCommandCanExecute(object p) => true;

        public MainWindowViewModel()
        {
            OpenConnectDialogCommand = new RelayCommand(OpenConnectDialogCommandExecute, OpenConnectDialogCommandCanExecute);
        }
    }
}
