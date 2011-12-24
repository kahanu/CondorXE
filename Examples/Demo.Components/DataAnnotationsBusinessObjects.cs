using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Core.PropertyObjects;
using MyMeta;

namespace Demo.Components
{
    /// <summary>
    /// This class renders business objects for ASP.NET MVC applications 
    /// with DataAnnotations attributes on the properties.
    /// </summary>
    public class DataAnnotationsBusinessObjects : RenderBase, IBusinessObjects
    {
        private RequestContext _context;

        public DataAnnotationsBusinessObjects(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog();

            _context.FileList.Add("");
            _context.FileList.Add("Generated DataAnnotations Business Objects");
            foreach (TableItem item in _context.ScriptSettings.Settings.Tables.Tables)
            {
                _context.Dialog.Display("Processing DataAnnotations Business Objects for '" + item.Name + "'");
                ITable table = _context.MyMeta.DefaultDatabase.Tables[item.Name];
                RenderDataAnnotationsBusinessObjectsClass(table);
            }
        }
  
        private void RenderDataAnnotationsBusinessObjectsClass(ITable table)
        {
            var output = _context.Zeus.Output;
            output.clear();

            try
            {
                _hdrUtil.WriteClassHeader(output);

                output.autoTabLn("using System;");
                output.autoTabLn("using System.Collections.Generic;");
                output.autoTabLn("using System.ComponentModel.DataAnnotations;");
                output.autoTabLn("using System.ComponentModel;");

                output.autoTabLn("");
                output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
                output.autoTabLn("{");
                output.tabLevel++;

                output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name));
                output.autoTabLn("{");
                output.tabLevel++;

                output.autoTabLn("#region Properties");
                RenderProperties(table);
                output.autoTabLn("#endregion");

                output.tabLevel--;
                output.autoTabLn("}");
                output.autoTabLn("");

                output.tabLevel--;
                output.autoTabLn("}");

                _context.FileList.Add("   " + StringFormatter.CleanUpClassName(table.Name) + ".cs");
                SaveOutput(CreateFullPath(@"BusinessObjects", StringFormatter.CleanUpClassName(table.Name) + ".cs"), SaveActions.DontOverwrite);
                output.clear();
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
                prop = new BusinessObjectsPropertyRenderDataAnnotations(c, _context);
                prop.Render();
            }
            BusinessObjectsPropertiesRenderForeignKey foreignKey = new BusinessObjectsPropertiesRenderForeignKey(table, _context);
            foreignKey.Render();
        }

        #endregion
    }
}
