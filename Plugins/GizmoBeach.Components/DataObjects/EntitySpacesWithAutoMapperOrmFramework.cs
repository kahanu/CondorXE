using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Infrastructure;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    public class EntitySpacesWithAutoMapperOrmFramework : RenderBase, IORMFramework
    {
        #region ctors

        private readonly IDataStore _dataStore;
        private readonly RequestContext _context;

        public EntitySpacesWithAutoMapperOrmFramework(IDataStore dataStore, RequestContext context) : base(context.Zeus.Output)
        {
            this._dataStore = dataStore;
            this._context = context;
        }

        #endregion

        public void Generate()
        {
            IMapperConfiguration mapperConfig = new DataObjectsAutoMapperConfiguration(_context);
            IMapperExtensions mapperExtensions = new EntitySpacesAutoMapperExtensions(_context);
            IAutoMapperFramework mapper = new RenderDataObjectsAutoMapper(_context, mapperConfig, mapperExtensions);
            IORMFramework ormFramework = new EntitySpacesOrmFramework(_dataStore, _context, mapper);
            ormFramework.Generate();
        }

        public string Name
        {
            get { return "EntitySpaces"; }
        }
    }
}
