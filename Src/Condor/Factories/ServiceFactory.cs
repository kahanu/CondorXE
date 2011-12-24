using System;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Interfaces;

namespace Condor.Factories
{
    public class ServiceFactory : IObjectFactory
    {
        private readonly RequestContext _context;

        public ServiceFactory(RequestContext context)
        {
            this._context = context;
        }

        public void Render(string typeName)
        {
            object[] parms = new object[1]
            {
                _context
            };

            var component = (IServiceObjects)Activator.CreateInstance(Type.GetType(typeName), parms);
            component.Render();
        }
    }
}
