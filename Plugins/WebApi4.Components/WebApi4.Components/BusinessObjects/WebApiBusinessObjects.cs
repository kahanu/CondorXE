using System;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Core.PropertyObjects;
using MyMeta;

namespace WebApi4.Components.BusinessObjects
{
    public class WebApiBusinessObjects : RenderBase, IBusinessObjects
    {
        private readonly RequestContext _context;

        public WebApiBusinessObjects(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog();

            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi4 Business Objects");
            foreach (TableItem item in _context.ScriptSettings.Settings.Tables.Tables)
            {
                _context.Dialog.Display("Processing WebApi4 Business Objects for '" + item.Name + "'");
                ITable table = _context.Database.Tables[item.Name];
                RenderWebApiBusinessObjectsClass(table);
            }

            RenderEntityBaseClass();
        }
  
        private void RenderEntityBaseClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.ComponentModel.DataAnnotations;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public abstract class Entity");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("[Key]");
            _output.autoTabLn("public int Id { get; set; }");
            _output.autoTabLn("//public string rowversion { get; set; }");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("   Entity.cs");
            SaveOutput(CreateFullPath(_script.Settings.BusinessObjects.BusinessObjectsNamespace, "_Entity.cs"), SaveActions.DontOverwrite);
        }

        private void RenderWebApiBusinessObjectsClass(ITable table)
        {
            try
            {
                _hdrUtil.WriteClassHeader(_output);

                /* Include any necessary using references here for this generated code. */
                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Collections.Generic;");
                _output.autoTabLn("using System.ComponentModel;");
                _output.autoTabLn("using System.ComponentModel.DataAnnotations;");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
                _output.autoTabLn("{");
                _output.tabLevel++;

                _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + ": Entity");
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

                _context.FileList.Add("   " + StringFormatter.CleanUpClassName(table.Name) + ".cs");
                SaveOutput(CreateFullPath(_script.Settings.BusinessObjects.BusinessObjectsNamespace, StringFormatter.CleanUpClassName(table.Name) + ".cs"), SaveActions.DontOverwrite);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RenderProperties(ITable table)
        {
            BusinessObjectsPropertyRenderDataAnnotationsForDbContext property = null;

            foreach (IColumn c in table.Columns)
            {
                property = new BusinessObjectsPropertyRenderDataAnnotationsForDbContext(c, _context, "Id");
                property.Render();
            }

            BusinessObjectsPropertiesRenderForeignKeyForDbContext foreignKey = new BusinessObjectsPropertiesRenderForeignKeyForDbContext(table, _context);
            foreignKey.Render();
        }

        #endregion
    }
}
