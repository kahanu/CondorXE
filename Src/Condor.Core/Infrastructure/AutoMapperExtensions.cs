using System;
using System.Linq;
using Condor.Core.Interfaces;

namespace Condor.Core.Infrastructure
{
    public class AutoMapperExtensions : RenderBase, IMapperExtensions
    {
        #region ctors
        private readonly RequestContext _context;

        /// <summary>
        /// Renders the 'ToModel' and 'FromModel' methods for AutoMapper mapped models.
        /// </summary>
        public AutoMapperExtensions(RequestContext context)
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
            _output.tabLevel--;
            foreach (string tableName in _script.Tables)
            {
                _output.tabLevel++;
                _output.autoTabLn("#region " + StringFormatter.CleanUpClassName(tableName) + " Mappers");
                _output.autoTabLn("");
                _output.autoTabLn("public static " + StringFormatter.CleanUpClassName(tableName) + modelName + " ToModel(this " + StringFormatter.CleanUpClassName(tableName) + " entity)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return AutoMapper.Mapper.Map<" + StringFormatter.CleanUpClassName(tableName) + ", " + StringFormatter.CleanUpClassName(tableName) + modelName + ">(entity);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("public static List<" + StringFormatter.CleanUpClassName(tableName) + modelName + "> ToModel(this List<" + StringFormatter.CleanUpClassName(tableName) + "> " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName)) + "List)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("var models = new List<" + StringFormatter.CleanUpClassName(tableName) + modelName + ">();");
                _output.autoTabLn("");
                _output.autoTabLn("if (" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName)) + "List != null && " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName)) + "List.Count > 0)");
                _output.tabLevel++;
                _output.autoTabLn(StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName)) + "List.ForEach(b => models.Add(b.ToModel()));");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.autoTabLn("return models;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("public static " + StringFormatter.CleanUpClassName(tableName) + " FromModel(this " + StringFormatter.CleanUpClassName(tableName) + modelName + " model)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return AutoMapper.Mapper.Map<" + StringFormatter.CleanUpClassName(tableName) + modelName + ", " + StringFormatter.CleanUpClassName(tableName) + ">(model);");
                _output.tabLevel--;
                _output.autoTabLn("} ");
                _output.autoTabLn("#endregion");
                _output.autoTabLn("");
                _output.tabLevel--;
            }
            _output.tabLevel++;
        }
        #endregion
    }
}
