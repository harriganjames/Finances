using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Finances.Core.Wpf.ObjectInformation
{
    public class ObjectInfoViewModel : ViewModelBase, IDisposable
    {
        ObservableCollection<ObjectReference> objectReferences = new ObservableCollection<ObjectReference>();

        
        readonly ObjectInformation objectInformation;
        readonly Dispatcher dispatcher;

        public ObjectInfoViewModel(ObjectInformation objectInformation,
                                Dispatcher dispatcher)
        {
            this.objectInformation = objectInformation;
            this.dispatcher = dispatcher;

            //this.objectInformation.ObjectReferences.CollectionChanged += ObjectReferences_CollectionChanged;

            this.objectInformation.ReferencesChanged += objectInformation_ReferencesChanged;

            GcCollectCommand = base.AddNewCommand(new ActionCommand(GcCollect));
        }

        void objectInformation_ReferencesChanged(object sender, EventArgs e)
        {
            var add = new List<ObjectReference>();
            var remove = new List<ObjectReference>();

            lock (this.objectInformation.Locker)
            {
                var ors = new Dictionary<WeakReference,ObjectReference>();
                foreach (var or in this.ObjectReferences)
	            {
		            ors.Add(or.Reference,or);
	            }

                foreach (var r in this.objectInformation.References)
                {
                    if (!ors.ContainsKey(r))
                    {
                        // add
                        add.Add(new ObjectReference(r));
                    }
                }


                var refs = new Dictionary<WeakReference, WeakReference>();
                foreach (var r in this.objectInformation.References)
                {
                    refs.Add(r, r);
                }

                foreach (var or in this.ObjectReferences)
                {
                    if (!refs.ContainsKey(or.Reference))
                    {
                        // remove
                        remove.Add(or);
                    }
                }

            }

            if(add.Count()>0 || remove.Count()>0)
            {
                this.dispatcher.Invoke(() =>
                {
                    add.ForEach(or => this.objectReferences.Add(or));
                    remove.ForEach(or => this.objectReferences.Remove(or));
                });
            }

        }

        public ActionCommand GcCollectCommand { get; set; }



        //void ObjectReferences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //    {
        //        foreach (var item in e.NewItems)
        //        {
        //            this.objectReferences.Add(new ObjectReference(item as WeakReference));
        //        }
        //    }
        //    if (e.OldItems != null)
        //    {
        //        foreach (var item in e.OldItems)
        //        {
        //            ObjectReference or = this.objectReferences.Single(r => r.Reference == item);
        //            if (or != null && this.objectReferences.Contains(or))
        //            {
        //                this.dispatcher.Invoke(() =>
        //                    {
        //                        this.objectReferences.Remove(or);
        //                    }
        //                );
        //            }
        //        }
        //    }

        //}

        public ObservableCollection<ObjectReference> ObjectReferences
        {
            get
            {
                return objectReferences;
            }
        }


        public string DialogTitle
        {
            get
            {
                return "Object Information";
            }
        }


        void GcCollect()
        {
            GC.Collect();
        }

        void IDisposable.Dispose()
        {
            this.objectInformation.ReferencesChanged -= objectInformation_ReferencesChanged;
            this.objectReferences.Clear();
        }
    }
}
