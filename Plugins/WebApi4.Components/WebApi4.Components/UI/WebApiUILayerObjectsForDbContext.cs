using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace WebApi4.Components.UI
{
    /// <summary>
    /// This class renders the WebApi4 classes for the UI layer.
    /// </summary>
    public class WebApiUILayerObjectsForDbContext: RenderBase, IUIObjects
    {
        #region ctors
        private readonly RequestContext _context;

        public WebApiUILayerObjectsForDbContext(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #endregion
        
        #region IRenderObject Members

        public void Render()
        {
            // Get the UI namespace here and pass it to the base class.
            string namespaceName = _script.Settings.UI.UINamespace;
            IRenderObject ui = new WebApiBaseClass(_context, namespaceName);
            ui.Render();
        }

        #endregion
    }
}
