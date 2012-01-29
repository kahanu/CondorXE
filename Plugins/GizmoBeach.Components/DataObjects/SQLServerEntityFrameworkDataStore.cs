using System;
using System.Linq;
using Condor.Core;
using Zeus;
using MyMeta;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    public class SQLServerEntityFrameworkDataStore : IDataStore
    {
        #region ctors
        protected IZeusOutput _output;
        protected ScriptSettings _script;
        protected CommonUtility _util;
        protected RequestContext _context;

        public SQLServerEntityFrameworkDataStore(RequestContext context)
        {
            this._output = context.Zeus.Output;
            this._script = context.ScriptSettings;
            this._util = context.Utility;
            this._context = context;
        }
        #endregion

        #region Members

        public void GetAll(ITable table)
        {
            string tableName = table.Name;
            string model = _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName);
            string entity = _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + StringFormatter.CleanUpClassName(tableName);

            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// GetAll and map the fields to the entity");
            _output.autoTabLn("/// and add to generic queryable collection.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <returns></returns>");

            _output.autoTabLn("public List<" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + "> GetAll()");
            _output.autoTabLn("{");

            _output.tabLevel++;

            _output.autoTabLn("using (var context = DataObjectFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;

            _output.autoTabLn("var list = new List<" + model + ">();");
            _output.autoTabLn("var query = context." + StringFormatter.CleanUpClassName(tableName, true) + ".ToList();");
            _output.autoTabLn("foreach (" + entity + " entity in query)");
            _output.tabLevel++;
            _output.autoTabLn("list.Add(" + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToBusinessObject(entity));");
            _output.tabLevel--;
            _output.autoTabLn("return list;");

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;

        }

        public void Insert(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Insert new " + StringFormatter.CleanUpClassName(tableName) + ".");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName)) + "\"></param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public void Insert(" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model)");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("var entity = " + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToEntity(model);");
            _output.autoTabLn("");
            _output.autoTabLn("using (var context = DataObjectFactory.CreateContext())");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(tableName, true) + ".AddObject(entity);");
            _output.autoTabLn("context.SaveChanges();");

            // Updated 7/21/2011 by King Wilder
            // New addition to return the new entity ID

            /****** Begin new code ******/
            GenTable genTable = new GenTable(table, _context);
            string args = "";
            string keys = "";

            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                // type + " " + key name
                args += genTable.GetPrimaryKeyTypes[i] + " ";
                args += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
                keys += StringFormatter.PascalCasing(genTable.GetPrimaryKeyNames[i]) + ",";
            }
            // trim the trailing ", "
            args = args.Substring(0, (args.Length - 2));
            keys = keys.Substring(0, (keys.Length - 1));

            string[] keysSplit = keys.Split(',');
            foreach (string key in keysSplit)
            {
                _output.autoTabLn("model." + key.Trim() + " = entity." + key.Trim() + ";");
            }
            /****** End new code ******/



            _output.tabLevel--;

            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void Update(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Update " + StringFormatter.CleanUpClassName(tableName) + ".  Map  fields to " + StringFormatter.CleanUpClassName(tableName) + " properties.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName)) + "\"></param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public void Update(" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model)");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("var entity = " + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToEntity(model);");
            _output.autoTabLn("");
            _output.autoTabLn("using (var context = DataObjectFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;

            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(tableName, true) + ".Attach(entity);");
            _output.autoTabLn("context.ObjectStateManager.ChangeObjectState(entity, System.Data.EntityState.Modified);");
            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(tableName, true) + ".ApplyCurrentValues(entity);");
            _output.autoTabLn("context.SaveChanges();");
            _output.tabLevel--;

            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void Delete(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Delete " + StringFormatter.CleanUpClassName(tableName) + ".");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName)) + "\"></param>");
            _output.autoTabLn("public void Delete(" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model)");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("");
            _output.autoTabLn("using (var context = DataObjectFactory.CreateContext())");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("var entity = context." + StringFormatter.CleanUpClassName(tableName, true));

            GenTable genTable = new GenTable(table, _context);
            _output.tabLevel++;
            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                _output.autoTabLn(".Where(o => o." + genTable.GetPrimaryKeyNames[i] + " == model." + genTable.GetPrimaryKeyNames[i] + ")");
            }

            _output.autoTabLn(".SingleOrDefault();");
            _output.tabLevel--;
            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(tableName, true) + ".DeleteObject(entity);");
            _output.autoTabLn("context.SaveChanges();");

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void GetById(ITable table)
        {
            string tableName = table.Name;
            string args = string.Empty;
            string keys = string.Empty;

            GenTable genTable = new GenTable(table, _context);

            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// GetById and map the fields to the Model.");
            _output.autoTabLn("/// </summary>");
            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                _output.autoTabLn("/// <param name=\"" + StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + "\"></param>");
            }

            _output.autoTabLn("/// <returns></returns>");
            _output.autoTab("public " + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " GetById(");
            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                // type + " " + key name
                args += genTable.GetPrimaryKeyTypes[i] + " ";
                args += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
                keys += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
            }
            // trim the trailing ", "
            args = args.Substring(0, (args.Length - 2));
            keys = keys.Substring(0, (keys.Length - 2));
            _output.write(args);
            _output.writeln(")");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("using (var context = DataObjectFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return " + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToBusinessObject(context." + StringFormatter.CleanUpClassName(tableName, true));
            string[] keysSplit = keys.Split(',');
            foreach (string key in keysSplit)
            {
                _output.tabLevel++;
                _output.autoTabLn(".Where(o => o." + _util.CleanUpProperty(key, true) + " == " + key + ")");
            }
            _output.autoTabLn(".SingleOrDefault());");
            _output.tabLevel--;
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void GetAllWithSortingAndPaging(ITable table)
        {
            string tableName = table.Name;
            string model = _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName);
            string entity = _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + StringFormatter.CleanUpClassName(tableName);

            _output.tabLevel++;
            _output.tabLevel++;

            _output.autoTabLn("public List<" + model + "> GetAll(string sortExpression, int startRowIndex, int maximumRows)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("using (var context = DataObjectFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;

            _output.autoTabLn("IQueryable<" + entity + "> query = context." + StringFormatter.CleanUpClassName(tableName, true) + ".AsQueryable().OrderBy(sortExpression);");
            _output.autoTabLn("query = query.Skip(startRowIndex).Take(maximumRows);");
            _output.autoTabLn("var list = new List<" + model + ">();");
            _output.autoTabLn("foreach (" + entity + " entity in query)");
            _output.tabLevel++;
            _output.autoTabLn("list.Add(" + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToBusinessObject(entity));");
            _output.tabLevel--;
            _output.autoTabLn("return list;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void Mapper(ITable table)
        {
            
        }

        public void Interface(ITable table)
        {
            string tableName = table.Name;
            string args = "";
            GenTable genTable = new GenTable(table, _context);
            string str = "";
            str += _util.BuildModelClassWithNameSpace(tableName) + " GetById(";

            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                // type + " " + key name
                args += genTable.GetPrimaryKeyTypes[i] + " ";
                args += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
            }
            // trim the trailing ", "

            args = args.Substring(0, (args.Length - 2));

            str += args;
            str += ");";
            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn(str);
            _output.tabLevel--;
            _output.tabLevel--;

            str = "";
            str += "List<" + _util.BuildModelClassWithNameSpace(tableName) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows);";
            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn(str);
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void CRUDInterface()
        {
            
        }

        #endregion
    }
}

