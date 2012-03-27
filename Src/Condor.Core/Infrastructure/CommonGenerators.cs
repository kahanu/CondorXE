using System;
using System.Linq;
using MyMeta;

namespace Condor.Core.Infrastructure
{
    public class CommonGenerators : RenderBase, Condor.Core.Interfaces.ICommonGenerators
    {
        #region ctors
        private readonly RequestContext _context;

        /// <summary>
        /// This class renders commonly used data code such as:
        /// -- DaoCRUDInterface
        /// -- DaoInterfaces
        /// -- ServiceCRUDInterface
        /// -- ServiceInterfaces
        /// </summary>
        /// <param name="context"></param>
        public CommonGenerators(RequestContext context) : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #endregion

        #region Members

        public void RenderDaoCRUDInterface()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + "<T>");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region CRUD Methods");
            _output.autoTabLn("");
            _output.autoTabLn("List<T> GetAll();");
            _output.autoTabLn("void Insert(T model);");
            _output.autoTabLn("void Delete(T model);");
            _output.autoTabLn("void Update(T model);");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
        }

        public void RenderDaoInterface(ITable table)
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : ICRUD" + _script.Settings.DataOptions.ClassSuffix.Name + "<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " GetById(" + StringFormatter.CamelCasing(_context.Utility.RenderConcreteMethodParameters(table)) + ");");
            _output.autoTabLn("List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
        }

        public void RenderServiceCRUDInterface()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
            _output.autoTabLn("{");
            _output.autoTabLn("    public interface ICRUDService<T>");
            _output.autoTabLn("    {");
            _output.autoTabLn("        List<T> GetAll();");
            _output.autoTabLn("        void Insert(T model);");
            _output.autoTabLn("        void Update(T model);");
            _output.autoTabLn("        void Delete(T model);");
            _output.autoTabLn("    }");
            _output.autoTabLn("}");
        }

        public void RenderServiceInterface(ITable table)
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface I" + StringFormatter.CleanUpClassName(table.Name) + "Service : ICRUDService<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + ">");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(_context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + " GetById(" + _context.Utility.RenderConcreteMethodParameters(table) + ");");
            _output.autoTabLn("List<" + _context.Utility.BuildModelClassWithNameSpace(StringFormatter.CleanUpClassName(table.Name)) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows);");
            _output.autoTabLn("int GetCount();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
        }
        #endregion
    }
}
