using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Collections.Generic
{
    public class ObservableVector<T> : Collection<T>, IObservableVector<T>, INotifyPropertyChanged
    {
        private const string IndexerName = "Item[]";

        // Cached EventArgs

        private static readonly ChangedArgs ResetArgs = new ChangedArgs(CollectionChange.Reset, 0);

        private static readonly PropertyChangedEventArgs IndexerArgs = new PropertyChangedEventArgs(IndexerName);
        private static readonly PropertyChangedEventArgs ItemsArgs = new PropertyChangedEventArgs(nameof(Items));
        private static readonly PropertyChangedEventArgs CountArgs = new PropertyChangedEventArgs(nameof(Count));

        // Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event VectorChangedEventHandler<T> VectorChanged;

        // Constructors

        public ObservableVector()
            : base()
        { }

        public ObservableVector(IList<T> list)
            : base(list)
        { }

        // Overridden Collection<T> methods

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCountChanged();
            OnItemsChanged();
            OnIndexerChanged();
            OnVectorChanged(ResetArgs);
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnCountChanged();
            OnItemsChanged();
            OnIndexerChanged();
            OnVectorChanged(CollectionChange.ItemInserted, (uint)index);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            OnCountChanged();
            OnItemsChanged();
            OnIndexerChanged();
            OnVectorChanged(CollectionChange.ItemRemoved, (uint)index);
        }

        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);
            OnItemsChanged();
            OnIndexerChanged();
            OnVectorChanged(CollectionChange.ItemChanged, (uint)index);
        }

        // Protected event wrapper methods

        protected void OnVectorChanged(CollectionChange change, uint index)
        {
            OnVectorChanged(new ChangedArgs(change, index));
        }

        protected void OnVectorChanged(ChangedArgs e) => VectorChanged?.Invoke(this, e);

        protected void OnPropertyChanged(string name) =>
            OnPropertyChanged(new PropertyChangedEventArgs(name));

        protected void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        // Event wrapper-wrapper methods!

        private void OnItemsChanged() => OnPropertyChanged(ItemsArgs);
        private void OnCountChanged() => OnPropertyChanged(CountArgs);
        private void OnIndexerChanged() => OnPropertyChanged(IndexerArgs);

        // Default implementation of IVectorChangedEventArgs

        protected class ChangedArgs : IVectorChangedEventArgs
        {
            public uint Index { get; }
            public CollectionChange CollectionChange { get; }

            public ChangedArgs(CollectionChange change, uint index)
            {
                this.Index = index;
                this.CollectionChange = change;
            }
        }
    }
}