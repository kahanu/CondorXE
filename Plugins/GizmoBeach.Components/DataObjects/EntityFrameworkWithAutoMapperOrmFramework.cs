using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Infrastructure;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    public class EntityFrameworkWithAutoMapperOrmFramework : RenderBase, IORMFramework
    {

        #region ctors

        private readonly RequestContext _context;

        private readonly IDataStore _dataStore;

        public EntityFrameworkWithAutoMapperOrmFramework(IDataStore dataStore, RequestContext context) : base(context.Zeus.Output)
        {
            this._dataStore = dataStore;
            this._context = context;
        }

        #endregion

        public void Generate()
        {
            IMapperConfiguration mapperConfig = new DataObjectsAutoMapperConfiguration(_context);
            IMapperExtensions mapperExtensions = new DataObjectsAutoMapperExtensions(_context);
            IAutoMapperFramework mapper = new RenderDataObjectsAutoMapper(_context, mapperConfig, mapperExtensions);
            IORMFramework ormFramework = new EntityFrameworkOrmFramework(_dataStore, _context, mapper);
            ormFramework.Generate();
        }

        public string Name
        {
            get { return "Entity Framework"; }
        }
    }
}
