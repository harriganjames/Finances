
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace Finances.Core.Wpf.ObjectInformation
{
    public class ObjectInformation
    {
        ObservableCollection<WeakReference> refs = new ObservableCollection<WeakReference>();



        public ObservableCollection<WeakReference> ObjectReferences
        {
            get { return refs; }
            set { refs = value; }
        }

        public void AddObjectReference(object obj)
        {
            lock (refs)
            {
                refs.Add(new WeakReference(obj));
            }
        }




        public ObjectInformation()
        {
            var t = new System.Timers.Timer(1000);
            t.Elapsed += (s,e) => 
                {
                    PurgeRefs();
                };
            t.Start();
        }


        void PurgeRefs()
        {
            lock (refs)
            {
                foreach (var wr in refs.Where(wr => !wr.IsAlive).ToList())
                {
                    refs.Remove(wr);
                }
            }
        }



    }
}
