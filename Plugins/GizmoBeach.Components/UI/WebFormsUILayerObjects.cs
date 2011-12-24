using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.UI
{
    public class WebFormsUILayerObjects : RenderBase, IUIObjects
    {
        #region ctors

        public WebFormsUILayerObjects(RequestContext context):base(context.Zeus.Output)
        {

        }

        #endregion
        
        #region IRenderObject Members

        public void Render()
        {
            
        }

        #endregion
    }
}
