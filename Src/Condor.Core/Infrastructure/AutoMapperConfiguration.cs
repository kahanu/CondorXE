using System;
using System.Linq;
using Condor.Core.Interfaces;

namespace Condor.Core.Infrastructure
{
    public class AutoMapperConfiguration : RenderBase, IMapperConfiguration
    {
        #region ctors
        private readonly RequestContext _context;

        /// <summary>
        /// Renders the AutoMapper.Mapper.CreateMap&lt;entity, model&gt; references.
        /// </summary>
        public AutoMapperConfiguration(RequestContext context)
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
            _output.tabLevel++;
            foreach (string tableName in _script.Tables)
            {
                _output.autoTabLn("AutoMapper.Mapper.CreateMap<" + StringFormatter.CleanUpClassName(tableName) + ", " + StringFormatter.CleanUpClassName(tableName) + modelName + ">();");
                _output.autoTabLn("AutoMapper.Mapper.CreateMap<" + StringFormatter.CleanUpClassName(tableName) + modelName + ", " + StringFormatter.CleanUpClassName(tableName) + ">();");
            }
            _output.tabLevel--;
        }
        #endregion
    }
}
