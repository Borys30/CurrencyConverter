using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CurrencyConverter.ViewModel.Base
{
    /// <summary>
    /// Base class for all view models that implements the INotifyPropertyChanged interface
    /// to support data binding in MVVM architecture.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event when a property value changes.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Sets a property value and raises the PropertyChanged event if the value has changed.
        /// Returns true if the value was changed, false otherwise.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return false;

            field = value;

            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
