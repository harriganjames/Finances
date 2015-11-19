using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Finances.Core;
using Finances.Service;
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

            // Note: the culture used by String.Format() and CultureInfo.CurrentCulture and Binding StringFormat
            // are independant.
            // String.Format() seems to use the System locale (i.e. select language in system tray)
            // CultureInfo.CurrentCulture is locale of User - Region settings, Change location, Format tab
            // I think Binding StringFormat always uses en-US unless changed explicitly using the below:
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

            // allow collection injection
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            // disable automatic property injection
            container.Kernel.ComponentModelBuilder.RemoveContributor(
                container.Kernel.ComponentModelBuilder.Contributors.OfType<PropertiesDependenciesModelInspector>().Single());

            container.Kernel.AddFacility<TypedFactoryFacility>();

            container.Register(Component.For<Window>().Instance(w));
            container.Register(Component.For<Dispatcher>().UsingFactoryMethod(() => w.Dispatcher));



            container.Install(FromAssembly.InDirectory(
                new AssemblyFilter(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location))));



            //container.Install(
            //                new SystemInstaller(),
            //                new EnginesInstaller(),
            //                new ConnectionInstaller(),
            //                new EFModelContextFactoryInstaller(),
            //                new DialogServiceInstaller(w),
            //                //new RepositoriesInstaller(),
            //                new InterceptorsInstaller(),
            //                new MappingsCreatorInstaller(),
            //                new EntityInstaller(),
            //                new Finances.Service.ServiceInstaller(),
            //                new Finances.Persistence.EF.BootstrapInstaller(),
            //                new DomainServicesInstaller(),
            //                new ExceptionServiceInstaller(),
            //                new UtilitiesInstaller(w),

            //                new WorkspaceInstaller(),
            //                new ViewModelInstallers()
            //                );



            foreach (var handler in container.Kernel.GetAssignableHandlers(typeof(object)))
            {
                Console.Write("{0} :",handler.ComponentModel.Implementation);
                foreach (var service in handler.ComponentModel.Services)
                {
                    Console.Write(" {0}", service.Name);
                }
                Console.WriteLine();
            }


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
