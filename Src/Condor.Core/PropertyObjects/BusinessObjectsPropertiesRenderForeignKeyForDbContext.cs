using System;
using System.Linq;
using Condor.Core.Interfaces;
using MyMeta;
using Zeus;

namespace Condor.Core.PropertyObjects
{
    /// <summary>
    /// This class doesn't need to inherit from Property since it has a different signature.
    /// This class renders properties that are related entities to this entity.
    /// </summary>
    public class BusinessObjectsPropertiesRenderForeignKeyForDbContext 
    {
        private readonly ITable _table;
        private readonly RequestContext _context;
        private readonly ScriptSettings _script;
        private readonly IZeusOutput _output;

        public BusinessObjectsPropertiesRenderForeignKeyForDbContext(MyMeta.ITable table, RequestContext context)
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
                        _output.autoTabLn("public virtual ICollection<" + StringFormatter.CleanUpClassName(key.ForeignTable.Name) + "> " + StringFormatter.CleanUpClassName(key.ForeignTable.Name) + "List { get; set; }");
                }

                if (_script.Tables.Contains(key.PrimaryTable.Name))
                {
                    if (key.PrimaryTable.Name != tableName)
                        _output.autoTabLn("public virtual " + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + " " + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + " { get; set; }");
                }
            }
            _output.writeln("");
        }
    }
}
