using System;
using System.Linq;
using Condor.Core.Interfaces;

namespace Condor.Core.Infrastructure
{
    public class DataObjectsAutoMapperConfiguration : RenderBase, IMapperConfiguration
    {
        #region ctors
        private readonly RequestContext _context;

        /// <summary>
        /// Renders the AutoMapper.Mapper.CreateMap&lt;entity, model&gt; references.
        /// </summary>
        public DataObjectsAutoMapperConfiguration(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #endregion

        #region Public Methods

        public void Render()
        {
            Render("Model");
        }

        public void Render(string modelName)
        {
            
            foreach (string tableName in _script.Tables)
            {
                _output.autoTabLn("AutoMapper.Mapper.CreateMap<" + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + StringFormatter.CleanUpClassName(tableName) + ", " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + ">();");
                _output.autoTabLn("AutoMapper.Mapper.CreateMap<" + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + ", " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + StringFormatter.CleanUpClassName(tableName) + ">();");
            }
            
        }
        #endregion
    }
}
