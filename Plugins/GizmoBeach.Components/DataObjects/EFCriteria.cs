using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GizmoBeach.Components.DataObjects
{
    /// <summary>
    /// This class is used to transport variable information to the base class
    /// in the case of versioning. Default version is 4.0.
    /// </summary>
    public class EFCriteria
    {
        public EFCriteria(double efVersion = 4.0)
        {
            this._efVersion = efVersion;
        }

        private double _efVersion;

        public double EFVersion
        {
            get { return _efVersion; }
            set { _efVersion = value; }
        }

    }
}
