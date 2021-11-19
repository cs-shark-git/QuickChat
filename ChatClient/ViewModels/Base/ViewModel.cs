using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient.ViewModels
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            if(PropertyChanged is null) return;

            Delegate[] invocationList = PropertyChanged.GetInvocationList();
            foreach(var action in invocationList)
            {
                action.DynamicInvoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if(Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

    }
}
