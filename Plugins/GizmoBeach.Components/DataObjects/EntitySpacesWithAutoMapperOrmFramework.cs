using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Infrastructure;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    /// <summary>
    /// This class may not completely work. It depends on whether or not you have a rowversion
    /// column in your tables.  They should be mapped from a byte[] to a string and back and
    /// I have not completely configured AutoMapper to handle that configuration properly.
    /// 
    /// You can feel free to go into the EntitySpacesAutoMapperExtensions.cs class and 
    /// make the proper adjustments.
    /// </summary>
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
