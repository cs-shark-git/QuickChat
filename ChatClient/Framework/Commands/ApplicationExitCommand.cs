using System.Windows;

namespace ChatClient.Framework.Commands
{
    internal class ApplicationExitCommand : Command
    {       
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}