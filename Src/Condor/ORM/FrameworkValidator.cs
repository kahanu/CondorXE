using System;
using System.Linq;

namespace Condor.ORM
{
    /// <summary>
    /// Apply rules to an ORM Framework prior to executing.
    /// </summary>
    public class FrameworkValidator : IValidator
    {
        private readonly string _ormType;
        private readonly string _dotNetType;

        public FrameworkValidator(string ormType, string dotNetType)
        {
            this._ormType = ormType;
            this._dotNetType = dotNetType;
        }

        public bool IsValid()
        {
            bool valid = true;

            //************************************************
            //*** This validates that the Entity Framework
            //*** only works with the 4.0 .Net Framework.
            //************************************************

            if (_ormType.Contains("EntityFramework"))
            {
                if (_dotNetType == "ThreePointFive")
                {
                    valid = false;
                    _message = "Entity Framework is not available for the .Net 3.5 Framework.";
                }
            }
            //************************************************

            // You can apply your own rules here.

            return valid;
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
