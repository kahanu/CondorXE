using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Interfaces;

namespace Condor.Factories
{
    public class IoCFactory : IObjectFactory
    {
        #region ctors

        private readonly RequestContext _context;

        public IoCFactory(RequestContext context)
        {
            this._context = context;
        }

        #endregion

        #region IObjectFactory Members

        public void Render(string typeName)
        {
            object[] parms = new object[1]
            {
                _context
            };

            var component = (IIoCProvider)Activator.CreateInstance(Type.GetType(typeName), parms);
            component.Render();
        }

        #endregion
    }
}
