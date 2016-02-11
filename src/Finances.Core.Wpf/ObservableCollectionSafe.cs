using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Finances.Core.Wpf
{
    public class ObservableCollectionSafe<T> : ObservableCollection<T>
    {
        private SynchronizationContext _syncContext = ViewModelBase.SynchronizationContext; //SynchronizationContext.Current;

        public ObservableCollectionSafe()
        {

        }

        public ObservableCollectionSafe(IEnumerable<T> list) : base(list)
        {

        }

        private void ExecuteOnSyncContext(Action action)
        {
            if (SynchronizationContext.Current == _syncContext || !Thread.CurrentThread.IsBackground)
                action();
            else
                _syncContext.Send(_ => action(), null);
        }

        protected override void InsertItem(int index, T item)
        {
            ExecuteOnSyncContext(() => base.InsertItem(index, item));
        }

        protected override void RemoveItem(int index)
        {
            ExecuteOnSyncContext(() => base.RemoveItem(index));
        }

        protected override void SetItem(int index, T item)
        {
            ExecuteOnSyncContext(() => base.SetItem(index, item));
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            ExecuteOnSyncContext(() => base.MoveItem(oldIndex, newIndex));
        }

        protected override void ClearItems()
        {
            ExecuteOnSyncContext(() => base.ClearItems());
        }

        //protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    if (SynchronizationContext.Current == _syncContext)
        //        RaiseCollectionChanged(e);
        //    else
        //        _syncContext.Send(RaiseCollectionChanged, e);
        //}

        //private void RaiseCollectionChanged(object param)
        //{
        //    base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        //}

        //protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    if (SynchronizationContext.Current == _syncContext)
        //        RaisePropertyChanged(e);
        //    else
        //        _syncContext.Send(RaisePropertyChanged, e);
        //}

        //private void RaisePropertyChanged(object param)
        //{
        //    base.OnPropertyChanged((PropertyChangedEventArgs)param);
        //}

    }
}
