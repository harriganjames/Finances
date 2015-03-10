using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Finances.Core.Wpf
{
    public class NotifyBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        public virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            Debug.WriteLine("NotifyPropertyChanged({0})",(object)propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual void NotifyPropertyChanged(Expression<Func<object>> propertyExpression)
        {
            if (propertyExpression != null)
            {
                if (propertyExpression.Body is MemberExpression)
                {
                    this.NotifyPropertyChanged(((MemberExpression)propertyExpression.Body).Member.Name);
                    return;
                }
            }
            throw new Exception("Invalid property expression passed into NotifyPropertyChanged");
        }

        public virtual void NotifyAllPropertiesChanged()
        {
            Debug.WriteLine("NotifyAllPropertiesChanged");
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(""));
            }
        }


        public void Dispose()
        {
            PropertyChanged = null;
        }
    }
}
