using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Core.PropertyObjects;
using MyMeta;
using Zeus;

namespace GizmoBeach.Components.CodeWriters
{
    /// <summary>
    /// This is an experimental class.  It is not used anywhere.
    /// </summary>
    public class DataAnnotationsWriter : RenderBase, ICodeWriter
    {
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;
        
        private readonly ITable _table;

        public DataAnnotationsWriter(RequestContext context, ITable table) : base(context.Zeus.Output)
        {
            this._table = table;
            this._context = context;
            this._output = context.Zeus.Output;
        }

        public void Write()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.ComponentModel.DataAnnotations;");
            _output.autoTabLn("using System.ComponentModel;");
            _output.autoTabLn("using System.Web.Mvc;");

            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;

            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(_table.Name));
            _output.autoTabLn("{");
            _output.tabLevel++;

            _output.autoTabLn("#region Properties");
            RenderProperties(_table);
            _output.autoTabLn("#endregion");

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");

            _output.tabLevel--;
            _output.autoTabLn("}");
        }

        public string Read
        {
            get { return _output.text; }
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
    }
}
