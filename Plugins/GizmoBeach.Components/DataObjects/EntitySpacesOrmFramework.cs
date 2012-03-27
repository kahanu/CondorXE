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

            CreateEntitySpacesFolder();
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
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected);
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
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.DataStore.Selected, _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
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
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.DataStore.Selected);
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
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\" + _script.Settings.DataOptions.DataStore.Selected + "\\Generated", StringFormatter.CleanUpClassName(table.Name) + "Base" + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.Overwrite);
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

        #endregion
    }
}


