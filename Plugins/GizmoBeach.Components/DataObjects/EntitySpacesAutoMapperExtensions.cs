using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.DataObjects
{
    public class EntitySpacesAutoMapperExtensions : RenderBase, IMapperExtensions
    {
        #region ctors

        private readonly RequestContext _context;
        protected CommonUtility _util;

        public EntitySpacesAutoMapperExtensions(RequestContext context):base(context.Zeus.Output)
        {
            this._context = context;
            this._util = context.Utility;
        }

        #endregion

        public void Render(string modelName)
        {
            foreach (string tableName in _script.Tables)
            {
                _output.tabLevel++;
                _output.autoTabLn("#region " + StringFormatter.CleanUpClassName(tableName) + " Mappers");
                _output.autoTabLn("");
                _output.autoTabLn("public static " + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " ToBusinessObject(this " + _util.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " entity)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return AutoMapper.Mapper.Map<" + _util.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + ", " + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + ">(entity);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("public static List<" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + "> ToBusinessObjects(this " + _util.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + "Collection entityList)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("var models = new List<" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + ">();");
                _output.autoTabLn("");
                _output.autoTabLn("if (entityList != null && entityList.Count > 0)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("foreach (var entity in entityList)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("models.Add(entity.ToBusinessObject());");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("return models;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("public static " + _util.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " ToEntity(this " + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model, " + _util.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " entity)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return AutoMapper.Mapper.Map<" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + ", " + _util.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + ">(model, entity);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("#endregion");
                _output.autoTabLn("");
                _output.tabLevel--;
            }
        }

        public void Render()
        {
            Render("Model");
        }
    }
}
