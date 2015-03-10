using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Wpf;
using Finances.Core.Wpf.Validation;

namespace Finances.WinClient.ViewModels
{
    //
    // HasValue
    // IsValid
    //
    public class InputDecimal : NotifyBase, INotifyDataErrorInfo, IValidationHelperObject
    {
        //  User-defined conversion from string to InputDecimal
        //public static implicit operator InputDecimal(string s)
        //{
        //    var i = new InputDecimal();
        //    i.Input = s;
        //    return i;
        //}
        //public static explicit operator string(InputDecimal i)
        //{
        //    return i.Input;
        //}

        //public override string ToString()
        //{
        //    return input;
        //}

        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");

        public InputDecimal()
        {
            this.FormatString = "n2";
            this.Mandatory = true;
        }

        public string FormatString { get; set; }

        public bool Mandatory { get; set; }

        public Action<Decimal> ValueChangedAction { get; set; }


        string input;
        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                decimal test;
                if (String.IsNullOrWhiteSpace(value))
                {
                    HasValue = false;
                    IsNumeric = true;
                }
                else if (Decimal.TryParse(value,NumberStyles.Currency,this.culture, out test))
                {
                    this.Value = test;
                }
                else
                {
                    HasValue = false;
                    IsNumeric = false;
                }
                NotifyPropertyChanged(() => this.Input);
            }
        }

        Decimal value;
        public Decimal Value 
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                this.input = this.value.ToString(this.FormatString);
                HasValue = true;
                IsNumeric = true;
                if (ValueChangedAction != null)
                    ValueChangedAction(this.value);
                NotifyPropertyChanged(() => this.Input);
            }
        }
        public bool HasValue { get; private set; }
        
        bool isNumeric = true;
        public bool IsNumeric 
        { 
            get { return isNumeric; }
            private set { isNumeric = value; } 
        }

        public bool IsValid
        {
            get
            {
                return this.Mandatory ? this.IsNumeric && this.HasValue : this.IsNumeric;
            }
        }

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return new string[] { this.error };
        }

        public bool HasErrors
        {
            get { return !String.IsNullOrEmpty(error); }
        }

        #endregion



        string error;

        public void Clear()
        {
            error = null;
        }

        public void AddError(string error)
        {
            this.error = error;
        }
    }
}
