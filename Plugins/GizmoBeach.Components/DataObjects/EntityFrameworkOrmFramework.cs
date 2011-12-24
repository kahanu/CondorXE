using System;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.DataObjects
{
    public class EntityFrameworkOrmFramework : RenderBase, IORMFramework
    {
        #region ctors
        protected IDataStore _dataStore;
        protected RequestContext _context;
        protected IDatabase _database;
        protected ProgressDialogWrapper _dialog;

        public EntityFrameworkOrmFramework(IDataStore dataStore, RequestContext context)
            : base(context.Zeus.Output)
        {
            if (dataStore == null)
                throw new ApplicationException("EntityFrameworkOrmFramework dataStore");

            this._dataStore = dataStore;
            this._context = context;
            this._database = context.Database;
            this._dialog = context.Dialog;
        } 
        #endregion

        #region IORMFramework Members

        public string Name
        {
            get { return "Entity Framework"; }
        }

        public void Generate()
        {
            _dialog.InitDialog(4);
            _dialog.Display("Processing Entity Framework 4.0 Helper classes");
            _context.FileList.Add("");
            _context.FileList.Add("Generated Entity Framework 4.0 Helper Classes:");

            _dialog.Display("Processing Entity Framework 4.0 DataObjectFactory class.");
            RenderDataObjectFactory();

            _dialog.Display("Processing Entity Framework 4.0 DataObjects Extensions class.");
            RenderDataObjectsExtensions();

            _dialog.Display("Processing Entity Framework 4.0 DynamicLinq class.");
            RenderDynamicLinqClass();


            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Entity Framework 4.0 Data Objects Mapper Classes:");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing Entity Framework 4.0 Data Objects Mapper classes for '" + table.Name + "'");
                RenderMapperClass(table);
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Entity Framework 4.0 Data Objects Interfaces:");
            RenderGenericCRUDDaoInterface();
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing Entity Framework 4.0 Data Objects Interfaces for '" + table.Name + "'");
                RenderDaoInterface(table);
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Entity Framework 4.0 Data Objects Base Classes: ");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing Entity Framework 4.0 Data Objects Base Classes for '" + table.Name + "'");
                RenderBaseClass(table);
            }

            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated Entity Framework 4.0 Data Objects Concrete Classes: ");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing Entity Framework 4.0 Data Objects Concrete Classes for '" + table.Name + "'");
                RenderConcreteClass(table);
            }
        }

        #endregion


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
            _output.autoTabLn("using System.Linq.Dynamic;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : " + StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ", I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name);
            _output.autoTabLn("{");
            _output.tabLevel--;
            _output.autoTabLn("");

            _dataStore.GetById(table);
            _dataStore.GetAllWithSortingAndPaging(table);

            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.DataStore.Selected, _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
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
            _output.autoTabLn("");

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

        private void RenderDaoInterface(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + "<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " GetById(int id);");
            _output.autoTabLn("List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + "I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Interfaces", "I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderGenericCRUDDaoInterface()
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
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Interfaces", "ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.Overwrite);
        }

        private void RenderMapperClass(ITable table)
        {
            string str = "";
            string tableName = table.Name;

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
                    str += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = " + "entity." + _context.Utility.CleanUpProperty(c.Name, false) + ".AsBase64String()," + Environment.NewLine;
                }
            }

            // Now include any foreign key relationships
            foreach (IForeignKey key in table.ForeignKeys)
            {
                /*
                if (script.Tables.Contains(key.ForeignTable.Name))
                {
                    if (key.PrimaryTable.Name == tableName)
                        str += "				" + StringFormatter.CleanUpClassName(StringFormatter.MakeSingular(key.ForeignTable.Name), false) + "List = " + StringFormatter.CleanUpClassName(StringFormatter.MakeSingular(key.ForeignTable.Name), false) + "Mapper.ToBusinessObjects(entity." + StringFormatter.CleanUpClassName(StringFormatter.MakePlural(key.ForeignTable.Name), false) + ")," + Environment.NewLine;
                }
                */

                if (_script.Tables.Contains(key.PrimaryTable.Name))
                {
                    if (key.PrimaryTable.Name != tableName)
                        str += "				" + StringFormatter.CleanUpClassName(StringFormatter.MakeSingular(key.PrimaryTable.Name)) + " = " + StringFormatter.CleanUpClassName(StringFormatter.MakeSingular(key.PrimaryTable.Name)) + "Mapper.ToBusinessObject(entity." + StringFormatter.CleanUpClassName(StringFormatter.MakeSingular(key.PrimaryTable.Name)) + ")," + Environment.NewLine;
                }
            }

            int lastComma = str.LastIndexOf(",");
            str = str.Substring(0, lastComma);

            string strEntity = string.Empty;

            foreach (Column c in table.Columns)
            {
                if (c.Name.ToLower() != _script.Settings.DataOptions.VersionColumnName.ToLower())
                {
                    strEntity += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = model." + _context.Utility.CleanUpProperty(c.Name, false) + "," + Environment.NewLine;
                }
                else
                {
                    strEntity += "				" + _context.Utility.CleanUpProperty(c.Name, false) + " = model." + _context.Utility.CleanUpProperty(c.Name, false) + ".AsByteArray()," + Environment.NewLine;
                }
            }


            lastComma = strEntity.LastIndexOf(",");
            strEntity = strEntity.Substring(0, lastComma);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Data;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Linq.Dynamic;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Data.Objects.DataClasses;");
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
            _output.autoTabLn("if (entity == null) return null;");
            _output.autoTabLn("");
            _output.autoTabLn("return new " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)));
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(str);
            _output.tabLevel--;
            _output.autoTabLn("};");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public static " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " ToEntity(" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (model == null) return null;");
            _output.autoTabLn("");
            _output.autoTabLn("return new " + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)));
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(strEntity);
            _output.tabLevel--;
            _output.autoTabLn("};");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public static List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> ToBusinessObjects(EntityCollection<" + _context.Utility.BuildEntityClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> entities)");
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

        private void RenderDynamicLinqClass()
        {
            


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

        private void RenderDataObjectFactory()
        {
            _hdrUtil.WriteClassHeader(_output);

_output.autoTabLn("using System.Configuration;");
_output.autoTabLn("");
_output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected);
_output.autoTabLn("{");
	_output.tabLevel++;
	_output.autoTabLn("/// <summary>");
	_output.autoTabLn("/// DataObjectFactory caches the connectionstring so that the context can be created quickly.");
	_output.autoTabLn("/// </summary>");
	_output.autoTabLn("public static class DataObjectFactory");
	_output.autoTabLn("{");
		_output.tabLevel++;
		_output.autoTabLn("private static readonly string _connectionString;");
		_output.autoTabLn("");
		_output.autoTabLn("/// <summary>");
		_output.autoTabLn("/// Static constructor. Reads the connectionstring from web.config just once.");
		_output.autoTabLn("/// </summary>");
		_output.autoTabLn("static DataObjectFactory()");
		_output.autoTabLn("{");
			_output.tabLevel++;
			_output.autoTabLn("string connectionStringName = ConfigurationManager.AppSettings.Get(\"ConnectionStringName\");");
			_output.autoTabLn("_connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;");
		_output.tabLevel--;
		_output.autoTabLn("}");
		_output.autoTabLn("");
		_output.autoTabLn("/// <summary>");
		_output.autoTabLn("/// Creates the Context using the current connectionstring.");
		_output.autoTabLn("/// </summary>");
		_output.autoTabLn("/// <remarks>");
		_output.autoTabLn("/// Gof pattern: Factory method. ");
		_output.autoTabLn("/// </remarks>");
		_output.autoTabLn("/// <returns>Action Entities context.</returns>");
		_output.autoTabLn("public static " + _script.Settings.DataOptions.DataContext.Name + " CreateContext()");
		_output.autoTabLn("{");
			_output.tabLevel++;
            _output.autoTabLn("return new " + _script.Settings.DataOptions.DataContext.Name + "(_connectionString);");
		_output.tabLevel--;
		_output.autoTabLn("}");
	_output.tabLevel--;
	_output.autoTabLn("}");
_output.tabLevel--;
_output.autoTabLn("}");

        _context.FileList.Add("    DataObjectFactory.cs");
		SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.ORMFramework.Selected, "DataObjectFactory.cs"), SaveActions.Overwrite);
        }            
    }
}


