using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.IoC
{
    public class UnityMVC4ServiceReferenceIoCProviderForDbContext : RenderBase, IIoCProvider
    {
        #region ctors

        public readonly RequestContext _context;
        public readonly IDatabase _database;

        public UnityMVC4ServiceReferenceIoCProviderForDbContext(RequestContext context):base(context.Zeus.Output)
        {
            this._context = context;
            this._database = _context.Database;
        }

        #endregion

        /// <summary>
        /// This renders the Unity mappings for the Services and Data Objects.
        /// </summary>
        public void Render()
        {
            _output.tabLevel++;
            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn("container.RegisterType<IUnitOfWork, UnitOfWork>();");
            _output.tabLevel--;
            _output.tabLevel--;
            _output.tabLevel--;

            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _output.tabLevel++;
                _output.tabLevel++;
                _output.tabLevel++;

                _output.autoTabLn("container.RegisterType<I" + StringFormatter.CleanUpClassName(table.Name) + "Service, " + StringFormatter.CleanUpClassName(table.Name) + "Service>();");

                _output.tabLevel--;
                _output.tabLevel--;
                _output.tabLevel--;
            }

            _output.autoTabLn("");

            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _output.tabLevel++;
                _output.tabLevel++;
                _output.tabLevel++;

                _output.autoTabLn("container.RegisterType<I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ", " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ">();");

                _output.tabLevel--;
                _output.tabLevel--;
                _output.tabLevel--;
            }
        }
    }
}
