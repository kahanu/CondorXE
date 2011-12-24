using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GizmoBeach.Condor.Core.Interfaces;
using MyMeta;
using Zeus;

namespace GizmoBeach.Condor.Core
{
    public abstract class ORMFrameworkBase : RenderBase, IORMFramework
    {
        protected IDataStore _dataStore;
        protected RequestContext _context;
        protected ScriptSettings _script;
        protected IDatabase _database;
        protected IZeusOutput output;
        protected ProgressDialogWrapper _dialog;

        public ORMFrameworkBase(IDataStore dataStore, RequestContext context):base(context.Zeus.Output)
        {
            this._dataStore = dataStore;
            this._context = context;
            this._script = context.ScriptSettings;
            this._database = context.Database;
            this._dialog = context.Dialog;
        }
        #region IORMFramework Members

        public abstract string Name { get; }

        public void Generate()
        {
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderBaseClass(table);
            }

            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderConcreteClass(table);
            }

            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderMapperClass(table);
            }

            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderInterface(table);
            }

            RenderCRUDInterface();
        }

        private void RenderCRUDInterface()
        {
            
        }

        private void RenderInterface(ITable table)
        {
            
        }

        private void RenderMapperClass(ITable table)
        {
            string str = "";
            // To BusinessObject loop
            foreach (Column c in table.Columns)
            {
                if (c.Name.ToLower() != _script.Settings.DataOptions.VersionColumnName.ToLower())
                {
                    if (c.IsNullable && (c.LanguageType != "string"))
                    {
                        str += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = " + "entity." + _context.Utility.CleanUpProperty(c.Name, false) + ".HasValue ? (" + c.LanguageType + ")entity." + _context.Utility.CleanUpProperty(c.Name, false) + " : default(" + c.LanguageType + ")," + Environment.NewLine;
                    }
                    else
                    {
                        str += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = " + "entity." + _context.Utility.CleanUpProperty(c.Name, false) + "," + Environment.NewLine;
                    }
                }
                else
                {
                    str += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = " + "VersionConverter.ToString(entity." + _context.Utility.CleanUpProperty(c.Name, false) + ")," + Environment.NewLine;
                }
            }

            int lastComma = str.LastIndexOf(",");
            str = str.Substring(0, lastComma);

            string strEntity = string.Empty;

            // ToEntity loop
            foreach (Column c in table.Columns)
            {
                if (c.Name.ToLower() != _script.Settings.DataOptions.VersionColumnName.ToLower())
                {
                    strEntity += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = model." + _context.Utility.CleanUpProperty(c.Name, false) + "," + Environment.NewLine;
                }
                else if (!c.IsInPrimaryKey && c.IsInForeignKey)
                {
                }
                else
                {
                    strEntity += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = VersionConverter.ToBinary(model." + _context.Utility.CleanUpProperty(c.Name, false) + ")," + Environment.NewLine;
                }
            }

            lastComma = strEntity.LastIndexOf(",");
            strEntity = strEntity.Substring(0, lastComma);

            output.autoTabLn("using System;");
            output.autoTabLn("using System.Data;");
            output.autoTabLn("using System.Linq;");
            output.autoTabLn("using System.Linq.Dynamic;");
            output.autoTabLn("using System.Collections.Generic;");
            output.autoTabLn("using System.Data.Objects.DataClasses;");
            output.autoTabLn("");
            output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "Mapper");
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("public static " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " ToBusinessObject(" + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " entity)");
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("if (entity == null) return null;");
            output.autoTabLn("");
            output.autoTabLn("return new " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)));
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn(str);
            output.tabLevel--;
            output.autoTabLn("};");
            output.tabLevel--;
            output.autoTabLn("}");
            output.autoTabLn("");
            output.autoTabLn("public static " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " ToEntity(" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " model)");
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("if (model == null) return null;");
            output.autoTabLn("");
            output.autoTabLn("return new " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)));
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn(strEntity);
            output.tabLevel--;
            output.autoTabLn("};");
            output.tabLevel--;
            output.autoTabLn("}");
            output.autoTabLn("");
            output.autoTabLn("public static List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> ToBusinessObjects(EntityCollection<" + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> entities)");
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("if (entities == null) return null;");
            output.autoTabLn("return entities.Select(o => ToBusinessObject(o)).ToList();");
            output.tabLevel--;
            output.autoTabLn("}");
            output.tabLevel--;
            output.autoTabLn("}");
            output.tabLevel--;
            output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Mapper.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\EntityMapper", StringFormatter.CleanUpClassName(table.Name) + "Mapper.cs"), SaveActions.DontOverwrite);			
        }

        private void RenderConcreteClass(ITable table)
        {
            hdrUtil.WriteClassHeader(output);

            output.autoTabLn("using System;");
            output.autoTabLn("using System.Collections.Generic;");
            output.autoTabLn("using System.Linq;");
            output.autoTabLn("using System.Text;");
            output.autoTabLn("using System.Data.Linq;");
            output.autoTabLn("");
            output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntityFramework;");
            output.autoTabLn("");
            output.autoTabLn("using System.Linq.Dynamic;");
            output.autoTabLn("");
            output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".SQLServer");
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("public class " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ", I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name);
            output.autoTabLn("{");
            output.tabLevel--;
            output.autoTabLn("");

            _dataStore.GetById(table);
            _dataStore.GetAllWithSortingAndPaging(table);

            output.autoTabLn("");
            output.autoTabLn("");
            output.tabLevel++;
            output.autoTabLn("}");
            output.tabLevel--;
            output.autoTabLn("}");

            _context.FileList.Add("    " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.DataStore.Selected, _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderBaseClass(ITable table)
        {
            hdrUtil.WriteClassHeader(output);

            output.autoTabLn("using System;");
            output.autoTabLn("using System.Collections.Generic;");
            output.autoTabLn("using System.Linq;");
            output.autoTabLn("using System.Text;");
            output.autoTabLn("using System.Data.Linq;");
            output.autoTabLn("");
            output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntityFramework;");
            output.autoTabLn("");
            output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".SQLServer");
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "BaseDao : ICRUDDao<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">");
            output.autoTabLn("{");
            output.tabLevel++;
            output.autoTabLn("#region CRUD Methods");
            output.autoTabLn("");
            output.tabLevel--;
            output.autoTabLn("");

            _dataStore.GetAll(table);
            _dataStore.Insert(table);
            _dataStore.Update(table);
            _dataStore.Delete(table);

            output.autoTabLn("");
            output.tabLevel++;
            output.autoTabLn("#endregion");
            output.tabLevel--;
            output.autoTabLn("}");
            output.tabLevel--;
            output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.DataStore.Selected + "\\Generated", StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.Overwrite);
        }

        #endregion
    }
}
