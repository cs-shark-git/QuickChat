using System.Windows;
using ChatClient.Framework.Commands.Base;

namespace ChatClient.Framework.Commands
{
    class HideWindowCommand : Command
    {
        public override bool CanExecute(object parameter) => parameter is Window;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            var window = (Window)parameter;
            window.Hide();
        }
    }
}
