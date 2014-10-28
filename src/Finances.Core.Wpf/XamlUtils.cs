using System.Collections.Generic;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Linq;

namespace Finances.Core.Wpf
{
    public static class XamlUtils
    {
        public static void DetachTriggers(object current)
        {
            //AttachDetachTriggers(current, false);
        }
        public static void AttachTriggers(object current)
        {
            //AttachDetachTriggers(current, true);
        }


        private static void AttachDetachTriggers(object current, bool attach)
        {
            DependencyObject parent = current as DependencyObject;

            if (parent != null)
            {
                int triggerQty = Interaction.GetTriggers(parent).Count;
                if (triggerQty > 0)
                {
                    int i = 0;
                }


                //var triggers = new List<System.Windows.Interactivity.TriggerBase>();



                foreach (var t in Interaction.GetTriggers(parent).ToList())
                {
                    var tb = t as System.Windows.Interactivity.TriggerBase;
                    var kt = t as Microsoft.Expression.Interactivity.Input.KeyTrigger;


                    if (attach)
                    {
                        //t.Attach(parent);
                        //Interaction.GetTriggers(parent).Remove(t);
                        //Interaction.GetTriggers(parent).Add(t);
                    }
                    else
                        t.Detach();
                }

                //foreach (var b in Interaction.GetBehaviors(parent))
                //{
                //    if (attach)
                //        b.Attach(parent);
                //    else
                //        b.Detach();
                //}

                //if (!attach && Interaction.GetTriggers(parent).Count>0)
                //{
                //    Interaction.GetTriggers(parent).Clear();
                //}


                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    AttachDetachTriggers(child,attach);
                }
            }
        }

    }
}
