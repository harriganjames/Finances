using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;

namespace Finances.Core.Wpf.Actions
{
    public class SetPropertyTriggerAction : TriggerAction<FrameworkElement>
    {

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(SetPropertyTriggerAction));



        public object PropertyValue
        {
            get { return (object)GetValue(PropertyValueProperty); }
            set { SetValue(PropertyValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyValueProperty =
            DependencyProperty.Register("PropertyValue", typeof(object), typeof(SetPropertyTriggerAction));




        public object TargetObject
        {
            get { return (object)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(object), typeof(SetPropertyTriggerAction));




        protected override void Invoke(object parameter)
        {
            object target = this.TargetObject ?? AssociatedObject;
            PropertyInfo propertyInfo = target.GetType().GetProperty(
                this.PropertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            propertyInfo.SetValue(target, Convert.ChangeType(this.PropertyValue,propertyInfo.PropertyType.UnderlyingSystemType));
        }
    }
}
