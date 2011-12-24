using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Zeus;

namespace GizmoBeach.Components.DataObjects
{
    public class SQLServerLinqToSqlDataStore : IDataStore
    {
        #region ctors
        protected IZeusOutput _output;
        protected ScriptSettings _script;
        protected CommonUtility _util;
        protected RequestContext _context;

        public SQLServerLinqToSqlDataStore(RequestContext context)
        {
            this._output = context.Zeus.Output;
            this._script = context.ScriptSettings;
            this._util = context.Utility;
            this._context = context;
        }
        #endregion

        #region IDataStore Members

        public void GetAll(MyMeta.ITable table)
        {
            string tableName = table.Name;
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

            _output.autoTabLn("using (var context = DataContextFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return context." + StringFormatter.CleanUpClassName(StringFormatter.MakePlural(tableName)) + ".Select(o => " + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToBusinessObject(o)).ToList();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void Insert(MyMeta.ITable table)
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
            _output.autoTabLn("using (var context = DataContextFactory.CreateContext())");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(StringFormatter.MakePlural(tableName)) + ".InsertOnSubmit(entity);");
            _output.autoTabLn("context.SubmitChanges();");

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (ChangeConflictException)");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("foreach (ObjectChangeConflict conflict in context.ChangeConflicts)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("conflict.Resolve(RefreshMode.KeepCurrentValues);");

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("context.SubmitChanges();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (ChangeConflictException)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("throw new Exception(\"A concurrency error occurred!\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("throw new Exception(\"There was an error inserting the record!\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void Update(MyMeta.ITable table)
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
            _output.autoTabLn("using (var context = DataContextFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(StringFormatter.MakePlural(tableName)) + ".Attach(entity);");
            _output.autoTabLn("context.SubmitChanges();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (ChangeConflictException)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("foreach (ObjectChangeConflict conflict in context.ChangeConflicts)");

            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("conflict.Resolve(RefreshMode.KeepChanges);");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("context.SubmitChanges();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (ChangeConflictException)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("throw new Exception(\"A concurrency error occurred!\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("throw new Exception(\"There was an error updating the record!\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void Delete(MyMeta.ITable table)
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
            _output.autoTabLn("var entity = " + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToEntity(model);");
            _output.autoTabLn("");
            _output.autoTabLn("using (var context = DataContextFactory.CreateContext())");
            _output.autoTabLn("{");

            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(StringFormatter.MakePlural(tableName)) + ".Attach(entity);");
            _output.autoTabLn("context." + StringFormatter.CleanUpClassName(StringFormatter.MakePlural(tableName)) + ".DeleteOnSubmit(entity);");
            _output.autoTabLn("context.SubmitChanges();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (ChangeConflictException)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("foreach (ObjectChangeConflict conflict in context.ChangeConflicts)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("conflict.Resolve(RefreshMode.KeepChanges);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("context.SubmitChanges();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (ChangeConflictException)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("throw new Exception(\"A concurrency error occurred!\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("throw new Exception(\"There was an error deleting the record!\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void GetById(MyMeta.ITable table)
        {
            string args = string.Empty;
            string keys = string.Empty;
            string tableName = table.Name;

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
            _output.autoTabLn("using (var context = DataContextFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return " + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToBusinessObject(context." + StringFormatter.MakePlural(StringFormatter.CleanUpClassName(tableName)));
            string[] keysSplit = keys.Split(',');
            foreach (string key in keysSplit)
            {
                _output.tabLevel++;
                _output.autoTabLn(".Where(o => o." + _util.CleanUpProperty(key) + " == " + key + ")");
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

        public void GetAllWithSortingAndPaging(MyMeta.ITable table)
        {
            string tableName = table.Name;
            string model = _script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + StringFormatter.CleanUpClassName(tableName);
            string entity = _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + "." + StringFormatter.CleanUpClassName(tableName);

            _output.tabLevel++;
            _output.tabLevel++;

            _output.autoTabLn("public List<" + model + "> GetAll(string sortExpression, int startRowIndex, int maximumRows)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("using (var context = DataContextFactory.CreateContext())");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("IQueryable<" + entity + "> query = context." + StringFormatter.CleanUpClassName(StringFormatter.MakePlural(tableName)) + ";");
            _output.autoTabLn("query = GetListSort(query, sortExpression);");
            _output.autoTabLn("query = query.Skip(startRowIndex).Take(maximumRows);");
            _output.autoTabLn("return query.Select(o => " + StringFormatter.CleanUpClassName(tableName) + "Mapper.ToBusinessObject(o)).ToList();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void Mapper(MyMeta.ITable table)
        {
            
        }

        public void Interface(MyMeta.ITable table)
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
            //str += "List<" + StringFormatter.CleanUpClassName(tableName) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows);";
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
