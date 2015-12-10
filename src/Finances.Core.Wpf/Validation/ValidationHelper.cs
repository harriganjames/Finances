using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Wpf.Validation
{

    public interface IValidationHelperObject
    {
        void Clear();
        void AddError(string error); 
    }


    public class ValidationHelper
    {
        private readonly List<object> instances = new List<object>();
        private List<ValidationResult> validationResults = new List<ValidationResult>();
        bool _enabled = true;


        #region ctor
        public ValidationHelper()
        {
        }
        public ValidationHelper(object instance)
        {
            if (instance == null) { throw new ArgumentNullException("instance"); }
            this.instances.Add(instance);
        }
        public ValidationHelper(IEnumerable<object> inst)
        {
            if (instances == null) { throw new ArgumentNullException("instance"); }
            foreach (object o in inst)
                instances.Add(inst);
        }
        #endregion

        public void AddInstance(object instance)
        {
            this.instances.Add(instance);
        }


        public bool Enabled
        {
            get { return _enabled; }
            set 
            { 
                _enabled = value;
                if (!_enabled)
                    validationResults.Clear();
            }
        }

        public void Reset()
        {
            validationResults.Clear();
        }


        public void AddValidationMessage(string msg, IValidationHelperObject vho)
        {
            var v = instances.FirstOrDefault(o => o == vho) as IValidationHelperObject;
            if (v != null)
                v.AddError(msg);
            validationResults.Add(new ValidationResult(msg));
        }

        public void AddValidationMessage(string msg, string memberName = null)
        {
            ValidationResult vr;
            if (String.IsNullOrEmpty(memberName))
                vr = new ValidationResult(msg);
            else
                vr = new ValidationResult(msg, new List<string>() { memberName });
            validationResults.Add(vr);
        }

        public void Validate()
        {
            //Debug.WriteLine("Validate(helper) - start");

            if (!_enabled)
                return;

            //Debug.WriteLine("Validate(helper) - clear");
            // clear all ValidationResults
            validationResults.Clear();

            foreach (object instance in instances)
            {
                // validate the objects and their members
                Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), validationResults, true);

                var vho = instance as IValidationHelperObject;
                if (vho != null)
                    vho.Clear();

            }

            //Debug.WriteLine("Validate(helper) - start");
        }


        public string GetPropertyError(string memberName)
        {
            StringBuilder e = new StringBuilder(100);



            // get the errors for this 'memberName'
            foreach (System.ComponentModel.DataAnnotations.ValidationResult vr in validationResults)
            {
                if (vr.MemberNames.Any(mn => mn == memberName))
                {
                    if (e.Length>0) e.AppendLine();
                    e.Append(vr.ErrorMessage);
                }
            }

            return e.ToString();
        }

        public IEnumerable<ValidationResult> ValidationResults
        {
            get
            {
                return validationResults;
            }
        }

        public IEnumerable<string> Errors
        {
            get
            {
                return validationResults.Select(e => e.ErrorMessage); ;
            }
        }

        public bool HasErrors
        {
            get
            {
                return validationResults.Count() > 0;
            }
        }

    }


}
