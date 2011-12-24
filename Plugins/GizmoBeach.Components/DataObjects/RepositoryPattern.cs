using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    public class RepositoryPattern : IDataPattern
    {
        #region ctors
        private readonly IORMFramework _ormFramework;

        public RepositoryPattern(IORMFramework ormFramework, RequestContext context)
        {
            this._ormFramework = ormFramework;
        } 
        #endregion

        #region IDataPattern Members

        public void Render()
        {
            _ormFramework.Generate();
        }

        #endregion
    }
}
