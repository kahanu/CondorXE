using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.IoC
{
    public class NinjectMVC3ServiceReferenceIoCProvider : RenderBase, IIoCProvider
    {
        #region ctors

        private readonly IDatabase _database;
        private readonly RequestContext _context;

        public NinjectMVC3ServiceReferenceIoCProvider(RequestContext context):base(context.Zeus.Output)
        {
            this._database = context.Database;
            this._context = context;
        }

        #endregion

        #region IRenderObject Members

        /// <summary>
        /// This renders both the Ninject Repository and Service mappings.
        /// </summary>
        public void Render()
        {
            
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _output.tabLevel++;
                _output.tabLevel++;
                _output.tabLevel++;

                _output.autoTabLn("kernel.Bind<I" + StringFormatter.CleanUpClassName(table.Name) + "Service>().To<" + StringFormatter.CleanUpClassName(table.Name) + "Service>();");

                _output.tabLevel--;
                _output.tabLevel--;
                _output.tabLevel--;
            }
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _output.tabLevel++;
                _output.tabLevel++;
                _output.tabLevel++;

                _output.autoTabLn("kernel.Bind<I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ">().To<" + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ">();");

                _output.tabLevel--;
                _output.tabLevel--;
                _output.tabLevel--;
            }
            _output.tabLevel++;
            _output.tabLevel++;
            _output.tabLevel++;

            _output.autoTabLn("kernel.Bind<IAuthRepository>().To<AuthRepository>();");

            _output.tabLevel--;
            _output.tabLevel--;
            _output.tabLevel--;
        }

        #endregion
    }
}
