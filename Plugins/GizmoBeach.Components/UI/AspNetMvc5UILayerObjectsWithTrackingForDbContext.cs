using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.UI
{
    public class AspNetMvc5UILayerObjectsWithTrackingForDbContext : RenderBase, IUIObjects
    {
        #region ctors

        private readonly RequestContext _context;

        public AspNetMvc5UILayerObjectsWithTrackingForDbContext(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #endregion

        public void Render()
        {
            UICriteria criteria = new UICriteria();
            criteria.MvcVersion = 5.0;
            criteria.UseTracking = true;

            IUIObjects ui = new AspNetMvcUILayerObjectsForDbContextBase(_context, null, criteria);
            ui.Render();
        }
    }
}
