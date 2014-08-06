using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    public class EntityFrameworkDbContextRepositoryOrmFramework : EntityFrameworkDbContextRepositoryOrmFrameworkBase
    {
        #region ctors

        public EntityFrameworkDbContextRepositoryOrmFramework(IDataStore dataStore, RequestContext context)
            :base(dataStore, context)
        {

        }

        #endregion
    }
}
