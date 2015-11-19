using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Finances.Interface;

namespace Finances.Core.Wpf
{
    //public interface IDialog
    //{
    //    ICommand DialogAcceptCommand { get; }
    //    void DialogOkClicked();
    //}

    //public interface IDialogService
    //{
    //    //bool ShowDialog(object vm);
    //    bool ShowDialog(IDialog vm);
    //    bool ShowDialogView(IDialog vm);
    //    void ShowDialogNonModal(IDialog vm);
    //    ICommand DialogAcceptCommand { get; }
    //    MessageBoxResultEnum ShowMessageBox(string title, string message, MessageBoxButtonEnum buttons);
    //    string[] ShowOpenFileDialog(string filter = "", bool multi = false);
    //}

    public class DialogService : IDialogService
    {
        private Lazy<ICommand> _dialogAcceptCommand = new Lazy<ICommand>(
            () => new RoutedCommand("Accept", typeof(DialogService))
            );
        readonly Window _wpfWindow;

        public DialogService(Window wpfWindow)
        {
            _wpfWindow = wpfWindow;
        }


        public ICommand DialogAcceptCommand
        {
            get 
            { 
                return _dialogAcceptCommand.Value; 
            }
        }


        //public bool ShowDialog(object vm)
        //{
        //    ContainerWindow w = new ContainerWindow();
        //    w.Owner = _wpfWindow;
        //    w.DataContext = vm;
        //    w.CommandBindings.Add(new CommandBinding(this.DialogAcceptCommand, (sender, e) => w.DialogResult = true));
        //    return w.ShowDialog().GetValueOrDefault(false);
        //}

        /// <summary>
        /// Shows a window containing the object passed
        /// </summary>
        /// <param name="vm">The object to show. e.g. the ViewModel</param>
        /// <returns></returns>
        public bool ShowDialog(IDialog vm)
        {
            ContainerWindow w = new ContainerWindow();
            w.Owner = _wpfWindow;
            w.DataContext = vm;
            w.Content = vm;
            w.CommandBindings.Add(new CommandBinding(vm.DialogAcceptCommand, (sender, e) => {
                vm.DialogOkClicked();    
                w.DialogResult = true;
                }));
            return w.ShowDialog().GetValueOrDefault(false);
        }

        /// <summary>
        /// Shows a dialog window (with Ok/Cancel buttons and validation display) containing the object passed
        /// </summary>
        /// <param name="vm">The object to show. e.g. the ViewModel</param>
        /// <returns></returns>
        public bool ShowDialogView(IDialog vm)
        {
            DialogContainer dc = new DialogContainer();
            ContainerWindow w = new ContainerWindow();
            w.Owner = _wpfWindow;
            w.DataContext = vm;
            w.Content = dc;
            w.CommandBindings.Add(new CommandBinding(vm.DialogAcceptCommand, (sender, e) =>
            {
                vm.DialogOkClicked();
                w.DialogResult = true;
            }));
            return w.ShowDialog().GetValueOrDefault(false);
        }


        /// <summary>
        /// Shows a window non-modally containing the object passed
        /// </summary>
        /// <param name="vm">The object to show. e.g. the ViewModel</param>
        /// <returns></returns>
        public void ShowDialogNonModal(IDialog vm)
        {
            ContainerWindow w = new ContainerWindow();
            w.Owner = _wpfWindow;
            w.DataContext = vm;
            w.Content = vm;
            w.CommandBindings.Add(new CommandBinding(vm.DialogAcceptCommand, (sender, e) =>
            {
                vm.DialogOkClicked();
                w.DialogResult = true;
            }));
            w.Show();
        }


        //this works fine but best to keep ContainerWindow
        //public bool ShowDialogView_Window_test(IDialog vm)
        //{
        //    DialogContainer dc = new DialogContainer();
        //    Window w = new Window();
        //    //var titleBinding = new Binding("DialogTitle");
        //    w.SetBinding(Window.TitleProperty,new Binding("DialogTitle"));
        //    w.SizeToContent = SizeToContent.WidthAndHeight;
        //    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    w.WindowStyle = WindowStyle.ToolWindow;
        //    w.ResizeMode = ResizeMode.CanResizeWithGrip;
        //    w.Owner = _wpfWindow;
        //    w.DataContext = vm;
        //    w.Content = dc;
        //    w.CommandBindings.Add(new CommandBinding(vm.DialogAcceptCommand, (sender, e) => w.DialogResult = true));
        //    return w.ShowDialog().GetValueOrDefault(false);
        //}


        public MessageBoxResultEnum ShowMessageBox(string title, string message, MessageBoxButtonEnum buttons)
        {
            var b = (MessageBoxButton)buttons;
            //var r = MessageBox.Show(message, title, b);
            MessageBoxResult r;
            r = _wpfWindow.Dispatcher.Invoke(() =>
                    MessageBox.Show(_wpfWindow, message, title, b, MessageBoxImage.Exclamation)
                    );
            return (MessageBoxResultEnum)r;
        }

        public string[] ShowOpenFileDialog(string filter="",bool multi=false)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = multi;
            dlg.Filter = filter;

            Nullable<bool> result = dlg.ShowDialog();

            return dlg.FileNames;
        }


    }
}
