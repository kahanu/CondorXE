using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.UI
{
    public class AspNetMvcWithAutoMapperUILayerObjects : RenderBase, IUIObjects
    {
        private readonly RequestContext _context;

        public AspNetMvcWithAutoMapperUILayerObjects(RequestContext context):base(context.Zeus.Output)
        {
            this._context = context;
        }

        #region IRenderObject Members

        public void Render()
        {
            IAutoMapperFramework autoMapper = new RenderMvcUIAutoMapper(_context);
            IUIObjects mvc = new AspNetMvcUILayerObjects(_context, autoMapper);
            mvc.Render();
        }

        #endregion
    }
}
