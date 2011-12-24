using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.IoC
{
    public class NinjectMVC3RepositoryReferenceIoCProvider : RenderBase, IIoCProvider
    {
        #region ctors

        private readonly IDatabase _database;
        private readonly RequestContext _context;

        public NinjectMVC3RepositoryReferenceIoCProvider(RequestContext context):base(context.Zeus.Output)
        {
            this._database = context.Database;
            this._context = context;
        }

        #endregion

        #region IRenderObject Members

        /// <summary>
        /// This simply creates a code-block that will print to the output screen
        /// in MyGeneration that contains the mapped code for the Ninject references.
        /// I will just copy this code and paste it into the Ninject class in my project.
        /// </summary>
        public void Render()
        {
   
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _output.tabLevel++;
                _output.tabLevel++;
                _output.tabLevel++;

                _output.autoTabLn("kernel.Bind<I" + StringFormatter.CleanUpClassName(table.Name) + "Repository>().To<" + StringFormatter.CleanUpClassName(table.Name) + "Repository>();");

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
