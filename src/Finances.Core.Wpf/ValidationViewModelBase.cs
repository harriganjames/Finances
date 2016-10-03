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
    public interface IValidationViewModelBase : IViewModelBase
    {
        ObservableCollectionSafe<string> Errors { get; }
        //event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        bool IsValid { get; set; }
        void NotifyPropertyChangedAndValidate(Expression<Func<object>> propertyExpression);
        void NotifyPropertyChangedAndValidate(string propertyName = "");
        void Validate();
        event EventHandler ValidationComplete;
    }

    public abstract class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo, IValidationViewModelBase
    {
        bool _isValid;
        ValidationHelper _validationHelper;
        private readonly List<IValidationViewModelBase> instances = new List<IValidationViewModelBase>();

        public ValidationViewModelBase()
        {
            _validationHelper = new ValidationHelper(this);

        }

        // add child instances
        public void AddValidationInstance(IValidationViewModelBase instance)
        {
            instances.Add(instance);
            instance.ValidationComplete += ValidationInstance_ValidationComplete;
        }

        private void ValidationInstance_ValidationComplete(object sender, EventArgs e)
        {
            // when an instance has completed validation, validate this only
            ValidateThis();
        }


        public event EventHandler ValidationComplete;

        protected void OnValidationComplete()
        {

            if (ValidationComplete != null)
            {
                ValidationComplete(this, new EventArgs());
            }
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
                }
            }
        }


        ObservableCollectionSafe<string> errors = new ObservableCollectionSafe<string>();
        void UpdateErrors()
        {
            //Debug.WriteLine("UpdateErrors start {0}", (object)this.GetType().Name);
            errors.Clear();
            foreach (var inst in instances)
            {
                errors.AddRange(inst.Errors);
            }
            errors.AddRange(_validationHelper.Errors);

            this.IsValid = errors.Count() == 0;

            base.RefreshCommands();
            //Debug.WriteLine("UpdateErrors end {0}, errors={1}", (object)this.GetType().Name, errors.Count());
        }

        // returns a list of all validation errors
        //public IEnumerable<string> Errors
        public ObservableCollectionSafe<string> Errors
        {
            get
            {
                Debug.WriteLine("Errors read - qty={0}", errors.Count());

                return errors;
            }
        }

        public void Validate()
        {
            if (!_validationHelper.Enabled) return;

            //Debug.WriteLine("Validate(base) - start");
            //iterate instances and call their Validate() method
            foreach (var inst in instances)
            {
                inst.Validate();
            }

            ValidateThis();

            //Debug.WriteLine("Validate(base) - end");
            OnValidationComplete();
            return;
        }

        void ValidateThis()
        {
            _validationHelper.Enabled = true;
            _validationHelper.Validate();   // attribute validation

            this.ValidateData();            // custom validation
            //base.RefreshCommands();
            UpdateErrors();
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
