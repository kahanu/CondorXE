using System;
using System.Linq;

namespace GizmoBeach.Components.UI
{
    public class UICriteria
    {
        public UICriteria(double mvcVersion = 4.0)
        {
            this._mvcVersion = mvcVersion;
        }

        private double _mvcVersion;

        /// <summary>
        /// The version of the ASP.NET MVC framework being used.  This will impact which
        /// code templates are used and placed in the web application.
        /// </summary>
        public double MvcVersion
        {
            get { return _mvcVersion; }
            set { _mvcVersion = value; }
        }

        /// <summary>
        /// This property tells the KingsMvc5ServiceLibraryCodeTemplatesForDbContext class 
        /// if the business objects tracking properties should be included.
        /// </summary>
        public bool UseTracking { get; set; }

    }
}
