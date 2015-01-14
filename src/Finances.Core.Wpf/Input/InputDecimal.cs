using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finances.Core.Wpf;

namespace Finances.WinClient.ViewModels
{
    //
    // HasValue
    // IsValid
    //
    public class InputDecimal : NotifyBase, INotifyDataErrorInfo
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

        public InputDecimal()
        {
            this.FormatString = "n2";
            this.Mandatory = true;
        }

        public string FormatString { get; set; }

        public bool Mandatory { get; set; }


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
                else if (Decimal.TryParse(value, out test))
                {
                    this.Value = test;
                    //this.input = this.value.ToString(this.FormatString);
                    //HasValue = true;
                    //IsValid = true;
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
            string result=null;
            if (!IsNumeric)
                result = "Value is not numeric";
            else if (!HasValue && this.Mandatory)
                result = "Valie is mandatory";
            return new string[] { result };
        }

        public bool HasErrors
        {
            get { return !IsValid; }
        }

        #endregion
    }
}
