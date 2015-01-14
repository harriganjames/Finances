using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Finances.Core;
using Finances.WinClient.CastleInstallers;
using Finances.WinClient.ViewModels;
using Finances.WinClient.Views;

namespace Finances.WinClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            EventManager.RegisterClassHandler(typeof(UserControl), UserControl.LoadedEvent,
                new RoutedEventHandler(UserControlLoaded));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(SelectivelyIgnoreMouseButton));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotKeyboardFocusEvent,
                new RoutedEventHandler(SelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseDoubleClickEvent,
                new RoutedEventHandler(SelectAllText));



            Window w = new AppWindow();

            var container = new WindsorContainer();

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Install(
                            new ConnectionInstaller(),
                            new EFModelContextFactoryInstaller(),
                            new NHibernateSessionFactoryInstaller(),
                            new DialogServiceInstaller(w),
                            new RepositoriesInstaller(),
                            new DomainServicesInstaller(),
                            new ExceptionServiceInstaller(),
                            new InterceptorsInstaller(),
                            new WorkspaceInstaller(),
                            new ViewModelInstallers(),
                            new UtilitiesInstaller(w),
                            new MappingsCreatorInstaller()
                            );



            //this.Resources.Add(null, new DataTemplate(typeof(MainViewModel)) { VisualTree = new MainView() });

            container.Resolve<Apex>();

            container.Resolve<MappingsCreator>();

            w.DataContext = container.Resolve<IMainViewModel>();
            
            w.Show();
        }

        void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focused, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            Apex.ObjectInformation.AddReference(sender);
            var fe = sender as FrameworkElement;
            if (fe != null && fe.DataContext!=null)
            {
                Apex.ObjectInformation.AddReference(fe.DataContext);
            }
        }
    }
}
