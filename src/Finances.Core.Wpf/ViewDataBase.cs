using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Wpf
{
    public class ViewDataBase : NotifyBase
    {
        private object viewData;

        /// <summary>
        /// General purpose store for View data
        /// Use ViewData[name]
        /// e.g. ViewData[col_widths]
        /// </summary>
        public object ViewData
        {
            get
            {
                if (viewData == null)
                    viewData = new ViewDataIndexer();
                return viewData;
            }
        }



        private class ViewDataIndexer
        {
            Dictionary<string, object> bag = new Dictionary<string, object>();

            public object this[string name]
            {
                get
                {
                    object value = null;
                    bag.TryGetValue(name, out value);
                    return value;
                }
                set
                {
                    if (bag.ContainsKey(name))
                    {
                        bag[name] = value;
                    }
                    else
                    {
                        bag.Add(name, value);
                    }
                }
            }

        }



    }
}
