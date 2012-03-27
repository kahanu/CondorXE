using System;
using System.Linq;
using Condor.Core.Interfaces;

namespace Condor.Core.Infrastructure
{
    public class DataObjectsAutoMapperExtensions : RenderBase, IMapperExtensions
    {
        #region ctors
        private readonly RequestContext _context;

        /// <summary>
        /// Renders the 'ToModel' and 'FromModel' methods for AutoMapper mapped models.
        /// </summary>
        public DataObjectsAutoMapperExtensions(RequestContext context)
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
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public static " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + " ToBusinessObject(this " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected + "." + StringFormatter.CleanUpClassName(tableName) + " entity)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return AutoMapper.Mapper.Map<" + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected + "." + StringFormatter.CleanUpClassName(tableName) + ", " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + ">(entity);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public static List<" + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + "> ToBusinessObjects(this List<" + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected + "." + StringFormatter.CleanUpClassName(tableName) + "> entityList)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("var models = new List<" + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + ">();");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("if (entityList != null && entityList.Count > 0)");
                _output.tabLevel++;
                _output.autoTabLn("entityList.ForEach(b => models.Add(b.ToBusinessObject()));");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("return models;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public static " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected + "." + StringFormatter.CleanUpClassName(tableName) + " ToEntity(this " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + " model)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return AutoMapper.Mapper.Map<" + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName) + ", " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected + "." + StringFormatter.CleanUpClassName(tableName) + ">(model);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("#endregion");
                _output.tabLevel--;
            }
            _output.tabLevel++;
        }
        #endregion
    }
}
