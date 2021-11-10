using ChatClient.Framework.Commands.Base;
using System.Windows;

namespace ChatClient.Framework.Commands
{
    class OpenChatCommand : Command
    {
        protected Chat _window;

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
             
            _window = new Chat()
            {
                Owner = Application.Current.MainWindow
            };

            _window.Closed += OnWindowClosed;
            _window.Show();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnWindowClosed;
            _window = null;
        }
    }
}
