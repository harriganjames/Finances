
using System;
using System.Configuration;
using Finances.Core;
using Finances.Core.Interfaces;
using Finances.Core.Wpf;
using Finances.Core.Wpf.ObjectInfo;
using Finances.Interface;
namespace Finances.WinClient.ViewModels
{
    public interface IMainViewModel
    {
        ActionCommand LoginCommand { get; set; }
        ActionCommand LogoutCommand { get; set; }
        ActionCommand OpenObjectInfoCommand { get; set; }

        bool LoggedIn { get; }
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        bool _loggedIn = false;
        
        readonly IWorkspaceAreaViewModel workspaceAreaViewModel;
        readonly IConnection connection;
        readonly IDialogService dialogService;
        readonly ObjectInfoViewModel objectInfoViewModel;
        readonly IAppSettings appSettings;

        public MainViewModel(IWorkspaceAreaViewModel workspaceAreaViewModel,
                        IConnection connection,
                        IDialogService dialogService,
                        IAppSettings appSettings,
                        ObjectInfoViewModel objectInfoViewModel)
        {
            this.workspaceAreaViewModel = workspaceAreaViewModel;
            this.connection = connection;
            this.dialogService = dialogService;
            this.appSettings = appSettings;
            this.objectInfoViewModel = objectInfoViewModel;

            LoginCommand = base.AddNewCommand(new ActionCommand(Login));
            LogoutCommand = base.AddNewCommand(new ActionCommand(Logout));
            OpenObjectInfoCommand = base.AddNewCommand(new ActionCommand(OpenObjectInfo));
        }

        #region Publics

        public ActionCommand LoginCommand { get; set; }
        public ActionCommand LogoutCommand { get; set; }
        public ActionCommand OpenObjectInfoCommand { get; set; }

        public IWorkspaceAreaViewModel LoggedInViewModel
        {
            get
            {
                return this.workspaceAreaViewModel;
            }
        }

        public bool LoggedIn
        {
            get
            {
                return _loggedIn;
            }
            private set
            {
                if (_loggedIn != value)
                {
                    _loggedIn = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string Connection
        {
            get
            {
                var b = new System.Data.SqlClient.SqlConnectionStringBuilder(connection.ConnectionString);
                return String.Format("{0}.{1}", b.DataSource, b.InitialCatalog);
            }
        }

        public bool IsDebugMode
        {
            get
            {
                bool debug = false;
                bool.TryParse(appSettings.GetSetting("debug"), out debug);
                return debug;
            }
        }



        #endregion

        private void Login(object pwd)
        {
            string spwd;

            spwd = pwd as string;


            //this.OpenWorkspace("Banks");


            LoggedIn = true;
            //_eventAggregator.GetEvent<LoginEvent>().Publish(null);
        }

        private void Logout()
        {
            LoggedIn = false;
            //_eventAggregator.GetEvent<LogoutEvent>().Publish(null);

            this.workspaceAreaViewModel.CloseAllWorkspaces();

            //this.Workspaces.Clear();

        }


        private void OpenObjectInfo()
        {
            this.dialogService.ShowDialogNonModal(this.objectInfoViewModel);
        }

    }
}
