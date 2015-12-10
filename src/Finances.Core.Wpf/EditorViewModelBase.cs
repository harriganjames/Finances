using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Finances.Core.Wpf.Validation;

namespace Finances.Core.Wpf
{
    public interface IEditorViewModelBase : IViewModelBase
    {
        IEnumerable<string> Errors { get; }
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        bool IsValid { get; set; }
        void NotifyPropertyChangedAndValidate(Expression<Func<object>> propertyExpression);
        void NotifyPropertyChangedAndValidate(string propertyName = "");
    }

    public abstract class EditorViewModelBase : ViewModelBase, INotifyDataErrorInfo, IEditorViewModelBase
    {
        bool _isValid;
        ValidationHelper _validationHelper;

        public EditorViewModelBase()
        {
            _validationHelper = new ValidationHelper(this);

        }


        public void NotifyPropertyChangedAndValidate([CallerMemberName] String propertyName = "")
        {
            this.Validate();
            base.NotifyPropertyChanged(propertyName);
        }
        public void NotifyPropertyChangedAndValidate(Expression<Func<object>> propertyExpression)
        {
            this.Validate();
            base.NotifyPropertyChanged(propertyExpression);
        }



        protected ValidationHelper ValidationHelper
        {
            get
            {
                return _validationHelper;
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    NotifyPropertyChanged();
                    //NotifyPropertyChanged(() => this.Errors);
                }
            }
        }



        // returns a list of all validation errors
        public IEnumerable<string> Errors
        {
            get
            {
                //Debug.WriteLine("Errors - qty={0}", _validationHelper.Errors.Count());

                return _validationHelper.Errors;
            }
        }

        protected void Validate()
        {
            //Debug.WriteLine("Validate(base) - start");
            _validationHelper.Enabled = true;
            _validationHelper.Validate();   // attribute validation
            this.ValidateData();            // custom validation
            base.RefreshCommands();
            this.IsValid = _validationHelper.Errors.Count() == 0;
            NotifyPropertyChanged(() => this.Errors);
            //Debug.WriteLine("Validate(base) - end");

            return;
        }


        protected virtual void ValidateData()
        {
        }


        protected void OnErrorsChanged(string propertyName)
        {
            //Debug.WriteLine(String.Format("OnErrorsChanged({0}) - {1}", propertyName, ErrorsChanged != null?"subscribed":"null"));

            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }


        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            List<object> lst = new List<object>();

            //if (propertyName == null)  Debug.WriteLine(String.Format("GetErrors(null)"));

            if (propertyName != null)
            {
                string err = _validationHelper.GetPropertyError(propertyName);
                if (err.Length > 0) lst.Add(err);

                //Debug.WriteLine(String.Format("GetErrors({0} - qty {1})", propertyName, lst.Count));
            }

            //Debug.WriteLine("GetErrors({0}) - qty={1}",propertyName,lst.Count);

            return lst;
        }

        bool INotifyDataErrorInfo.HasErrors
        {
            get
            {
                Debug.WriteLine(String.Format("HasErrors = {0}", _validationHelper.HasErrors));
                return _validationHelper.HasErrors;
            }
        }

        #endregion



    }
}
