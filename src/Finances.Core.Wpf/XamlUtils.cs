using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Finances.Core.Wpf
{
    public static class XamlUtils
    {
        public static void UnwireTriggers(object current)
        {
            DependencyObject parent = current as DependencyObject;

            if (parent != null)
            {
                int qty = Interaction.GetTriggers(parent).Count;

                if (Interaction.GetTriggers(parent).Count > 0)
                {

                    foreach (var t in Interaction.GetTriggers(parent))
                    {
                        t.Detach();
                    }
                    foreach (var b in Interaction.GetBehaviors(parent))
                    {
                        b.Detach();
                    }

                    //Interaction.GetTriggers(parent).Clear();
                    //Interaction.GetBehaviors(parent).Clear();

                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    UnwireTriggers(child);
                }
            }
        }

    }
}
