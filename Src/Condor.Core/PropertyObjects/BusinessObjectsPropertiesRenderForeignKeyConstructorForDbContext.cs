using System;
using System.Linq;
using MyMeta;
using Zeus;

namespace Condor.Core.PropertyObjects
{
    public class BusinessObjectsPropertiesRenderForeignKeyConstructorForDbContext
    {
        private readonly ITable _table;
        private readonly RequestContext _context;
        private readonly ScriptSettings _script;
        private readonly IZeusOutput _output;

        public BusinessObjectsPropertiesRenderForeignKeyConstructorForDbContext(MyMeta.ITable table, RequestContext context)
        {
            this._context = context;
            this._table = table;
            this._script = context.ScriptSettings;
            this._output = context.Zeus.Output;
        }

        public void Render()
        {
            string tableName = _table.Name;
            foreach (IForeignKey key in _table.ForeignKeys)
            {
                if (_script.Tables.Contains(key.ForeignTable.Name))
                {
                    if (key.PrimaryTable.Name == tableName)
                        _output.autoTabLn(StringFormatter.CleanUpClassName(key.ForeignTable.Name) + "List = new HashSet<" + StringFormatter.CleanUpClassName(key.ForeignTable.Name) + ">();");
                }
            }
        }
    }
}
