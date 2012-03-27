using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Zeus;
using MyMeta;

namespace GizmoBeach.Components.DataObjects
{
    public class SQLServerEntitySpacesDataStore : IDataStore
    {
        #region ctors
        protected IZeusOutput _output;
        protected ScriptSettings _script;
        protected CommonUtility _util;
        protected RequestContext _context;

        public SQLServerEntitySpacesDataStore(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
            this._script = context.ScriptSettings;
            this._util = context.Utility;
        }
        #endregion

        #region IDataStore Members

        public void GetAll(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// GetAll and map the fields to the entity");
            _output.autoTabLn("/// and add to generic queryable collection.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public List<" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + "> GetAll()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(StringFormatter.CleanUpClassName(tableName) + "Collection coll = new " + StringFormatter.CleanUpClassName(tableName) + "Collection();");
            _output.autoTabLn("coll.LoadAll();");
            _output.autoTabLn("");
            _output.autoTabLn("return coll.ToBusinessObjects();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.tabLevel--;
        }

        public void Insert(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Insert new " + StringFormatter.CleanUpClassName(tableName) + ".");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"" + StringFormatter.CleanUpClassName(tableName) + "\"></param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public void Insert(" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(StringFormatter.CleanUpClassName(tableName) + "Collection coll = new " + StringFormatter.CleanUpClassName(tableName) + "Collection();");
            _output.autoTabLn(StringFormatter.CleanUpClassName(tableName) + " entity = coll.AddNew();");
            _output.autoTabLn("entity = model.ToEntity(entity);");
            _output.autoTabLn("");
            _output.autoTabLn("coll.Save();");
            _output.autoTabLn("");

            GenTable genTable = new GenTable(table, _context);
            string args = "";
            string keys = "";
            string types = "";

            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                // type + " " + key name
                args += genTable.GetPrimaryKeyTypes[i] + " ";
                args += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
                keys += StringFormatter.PascalCasing(genTable.GetPrimaryKeyNames[i]) + ",";
                types += genTable.GetPrimaryKeyTypes[i] + ",";
            }
            // trim the trailing ", "
            args = args.Substring(0, (args.Length - 2));
            keys = keys.Substring(0, (keys.Length - 1));
            types = types.Substring(0, (types.Length - 1));

            string[] keysSplit = keys.Split(',');
            string[] argsSplit = args.Split(',');
            string[] typesSplit = types.Split(',');
            int counter = 0;
            foreach (string key in keysSplit)
            {
                _output.autoTabLn("model." + key.Trim() + " = (" + typesSplit[counter] + ")entity." + key.Trim() + ";");
                counter++;
            }

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.tabLevel--;

        }

        public void Update(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Update " + StringFormatter.CleanUpClassName(tableName) + ".  Map  fields to " + StringFormatter.CleanUpClassName(tableName) + " properties.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"" + StringFormatter.CleanUpClassName(tableName) + "\"></param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public void Update(" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(StringFormatter.CleanUpClassName(tableName) + " entity = new " + StringFormatter.CleanUpClassName(tableName) + "();");

            GenTable genTable = new GenTable(table, _context);
            string args = "";
            string keys = "";
            string types = "";

            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                // type + " " + key name
                args += genTable.GetPrimaryKeyTypes[i] + " ";
                args += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
                keys += StringFormatter.PascalCasing(genTable.GetPrimaryKeyNames[i]) + ",";
                types += genTable.GetPrimaryKeyTypes[i] + ",";
            }
            // trim the trailing ", "
            args = args.Substring(0, (args.Length - 2));
            keys = keys.Substring(0, (keys.Length - 1));
            types = types.Substring(0, (types.Length - 1));

            _output.autoTabLn("if (entity.LoadByPrimaryKey(model." + keys + "))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("entity = model.ToEntity(entity);");
            _output.autoTabLn("entity.Save();");
            _output.autoTabLn("");

            string[] keysSplit = keys.Split(',');
            string[] argsSplit = args.Split(',');
            string[] typesSplit = types.Split(',');
            int counter = 0;
            foreach (string key in keysSplit)
            {
                _output.autoTabLn("model." + key.Trim() + " = (" + typesSplit[counter] + ")entity." + key.Trim() + ";");
                counter++;
            }

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.tabLevel--;

        }

        public void Delete(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Delete " + StringFormatter.CleanUpClassName(tableName) + ".");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"" + StringFormatter.CleanUpClassName(tableName) + "\"></param>");
            _output.autoTabLn("public void Delete(" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(StringFormatter.CleanUpClassName(tableName) + " entity = new " + StringFormatter.CleanUpClassName(tableName) + "();");

            GenTable genTable = new GenTable(table, _context);
            string args = "";
            string keys = "";
            string types = "";

            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                // type + " " + key name
                args += genTable.GetPrimaryKeyTypes[i] + " ";
                args += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
                keys += StringFormatter.PascalCasing(genTable.GetPrimaryKeyNames[i]) + ",";
                types += genTable.GetPrimaryKeyTypes[i] + ",";
            }
            // trim the trailing ", "
            args = args.Substring(0, (args.Length - 2));
            keys = keys.Substring(0, (keys.Length - 1));
            types = types.Substring(0, (types.Length - 1));

            _output.autoTabLn("if (entity.LoadByPrimaryKey(model." + keys + "))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("entity.MarkAsDeleted();");
            _output.autoTabLn("entity.Save();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;

        }

        public void GetById(ITable table)
        {
            string tableName = table.Name;
            string args = string.Empty;
            string keys = string.Empty;

            GenTable genTable = new GenTable(table, _context);

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
            _output.write(args.Trim());
            _output.writeln(")");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(_util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + " model = null;");
            _output.autoTabLn(StringFormatter.CleanUpClassName(tableName) + " entity = new " + StringFormatter.CleanUpClassName(tableName) + "();");

            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                _output.autoTabLn("if (entity.LoadByPrimaryKey(" + StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + "))");
            }
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("model = entity.ToBusinessObject();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("return model;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.tabLevel--;

        }

        public void GetAllWithSortingAndPaging(ITable table)
        {
            string tableName = table.Name;

            _output.tabLevel++;
            _output.autoTabLn("public List<" + _util.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(tableName)) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(StringFormatter.CleanUpClassName(tableName) + "Collection coll = new " + StringFormatter.CleanUpClassName(tableName) + "Collection();");
            _output.autoTabLn("coll.Query.OrderBy(sortExpression);");
            _output.autoTabLn("coll.Query.Skip(startRowIndex).Take(maximumRows);");
            _output.autoTabLn("coll.Query.Load();");
            _output.autoTabLn("");
            _output.autoTabLn("return coll.ToBusinessObjects();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;

        }

        public void Mapper(ITable table)
        {

        }

        public void Interface(ITable table)
        {

        }

        public void CRUDInterface()
        {

        }

        #endregion
    }
}

