using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Set<T>(ref T originalValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(originalValue, newValue)) return false;
            originalValue = newValue;
            RaisedPropertyChanged(propertyName);

            return true;
        } 
       
        public void RaisedPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
        }
    }
}
