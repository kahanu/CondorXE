using System;
using System.Linq;
using Condor.Core;

namespace WebApi4.Components.BusinessObjects
{
    /// <summary>
    /// This class modifies the properties in the Entity base class.
    /// </summary>
    public class WebApiTrackingBusinessObjects : WebApiBusinessObjectsBase
    {
        #region ctors

        private readonly RequestContext _context;

        public WebApiTrackingBusinessObjects(RequestContext context):base(context, new TrackingEntityClass(context))
        {
            this._context = context;
        }


        #endregion
    }
}
