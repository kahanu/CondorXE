using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Interfaces;

namespace Condor.Factories
{
    public class UIFactory : IObjectFactory
    {
        private readonly RequestContext _context;

        public UIFactory(RequestContext context)
        {
            this._context = context;
        }

        #region IObjectFactory Members

        public void Render(string typeName)
        {
            object[] parms = new object[1]
            {
                _context
            };

            var component = (IUIObjects)Activator.CreateInstance(Type.GetType(typeName), parms);
            component.Render();
        }

        #endregion
    }
}
