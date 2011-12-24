using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.ServiceLayer
{
    public class EntLibValidationServiceLayerObjects : RenderBase, IServiceObjects
    {
        #region ctors
        private readonly RequestContext _context;

        public EntLibValidationServiceLayerObjects(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }
        #endregion

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog(_context.Database.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated EntLibValidation Service Layer interfaces: ");
            RenderServiceCRUDInterface();
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding I" + tableName + "Service Interface");
                ITable table = _context.Database.Tables[tableName];
                RenderInterface(table);
            }

            _context.Dialog.InitDialog(_context.Database.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated EntLibValidation Service Layer Base classes: ");
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding " + tableName + "ServiceBase class");
                ITable table = _context.Database.Tables[tableName];
                RenderServiceBaseClass(table);
            }

            _context.Dialog.InitDialog(_context.Database.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated EntLibValidation Service Layer classes: ");
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding " + tableName + "Service class");
                ITable table = _context.Database.Tables[tableName];
                RenderConcreteClass(table);
            }
        }


        #endregion

        #region Private Methods

        private void RenderConcreteClass(ITable table)
        {
            

            _hdrUtil.WriteClassHeader(_output);

            try
            {
                GenTable genTable = new GenTable(table, _context);
                string sortColumn = genTable.GetFirstStringColumnName();

                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Collections.Generic;");
                _output.autoTabLn("using System.Linq;");
                _output.autoTabLn("using System.Text;");
                _output.autoTabLn("");
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
                _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces;");
                _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
                _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
                _output.autoTabLn("{");
                _output.autoTabLn("    public class " + StringFormatter.CleanUpClassName(table.Name) + "Service : " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase, I" + StringFormatter.CleanUpClassName(table.Name) + "Service");
                _output.autoTabLn("    {");
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("#region Dependency Injection");
                _output.tabLevel++;
                _output.autoTabLn("");
                _output.tabLevel--;
                _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + "Service(I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + "):base(" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ")");
                _output.autoTabLn("{");
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("#endregion");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("#region I" + StringFormatter.CleanUpClassName(table.Name) + "Service Members");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("if (string.IsNullOrEmpty(sortExpression))");
                _output.tabLevel++;
                _output.autoTabLn("sortExpression = \"" + sortColumn + "\";");
                _output.tabLevel--;
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("return " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetAll(sortExpression, startRowIndex, maximumRows);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public int GetCount()");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetAll().Count;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("public " + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " GetById(int Id)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetById(Id);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("#endregion");
                _output.tabLevel--;
                _output.autoTabLn("    }");
                _output.autoTabLn("}");

                _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Service.cs");
                SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace, StringFormatter.CleanUpClassName(table.Name) + "Service.cs"), SaveActions.DontOverwrite);
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering ServiceLayer Concrete class - " + ex.Message);
            }
        }

        private void RenderServiceBaseClass(ITable table)
        {
            

            _hdrUtil.WriteClassHeader(_output);

            try
            {
                _output.autoTabLn("using System.Collections.Generic;");
                _output.autoTabLn("using System.Linq;");
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
                _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces;");
                _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
                _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
                _output.autoTabLn("{");
                _output.autoTabLn("    public class " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase : ICRUDService<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">");
                _output.autoTabLn("    {");
                _output.tabLevel++;
                _output.autoTabLn("#region Dependency Injection");
                _output.autoTabLn("");
                _output.autoTabLn("protected I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase(I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ")");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("this." + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + " = " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("#endregion");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> GetAll()");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetAll().ToList();");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public void Insert(" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " model)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("EntLibValidation.Validator<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">.Validate(model);"); 
                _output.autoTabLn("" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Insert(model);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public void Update(" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " model)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("EntLibValidation.Validator<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">.Validate(model);");
                _output.autoTabLn("" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Update(model);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("public void Delete(" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " model)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("EntLibValidation.Validator<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">.Validate(model);");
                _output.autoTabLn("" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Delete(model);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("    }");
                _output.autoTabLn("}");

                _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase.cs");
                SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Generated", StringFormatter.CleanUpClassName(table.Name) + "ServiceBase.cs"), SaveActions.Overwrite);
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering ServiceBase class - " + ex.Message);
            }

        }

        private void RenderInterface(ITable table)
        {
            

            _hdrUtil.WriteClassHeader(_output);

            try
            {
                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Collections.Generic;");
                _output.autoTabLn("using System.Linq;");
                _output.autoTabLn("using System.Text;");
                _output.autoTabLn("");
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
                _output.autoTabLn("{");
                _output.autoTabLn("    public interface I" + StringFormatter.CleanUpClassName(table.Name) + "Service : ICRUDService<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">");
                _output.autoTabLn("    {");
                _output.tabLevel++;
                _output.autoTabLn(_context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " GetById(" + _context.Utility.RenderConcreteMethodParameters(table) + ");");
                _output.autoTabLn("List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows);");
                _output.autoTabLn("int GetCount();");
                _output.tabLevel--;
                _output.autoTabLn("    }");
                _output.autoTabLn("}");

                _context.FileList.Add("    I" + StringFormatter.CleanUpClassName(table.Name) + "Service.cs");
                SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Interfaces", "I" + StringFormatter.CleanUpClassName(table.Name) + "Service.cs"), SaveActions.DontOverwrite);
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering RenderServiceLayerInterface  - " + ex.Message);
            }
        }

        private void RenderServiceCRUDInterface()
        {
            

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Text;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
            _output.autoTabLn("{");
            _output.autoTabLn("    public interface ICRUDService<T>");
            _output.autoTabLn("    {");
            _output.autoTabLn("        List<T> GetAll();");
            _output.autoTabLn("        void Insert(T model);");
            _output.autoTabLn("        void Update(T model);");
            _output.autoTabLn("        void Delete(T model);");
            _output.autoTabLn("    }");
            _output.autoTabLn("}");

            _context.FileList.Add("    ICRUDService.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Interfaces", "ICRUDService.cs"), SaveActions.Overwrite);
        }

        #endregion
    }
}

