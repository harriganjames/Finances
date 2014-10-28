using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Wpf.ObjectInformation
{
    public class ObjectReference : ItemViewModelBase
    {
        WeakReference reference;
        DateTime created;


        public ObjectReference(WeakReference reference)
        {
            this.reference = reference;

            this.created = DateTime.Now;
        }


        public WeakReference Reference
        {
            get { return reference; }
        }


        public string TypeName
        {
            get
            {
                object o = reference.Target;
                if (o != null)
                    return o.GetType().Name;
                else
                    return "Collected";
            }
        }

        public string CreatedTime
        {
            get
            {
                return this.created.ToString("hh:mm:ss");
            }
        }

        public int Generation
        {
            get
            {
                return GC.GetGeneration(reference);
            }
        }


    }
}
