using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Finances.Core.Wpf.Controls
{
    public class DecimalTextBox : TextBox
    {
        public DecimalTextBox()
        {
            this.FormatString = "n2";
            this.IsMandatory = true;
            this.LostFocus += LostFocusHandler;
        }

        string formatString;
        public bool IsMandatory { get; set; }

        public decimal? Value
        {
            get { return (decimal?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(decimal?), typeof(DecimalTextBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChanged));



        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public string FormatString
        {
            get
            {
                return formatString;
            }

            set
            {
                formatString = value;
                UpdateText();
            }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(DecimalTextBox),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var tb = d as DecimalTextBox;
            tb.UpdateText();
        }


        void UpdateText()
        {
            if (this.Value.HasValue)
                this.Text = this.Value.Value.ToString(this.FormatString);
            else
                this.Text = String.Empty;

        }


        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");


        void LostFocusHandler(object sender, RoutedEventArgs args)
        {
            string value = this.Text;
            decimal test;

            if (String.IsNullOrWhiteSpace(value))
            {
                this.IsValid = !this.IsMandatory;
                if (IsValid) Value = null;
            }
            else if (Decimal.TryParse(value, NumberStyles.Currency, this.culture, out test))
            {
                this.Value = test;
                this.IsValid = true;
                UpdateText();
            }
            else
            {
                this.IsValid = false;
            }


            //if (!this.IsValid)
            //{
            //    var rule = new ExceptionValidationRule();

            //    var binding = GetBindingExpression(DecimalTextBox.ValueProperty);

            //    var ve = new ValidationError(rule, binding);

            //    ve.ErrorContent = "Value is invalid";

            //    System.Windows.Controls.Validation.MarkInvalid(binding, ve);
            //}
            //else
            //{
            //    System.Windows.Controls.Validation.ClearInvalid(GetBindingExpression(DecimalTextBox.ValueProperty));
            //}


        }


    }
}
