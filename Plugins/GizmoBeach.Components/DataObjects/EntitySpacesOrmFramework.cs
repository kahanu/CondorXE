using System;
using Condor.Core;
using Condor.Core.Infrastructure;
using Condor.Core.Interfaces;
using MyMeta;
using Zeus;

namespace GizmoBeach.Components.DataObjects
{
    public class EntitySpacesOrmFramework : RenderBase, IORMFramework
    {
        #region ctors
        protected IDataStore _dataStore;
        protected RequestContext _context;
        protected IDatabase _database;
        protected IZeusOutput output;
        protected ProgressDialogWrapper _dialog;
        protected ICommonGenerators _commonGenerators;
        protected IAutoMapperFramework _autoMapperFramework;

        public EntitySpacesOrmFramework(IDataStore dataStore, RequestContext context)
            : this(dataStore, context, null)
        {

        }

        public EntitySpacesOrmFramework(IDataStore dataStore, RequestContext context, IAutoMapperFramework autoMapperFramework)
            : base(context.Zeus.Output)
        {
            this._dataStore = dataStore;
            this._context = context;
            this._database = context.Database;
            this._dialog = context.Dialog;
            this._commonGenerators = new CommonGenerators(context);
            this._autoMapperFramework = autoMapperFramework;

        }
        #endregion

        #region IORMFramework Members

        public string Name
        {
            get { return "EntitySpaces"; }
        }

        public void Generate()
        {
            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Starting DataObjects classes for " + Name + " Framework");

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated DataObjects CRUD Interface:");
            RenderCRUDInterface();

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated DataObjects Interfaces:");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing DataObjects interface for '" + table.Name + "'");
                RenderInterface(table);
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated DataObjects Base classes:");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing DataObjects Base class for '" + table.Name + "'");
                RenderBaseClass(table);
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated DataObjects Concrete classes:");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing DataObjects Concrete class for '" + table.Name + "'");
                RenderConcreteClass(table);
            }

            if (_autoMapperFramework == null)
            {
                _dialog.InitDialog();
                _context.FileList.Add("");
                _context.FileList.Add("Generated DataObjects Mapper classes:");
                foreach (string tableName in _script.Tables)
                {
                    ITable table = _database.Tables[tableName];
                    _dialog.Display("Processing DataObjects Mapper class for '" + table.Name + "'");

                    RenderMapperClass(table);
                }
            }


            RenderAutoMapperExtensions();

            //CreateEntitySpacesFolder();

            RenderDataObjectsExtensions();
        }

        private void RenderMapperClass(ITable table)
        {


            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntityMapper");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class " + StringFormatter.CleanUpClassName(table.Name) + "Mapper");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " ToBusinessObject(this " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " entity)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (entity == null) return null;");
            _output.autoTabLn("");

            _output.autoTabLn("var model = new " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "();");

            string line = string.Empty;

            // ToBusinessObject Loop
            foreach (Column c in table.Columns)
            {
                if (c.IsInPrimaryKey)
                {
                    _output.autoTabLn("model." + _context.Utility.CleanUpProperty(c.Name) + " = " + "(int)entity." + _context.Utility.CleanUpProperty(c.Name) + ";");
                }
                else if (c.Name.ToLower() == _context.ScriptSettings.Settings.DataOptions.VersionColumnName)
                {
                    _output.autoTabLn("model." + _context.ScriptSettings.Settings.DataOptions.VersionColumnName + " = entity." + _context.Utility.CleanUpProperty(c.Name, true) + ".AsBase64String();");
                }
                else
                {
                    line = "model." + _context.Utility.CleanUpProperty(c.Name) + " = " + "entity." + _context.Utility.CleanUpProperty(c.Name);
                    if (c.LanguageType.ToLower() != "string")
                    {
                        //if (c.IsNullable)
                        //{
                            line += ".HasValue ? (" + c.LanguageType + ")entity." + _context.Utility.CleanUpProperty(c.Name) + " : default(" + c.LanguageType + ")";
                        //}
                    }
                    _output.autoTabLn(line + ";");
                }
            }

            // Now include any foreign key relationships
            foreach (IForeignKey key in table.ForeignKeys)
            {
                if (_script.Tables.Contains(key.PrimaryTable.Name))
                {
                    if (key.PrimaryTable.Name != table.Name)
                        _output.autoTabLn("model." + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + " = " + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + "Mapper.ToBusinessObject(entity.UpTo" + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + "By" + key.ForeignColumns[0].Name + ");");
                }
            }


            _output.autoTabLn("");
            _output.autoTabLn("return model;");


            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public static " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " ToEntity(this " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " model, " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " entity)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (model == null) return null;");
            _output.autoTabLn("");
            _output.tabLevel++;

            // ToEntity Loop
            foreach (Column c in table.Columns)
            {
                if (c.Name.ToLower() == _context.ScriptSettings.Settings.DataOptions.VersionColumnName.ToLower())
                {
                    _output.autoTabLn("entity." + _context.Utility.CleanUpProperty(c.Name, true) + " = model." + _context.Utility.CleanUpProperty(c.Name) + ".AsByteArray();");
                }
                else
                {
                    _output.autoTabLn("entity." + _context.Utility.CleanUpProperty(c.Name) + " = " + "model." + _context.Utility.CleanUpProperty(c.Name) + ";");
                }
            }



            _output.autoTabLn("return entity;");

            _output.tabLevel--;
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public static List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> ToBusinessObjects(this " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "Collection entities)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (entities == null) return null;");
            _output.autoTabLn("return entities.Select(o => ToBusinessObject(o)).ToList();");

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Mapper.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\EntityMapper", StringFormatter.CleanUpClassName(table.Name) + "Mapper.cs"), SaveActions.DontOverwrite);
        }
  
        private void RenderCRUDInterface()
        {
            _hdrUtil.WriteClassHeader(_output);

            _commonGenerators.RenderDaoCRUDInterface();

            _context.FileList.Add("    ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Interfaces", "ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.Overwrite);
        }

        private void CreateEntitySpacesFolder()
        {
            // Create a dummy file to just have the folder created.
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + Name, "Dummy.txt"), SaveActions.DontOverwrite);
        }

        private void RenderConcreteClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntityMapper;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntitySpaces;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + _script.Settings.DataOptions.DataStore.Selected);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ", I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name);
            _output.autoTabLn("{");
            _output.autoTabLn("");

            _dataStore.GetById(table);
            _dataStore.GetAllWithSortingAndPaging(table);

            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.ORMFramework.Selected + "\\" + _script.Settings.DataOptions.DataStore.Selected, _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderBaseClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntityMapper;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntitySpaces;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + _script.Settings.DataOptions.DataStore.Selected);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + " : ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + "<" + _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(table.Name) + ">");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region CRUD Methods");
            _output.tabLevel--;
            _output.autoTabLn("");

            _dataStore.GetAll(table);
            _dataStore.Insert(table);
            _dataStore.Update(table);
            _dataStore.Delete(table);

            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.ORMFramework.Selected + "\\" + _script.Settings.DataOptions.DataStore.Selected + "\\Generated", StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.Overwrite);
        }

        private void RenderInterface(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _commonGenerators.RenderDaoInterface(table);

            _context.FileList.Add("    " + "I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Interfaces", "I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderAutoMapperExtensions()
        {
            if (_autoMapperFramework != null)
            {
                bool useWebService = false;
                _autoMapperFramework.RenderAutoMapperExtensionClass(useWebService);
                _autoMapperFramework.RenderAutoMapperConfiguration(useWebService);
                _autoMapperFramework.RenderAutoMapperAppStart();

                foreach (string tableName in _script.Tables)
                {
                    ITable table = _database.Tables[tableName];
                    _autoMapperFramework.BuildModelClass(table, useWebService);
                }
            }
        }

        private void RenderDataObjectsExtensions()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Data.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Useful set of Extension methods for Data Access purposes.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("public static class Extensions");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into Identity data type (integer).");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <param name=\"defaultId\">Optional default value is -1.</param>");
            _output.autoTabLn("/// <returns>Identity value.</returns>");
            _output.autoTabLn("public static int AsId(this object item, int defaultId = -1)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null)");
            _output.tabLevel++;
            _output.autoTabLn("return defaultId;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("int result;");
            _output.autoTabLn("if (!int.TryParse(item.ToString(), out result))");
            _output.tabLevel++;
            _output.autoTabLn("return defaultId;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return result;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into integer data type.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <param name=\"defaultId\">Optional default value is default(int).</param>");
            _output.autoTabLn("/// <returns>The integer value.</returns>");
            _output.autoTabLn("public static int AsInt(this object item, int defaultInt = default(int))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null)");
            _output.tabLevel++;
            _output.autoTabLn("return defaultInt;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("int result;");
            _output.autoTabLn("if (!int.TryParse(item.ToString(), out result))");
            _output.tabLevel++;
            _output.autoTabLn("return defaultInt;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return result;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into double data type.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <param name=\"defaultId\">Optional default value is default(double).</param>");
            _output.autoTabLn("/// <returns>The double value.</returns>");
            _output.autoTabLn("public static double AsDouble(this object item, double defaultDouble = default(double))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null)");
            _output.tabLevel++;
            _output.autoTabLn("return defaultDouble;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("double result;");
            _output.autoTabLn("if (!double.TryParse(item.ToString(), out result))");
            _output.tabLevel++;
            _output.autoTabLn("return defaultDouble;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return result;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into string data type.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <param name=\"defaultId\">Optional default value is default(string).</param>");
            _output.autoTabLn("/// <returns>The string value.</returns>");
            _output.autoTabLn("public static string AsString(this object item, string defaultString = default(string))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null || item.Equals(System.DBNull.Value))");
            _output.tabLevel++;
            _output.autoTabLn("return defaultString;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return item.ToString().Trim();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into DateTime data type.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <param name=\"defaultId\">Optional default value is default(DateTime).</param>");
            _output.autoTabLn("/// <returns>The DateTime value.</returns>");
            _output.autoTabLn("public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null || string.IsNullOrEmpty(item.ToString()))");
            _output.tabLevel++;
            _output.autoTabLn("return defaultDateTime;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("DateTime result;");
            _output.autoTabLn("if (!DateTime.TryParse(item.ToString(), out result))");
            _output.tabLevel++;
            _output.autoTabLn("return defaultDateTime;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return result;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into bool data type.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <param name=\"defaultId\">Optional default value is default(bool).</param>");
            _output.autoTabLn("/// <returns>The bool value.</returns>");
            _output.autoTabLn("public static bool AsBool(this object item, bool defaultBool = default(bool))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null)");
            _output.tabLevel++;
            _output.autoTabLn("return defaultBool;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return new List<string>() { \"yes\", \"y\", \"true\" }.Contains(item.ToString().ToLower());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform string into byte array.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"s\">The object to be transformed.</param>");
            _output.autoTabLn("/// <returns>The transformed byte array.</returns>");
            _output.autoTabLn("public static byte[] AsByteArray(this string s)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (string.IsNullOrEmpty(s))");
            _output.tabLevel++;
            _output.autoTabLn("return null;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return Convert.FromBase64String(s);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into base64 string.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <returns>The base64 string value.</returns>");
            _output.autoTabLn("public static string AsBase64String(this object item)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null)");
            _output.tabLevel++;
            _output.autoTabLn("return null;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return Convert.ToBase64String((byte[]) item); ");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform Binary into base64 string data type. ");
            _output.autoTabLn("/// Note: This is used in LINQ to SQL only.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <returns>The base64 string value.</returns>");
            _output.autoTabLn("public static string AsBase64String(this Binary item)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (item == null)");
            _output.tabLevel++;
            _output.autoTabLn("return null;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return Convert.ToBase64String(item.ToArray());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform base64 string to Binary data type. ");
            _output.autoTabLn("/// Note: This is used in LINQ to SQL only.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"s\">The base 64 string to be transformed.</param>");
            _output.autoTabLn("/// <returns>The Binary value.</returns>");
            _output.autoTabLn("public static Binary AsBinary(this string s)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (string.IsNullOrEmpty(s))");
            _output.tabLevel++;
            _output.autoTabLn("return null;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return new Binary(Convert.FromBase64String(s));");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Transform object into Guid data type.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"item\">The object to be transformed.</param>");
            _output.autoTabLn("/// <returns>The Guid value.</returns>");
            _output.autoTabLn("public static Guid AsGuid(this object item)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try { return new Guid(item.ToString()); }");
            _output.autoTabLn("catch { return Guid.Empty; }");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Concatenates SQL and ORDER BY clauses into a single string. ");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"sql\">The SQL string</param>");
            _output.autoTabLn("/// <param name=\"sortExpression\">The Sort Expression.</param>");
            _output.autoTabLn("/// <returns>Contatenated SQL Statement.</returns>");
            _output.autoTabLn("public static string OrderBy(this string sql, string sortExpression)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (string.IsNullOrEmpty(sortExpression))");
            _output.tabLevel++;
            _output.autoTabLn("return sql;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return sql + \" ORDER BY \" + sortExpression;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Takes an enumerable source and returns a comma separate string.");
            _output.autoTabLn("/// Handy to build SQL Statements (for example with IN () statements) from object collections.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <typeparam name=\"T\">The Enumerable type</typeparam>");
            _output.autoTabLn("/// <typeparam name=\"U\">The original data type (typically identities - int).</typeparam>");
            _output.autoTabLn("/// <param name=\"source\">The enumerable input collection.</param>");
            _output.autoTabLn("/// <param name=\"func\">The function that extracts property value in object.</param>");
            _output.autoTabLn("/// <returns>The comma separated string.</returns>");
            _output.autoTabLn("public static string CommaSeparate<T, U>(this IEnumerable<T> source, Func<T, U> func)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return string.Join(\",\", source.Select(s => func(s).ToString()).ToArray());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");

            _context.FileList.Add("    Extensions.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Shared", "Extensions.cs"), SaveActions.Overwrite);
        }


        #endregion
    }
}


