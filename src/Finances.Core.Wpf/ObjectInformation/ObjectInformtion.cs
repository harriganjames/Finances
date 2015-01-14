
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
        public object Locker = new object();

        List<WeakReference> refs = new List<WeakReference>();

        public event EventHandler ReferencesChanged;

        public List<WeakReference> References
        {
            get { return refs; }
            set { refs = value; }
        }

        public void AddReference(object obj)
        {
            bool changes = false;
            lock (Locker)
            {
                bool exists = false;
                foreach (var wr in refs)
                {
                    if (wr.Target!=null && wr.Target.Equals(obj))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    refs.Add(new WeakReference(obj));
                    changes = true;
                }
            }
            if (changes)
                NotifyReferencesChanged();
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
            bool changes = false;
            lock (Locker)
            {
                foreach (var wr in refs.Where(wr => !wr.IsAlive).ToList())
                {
                    refs.Remove(wr);
                    changes = true;
                }
            }
            if (changes)
                NotifyReferencesChanged();
        }


        void NotifyReferencesChanged()
        {
            if (ReferencesChanged != null)
                ReferencesChanged(this, new EventArgs());
        }


    }
}
