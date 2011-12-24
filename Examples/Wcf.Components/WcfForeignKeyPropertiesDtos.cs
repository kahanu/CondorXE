using System;
using System.Linq;
using Condor.Core;
using MyMeta;
using Zeus;

namespace Wcf.Components
{
    public class WcfForeignKeyPropertiesDtos
    {
        private readonly ITable _table;
        private readonly RequestContext _context;
        private ScriptSettings _script;
        private IZeusOutput output;

        public WcfForeignKeyPropertiesDtos(MyMeta.ITable table, RequestContext context)
        {
            this._context = context;
            this._table = table;
            this._script = context.ScriptSettings;
            this.output = context.Zeus.Output;
        }
        public void Render()
        {
            string tableName = _table.Name;
            foreach (IForeignKey key in _table.ForeignKeys)
            {
                output.autoTabLn("[DataMember]");
                if (_script.Tables.Contains(key.ForeignTable.Name))
                {
                    if (key.PrimaryTable.Name == tableName)
                        output.autoTabLn("public " + StringFormatter.CleanUpClassName(key.ForeignTable.Name) + "Dto[] " + StringFormatter.CleanUpClassName(key.ForeignTable.Name) + "List { get; set; }");
                }

                if (_script.Tables.Contains(key.PrimaryTable.Name))
                {
                    if (key.PrimaryTable.Name != tableName)
                        output.autoTabLn("public " + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + "Dto " + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + " { get; set; }");
                }
            }
            output.writeln("");
        }
    }
}
