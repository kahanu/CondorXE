using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.DataObjects
{
    public class LinqToSqlOrmFramework : RenderBase, IORMFramework
    {
        #region ctors

        private readonly RequestContext _context;
        private readonly ProgressDialogWrapper _dialog;
        private readonly IDatabase _database;
        private readonly IDataStore _dataStore;

        public LinqToSqlOrmFramework(IDataStore dataStore, RequestContext context):base(context.Zeus.Output)
        {
            this._dataStore = dataStore;
            this._context = context;
            this._database = context.Database;
            this._dialog = context.Dialog;
        }

        #endregion

        #region IORMFramework Members

        public string Name
        {
            get { return "Linq-To-Sql Framework"; }
        }

        public void Generate()
        {
            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Linq-To-Sql Interfaces:");
            RenderCRUDInterface();
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderInterface(table);
                _dialog.Display("Processing I" + table.Name + _script.Settings.DataOptions.ClassSuffix + ".cs");
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Linq-To-Sql Mappers:");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderMapper(table);
                _dialog.Display("Processing " + table.Name + "Mapper.cs");
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Linq-To-Sql Base classes:");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderBaseClass(table);
                _dialog.Display("Processing " + table.Name + "Base" + _script.Settings.DataOptions.ClassSuffix + ".cs");
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Linq-To-Sql Concrete classes:");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                RenderConcreteClass(table);
                _dialog.Display("Processing " + _script.Settings.DataOptions.DataStore.Selected + table.Name + _script.Settings.DataOptions.ClassSuffix + ".cs");
            }

            _dialog.InitDialog(1);
            _dialog.Display("Processing Linq-To-Sql Helper classes");
            _context.FileList.Add("");
            _context.FileList.Add("Generated Linq-To-Sql Helper Classes:");
            RenderDataContextFactory();
            RenderVersionConverter();
        }

        private void RenderVersionConverter()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Data.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + StringFormatter.ConvertPathToNamespace(_script.Settings.Namespace + ".DataObjects") + "." + _script.Settings.DataOptions.ORMFramework.Selected);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class VersionConverter");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Converts binary value to string.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"version\">Binary version number.</param>");
            _output.autoTabLn("/// <returns>Base64 version number.</returns>");
            _output.autoTabLn("public static string ToString(Binary version)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (version == null)");
            _output.tabLevel++;
            _output.autoTabLn("return null;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return Convert.ToBase64String(version.ToArray());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Converts string to binary value.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"version\">Base64 version number.</param>");
            _output.autoTabLn("/// <returns>Binary version number.</returns>");
            _output.autoTabLn("public static Binary ToBinary(string version)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (string.IsNullOrEmpty(version))");
            _output.tabLevel++;
            _output.autoTabLn("return null;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return new Binary(Convert.FromBase64String(version));");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    VersionConverter.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.ORMFramework.Selected, "VersionConverter.cs"), SaveActions.Overwrite);
        }

        private void RenderDataContextFactory()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System.Configuration;");
            _output.autoTabLn("using System.Data.Linq;");
            _output.autoTabLn("using System.Data.Linq.Mapping;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + StringFormatter.ConvertPathToNamespace(_script.Settings.Namespace + ".DataObjects") + "." + _script.Settings.DataOptions.ORMFramework.Selected);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class DataContextFactory");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("private static readonly string _connectionString;");
            _output.autoTabLn("private static readonly MappingSource _mappingSource;");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Static constructor.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <remarks>");
            _output.autoTabLn("/// Static initialization of connectionstring and mappingSource.");
            _output.autoTabLn("/// This significantly increases performance, primarily due to mappingSource cache.");
            _output.autoTabLn("/// </remarks>		");
            _output.autoTabLn("static DataContextFactory()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("string connectionStringName = ConfigurationManager.AppSettings.Get(\"ConnectionStringName\");");
            _output.autoTabLn("_connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("DataContext context = new " + _script.Settings.DataOptions.DataContext.Name + "(_connectionString);");
            _output.autoTabLn("_mappingSource = context.Mapping.MappingSource;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Rapidly creates a new DataContext using cached connectionstring and mapping source.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public static " + _script.Settings.DataOptions.DataContext.Name + " CreateContext()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return new " + _script.Settings.DataOptions.DataContext.Name + "(_connectionString, _mappingSource);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    DataContextFactory.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.ORMFramework.Selected, "DataContextFactory.cs"), SaveActions.Overwrite);
        }

        private void RenderConcreteClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Text;");
            _output.autoTabLn("using System.Data.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + ";");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ", I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name);
            _output.autoTabLn("{");
            _output.tabLevel--;
            
            _dataStore.GetById(table);
            _dataStore.GetAllWithSortingAndPaging(table);
            BuildListSort(table);

            _output.tabLevel++;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.DataStore.Selected, _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void BuildListSort(ITable table)
        {
            string tableName = table.Name;
            //string model = _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName);
            string entity = _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + StringFormatter.CleanUpClassName(tableName);

            _output.tabLevel++;
            _output.tabLevel++;

            _output.autoTabLn("private IQueryable<" + entity + "> GetListSort(IQueryable<" + entity + "> query, string sortExpression)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("bool sortDescending = false;");
            _output.autoTabLn("string sortType = string.Empty;");
            _output.autoTabLn("if (!string.IsNullOrEmpty(sortExpression))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("string[] values = sortExpression.Split(' ');");
            _output.autoTabLn("sortType = values[0];");
            _output.autoTabLn("if (values.Length > 1)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("sortDescending = values[1] == \"DESC\";");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("switch (sortType)");
            _output.autoTabLn("{");
            _output.tabLevel++;

            bool dateExists = false;
            string dateColumnName = string.Empty;
            foreach (IColumn dc in table.Columns)
            {
                if (dc.LanguageType == "DateTime")
                {
                    dateExists = true;
                    dateColumnName = dc.Name;
                    break;
                }
            }

            foreach (IColumn c in table.Columns)
            {
                if (c.Name.ToLower() != _script.Settings.DataOptions.VersionColumnName.ToLower())
                {

                    _output.autoTabLn("case \"" + c.Name + "\":");
                    _output.tabLevel++;
                    _output.autoTabLn("if (sortDescending)");
                    _output.tabLevel++;
                    _output.autoTab("query = query.OrderByDescending(o => o." + c.Name + ")");
                    if (dateExists)
                    {
                        _output.writeln(".ThenBy(o => o." + dateColumnName + ");");
                    }
                    else
                    {
                        _output.writeln(";");
                    }
                    _output.tabLevel--;
                    _output.autoTabLn("else");
                    _output.tabLevel++;
                    _output.autoTab("query = query.OrderBy(o => o." + c.Name + ")");
                    if (dateExists)
                    {
                        _output.writeln(".ThenBy(o => o." + dateColumnName + ");");
                    }
                    else
                    {
                        _output.writeln(";");
                    }
                    _output.tabLevel--;
                    _output.autoTabLn("break;");
                    _output.tabLevel--;
                }
            }

            _output.autoTabLn("default:");
            _output.tabLevel++;
            if (dateExists)
            {
                _output.autoTabLn("query = query.OrderByDescending(o => o." + dateColumnName + ");");
            }
            _output.autoTabLn("break;");
            _output.tabLevel--;
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("return query;");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;
        }

        private void RenderBaseClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Text;");
            _output.autoTabLn("using System.Data.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + ";");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + " : ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + "<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region CRUD Methods");
            _output.autoTabLn("");
            _output.tabLevel--;
            
            _dataStore.GetAll(table);
            _dataStore.Insert(table);
            _dataStore.Update(table);
            _dataStore.Delete(table);
            
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.DataStore.Selected + "\\Generated", StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.Overwrite);
        }

        private void RenderMapper(ITable table)
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


            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + ";");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "Mapper");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " ToBusinessObject(" + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " entity)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return new " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)));
            _output.autoTabLn("{");
            _output.tabLevel--;

            _output.autoTabLn(str);
            
            _output.tabLevel++;
            _output.autoTabLn("};");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public static " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " ToEntity(" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return new " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)));
            _output.autoTabLn("{");
            _output.tabLevel--;
            
            _output.autoTabLn(strEntity);

            _output.tabLevel++;
            _output.autoTabLn("};");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Mapper.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\EntityMapper", StringFormatter.CleanUpClassName(table.Name) + "Mapper.cs"), SaveActions.Overwrite);
        }

        private void RenderInterface(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + "<" + StringFormatter.CleanUpClassName(table.Name) + ">");
            _output.autoTabLn("{");
            _output.tabLevel--;

            _dataStore.Interface(table);

            _output.tabLevel++;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + "I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Interfaces", "I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderCRUDInterface()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + "<T>");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region CRUD Methods");
            _output.autoTabLn("");
            _output.autoTabLn("List<T> GetAll();");
            _output.autoTabLn("void Insert(T model);");
            _output.autoTabLn("void Delete(T model);");
            _output.autoTabLn("void Update(T model);");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Interfaces", "ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.Overwrite);
        }

        #endregion
    }
}
