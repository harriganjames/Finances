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
using Finances.Core.Wpf.Validation;

namespace Finances.Core.Wpf
{
    public interface IItemViewModelBase
    {
        bool IsSelected { get; set; }
        event EventHandler<BooleanResultEventArgs> SelectedChanged;
    }

    public abstract class ItemViewModelBase : ViewModelBase, IItemViewModelBase
    {
        bool _isSelected;

        public event EventHandler<BooleanResultEventArgs> SelectedChanged;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnSelectedChanged(value);
                    NotifyPropertyChanged();
                }
            }
        }

        protected void OnSelectedChanged(bool selectedChanged)
        {
            if (this.SelectedChanged != null)
                SelectedChanged(this, new BooleanResultEventArgs(selectedChanged));
        }


    }
}
