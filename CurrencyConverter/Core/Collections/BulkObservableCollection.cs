using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CurrencyConverter.Core.Collections
{
    /// <summary>
    /// Extends ObservableCollection with functionality for adding multiple items at once while minimizing UI update notifications.
    /// </summary>
    public class BulkObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Adds multiple items to the collection with a single notification event instead of one per item.
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                return;

            CheckReentrancy();

            foreach (var item in items)
                Items.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
