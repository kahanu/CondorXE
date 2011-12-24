using System;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace Wcf.Components
{
    /// <summary>
    /// An Example class that renders WCF DTO business objects
    /// with [DataContract] and [DataMember] attributes.
    /// </summary>
    public class WcfDto : RenderBase, IBusinessObjects
    {
        private RequestContext _context;

        public WcfDto(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog();

            _context.FileList.Add("");
            _context.FileList.Add("Generated WcfDto Objects");
            foreach (TableItem item in _context.ScriptSettings.Settings.Tables.Tables)
            {
                _context.Dialog.Display("Processing WcfDto Objects for '" + item.Name + "'");
                ITable table = _context.MyMeta.DefaultDatabase.Tables[item.Name];
                RenderClass(table);
            }
        }

        private void RenderClass(ITable table)
        {
            var output = _context.Zeus.Output;
            output.clear();

            try
            {
                _hdrUtil.WriteClassHeader(output);

                output.autoTabLn("using System;");
                output.autoTabLn("using System.Collections.Generic;");
                output.autoTabLn("using System.Runtime.Serialization;");
                output.autoTabLn("");
                output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".DataTransferObjects");
                output.autoTabLn("{");
                output.autoTabLn("    [DataContract(Name = \"" + StringFormatter.CleanUpClassName(table.Name) + "\", Namespace = \"" + _script.Settings.ServiceLayer.DataContract + "\")]");
                output.autoTabLn("    public class " + StringFormatter.CleanUpClassName(table.Name) + "Dto ");
                output.autoTabLn("    {");
                output.autoTabLn("        #region Properties");
                output.autoTabLn("");
                output.tabLevel++;
                
                RenderProperties(table);

                output.tabLevel--;
                output.autoTabLn("");
                output.autoTabLn("");
                output.autoTabLn("        #endregion");
                output.autoTabLn("    }");
                output.autoTabLn("}");

                _context.FileList.Add("   " + StringFormatter.CleanUpClassName(table.Name) + "Dto.cs");
                SaveOutput(CreateFullPath(@"BusinessObjects", StringFormatter.CleanUpClassName(table.Name) + "Dto.cs"), SaveActions.DontOverwrite);
                output.clear();
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        private void RenderProperties(ITable table)
        {
            Condor.Core.Property prop = null;
            foreach (IColumn c in table.Columns)
            {
                prop = new WcfPropertyRenderDto(c, _context);
                prop.Render();
            }

            WcfForeignKeyPropertiesDtos dtos = new WcfForeignKeyPropertiesDtos(table, _context);
            dtos.Render();
        }
    }
}
