using System;
using System.Linq;
using Condor.Core;

namespace GizmoBeach.Components.UI
{
    public class AspNetMvcUILayerObjectsForDbContext : AspNetMvcUILayerObjectsForDbContextBase
    {
        public AspNetMvcUILayerObjectsForDbContext(RequestContext context):base(context)
        {

        }
    }
}
