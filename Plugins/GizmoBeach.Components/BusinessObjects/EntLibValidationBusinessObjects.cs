using System;
using MyMeta;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Core.PropertyObjects;

namespace GizmoBeach.Components.BusinessObjects
{
    public class EntLibValidationBusinessObjects : RenderBase, IBusinessObjects
    {
        private readonly RequestContext _context;

        public EntLibValidationBusinessObjects(RequestContext context)
        : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog();

            _context.FileList.Add("");
            _context.FileList.Add("Generated EntLibValidation Business Objects");
            foreach (TableItem item in _context.ScriptSettings.Settings.Tables.Tables)
            {
                _context.Dialog.Display("Processing EntLibValidation Business Objects for '" + item.Name + "'");
                ITable table = _context.MyMeta.DefaultDatabase.Tables[item.Name];
                RenderEntLibValidationBusinessObjectsClass(table);
            }
        }

        private void RenderEntLibValidationBusinessObjectsClass(ITable table)
        {
            

            try
            {
                _hdrUtil.WriteClassHeader(_output);

                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Collections.Generic;");
                _output.autoTabLn("using System.ComponentModel;");
                _output.autoTabLn("using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
                _output.autoTabLn("{");
                _output.tabLevel++;

                _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name));
                _output.autoTabLn("{");
                _output.tabLevel++;

                _output.autoTabLn("#region Properties");
                RenderProperties(table);
                _output.autoTabLn("#endregion");

                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");

                _output.tabLevel--;
                _output.autoTabLn("}");

                _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + ".cs");
                SaveOutput(CreateFullPath(_script.Settings.BusinessObjects.BusinessObjectsNamespace, StringFormatter.CleanUpClassName(table.Name) + ".cs"), SaveActions.DontOverwrite);
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RenderProperties(ITable table)
        {
            Condor.Core.Property prop = null;
            foreach (IColumn c in table.Columns)
            {
                prop = new BusinessObjectsPropertyRenderEntLibValidation(c, _context);
                prop.Render();
            }
            BusinessObjectsPropertiesRenderForeignKey foreignKey = new BusinessObjectsPropertiesRenderForeignKey(table, _context);
            foreignKey.Render();
        }
        #endregion
    }
}
