using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    public class EntityFramework6DbContextRepositoryOrmFramework : RenderBase, IORMFramework
    {        
        #region ctors

        private readonly IDataStore _dataStore;
        private readonly RequestContext _context;

        public EntityFramework6DbContextRepositoryOrmFramework(IDataStore dataStore, RequestContext context) : base(context.Zeus.Output)
        {
            this._dataStore = dataStore;
            this._context = context;
        }

        #endregion


        public void Generate()
        {
            EFCriteria criteria = new EFCriteria();
            criteria.EFVersion = 6.0;

            IORMFramework orm = new EntityFrameworkDbContextRepositoryOrmFrameworkBase(_dataStore, _context, criteria);
            orm.Generate();
        }

        public string Name
        {
            get { return "Entity Framework 6 DbContext"; }
        }
    }
}
