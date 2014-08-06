using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace WebApi4.Components.ServiceLayer
{
    /// <summary>
    /// This class generates the WebApi4 code from the common base class for the stand-alone services project.
    /// </summary>
    public class WebApiServiceLayerObjectsForDbContext : RenderBase, IServiceObjects
    {
        #region ctors

        private RequestContext _context;

        public WebApiServiceLayerObjectsForDbContext(RequestContext context) : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #endregion

        #region IServiceObjects Members

        public void Render()
        {
            // Get the Services namespace here and pass it to the base class.
            string namespaceName = _script.Settings.ServiceLayer.ServiceNamespace;
            IRenderObject service = new WebApiBaseClass(_context, namespaceName);
            service.Render();
        }

        #endregion

    }
}
