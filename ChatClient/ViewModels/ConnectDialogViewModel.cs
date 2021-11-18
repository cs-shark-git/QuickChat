using System.Windows;
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
        readonly private OpenChatCommand _openChatCommand;

        public void OnOpenChatCommandExecute(object parameter)
        {            
            if (ConnectDataModelStatic.SetValues(Adress, Name))
            {
                _openChatCommand.Execute(parameter);
                Application.Current.MainWindow.Hide();
                _closeWindowCommand.Execute(parameter);
            }
        }
        public bool OnOpenChatCommandCanExecute(object parameter) => _openChatCommand.CanExecute(parameter);

        private CloseWindowCommand _closeWindowCommand { get; }


        public ConnectDialogViewModel()
        {
            Adress = "localhost:49276";
            _openChatCommand = new OpenChatCommand();
            _closeWindowCommand = new CloseWindowCommand();
            OnOpenChatCommand = new RelayCommand(OnOpenChatCommandExecute, OnOpenChatCommandCanExecute);
        }
    }
}
