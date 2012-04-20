using System;
using System.Linq;
using System.Text;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus;

namespace GizmoBeach.Components.Tests
{
    [TestClass]
    public class ORMFrameworkCompositionTests
    {
        //private readonly IZeusContext _context;
        private readonly IZeusOutput _output;
        private readonly RequestContext _context;

        public ORMFrameworkCompositionTests()
        {
            _context = new RequestContext();
            _context.Zeus = new TempZeusContext();
            //_context.ScriptSettings = new GizmoBeach.Components.Tests.Context.ScriptSettings();
            _output = _context.Zeus.Output;
        }

        [TestMethod]
        public void write_the_standard_class_wrapper()
        {
            // Arrange 
            IORMFramework orm = new StandardORMFramework(_context);

            // Act
            orm.Generate();
            StringBuilder expected = new StringBuilder();
            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class BlogBaseDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");
            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class SQLServerBlogDao : BlogBaseDao, IBlogDao");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");
            expected.AppendLine("namespace BlogApp.DataObjects.Interfaces");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic interface IBlogDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            Console.WriteLine(_output.text);

            // Assert
            Assert.AreEqual(expected.ToString(), _output.text);
        }

        [TestMethod]
        public void standard_orm_class_with_entityframework_usings()
        {
            // Arrange
            IORMUsings usings = new EntityFrameworkBaseORMUsings(_context);
            IORMUsings concreteUsings = new EntityFrameworkConcreteORMUsings(_context);

            IORMFramework orm = new DerivedWithUsingsFromStandardORMFramework(_context, usings, concreteUsings, null);

            // Act
            orm.Generate();
            StringBuilder expected = new StringBuilder();
            expected.AppendLine("using System;");
            expected.AppendLine("using System.Collections.Generic;");
            expected.AppendLine("using System.Data;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine("");
            expected.AppendLine("using BlogApp.DataObjects.Interfaces;");
            expected.AppendLine("using BlogApp.DataObjects.EntityFramework.EntityMapper;");
            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class BlogBaseDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");
            expected.AppendLine("using System;");
            expected.AppendLine("using System.Collections.Generic;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine("using System.Linq.Dynamic;");
            expected.AppendLine("using BlogApp.DataObjects.Interfaces;");
            expected.AppendLine("using BlogApp.DataObjects.EntityFramework.EntityMapper;");
            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class SQLServerBlogDao : BlogBaseDao, IBlogDao");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");
            expected.AppendLine("namespace BlogApp.DataObjects.Interfaces");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic interface IBlogDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            // Assert
            Assert.AreEqual(expected.ToString(), _output.text);
        }

        [TestMethod]
        public void standard_orm_class_with_entityspaces_usings()
        {
            // Arrange
            IORMUsings usings = new EntitySpacesBaseORMUsings(_context);
            IORMUsings concreteUsings = null;

            IORMFramework orm = new DerivedWithUsingsFromStandardORMFramework(_context, usings, concreteUsings, null);

            // Act
            orm.Generate();
            StringBuilder expected = new StringBuilder();
            expected.AppendLine("using System;");
            expected.AppendLine("using System.Collections.Generic;");
            expected.AppendLine("using System.Data;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine("");
            expected.AppendLine("using BlogApp.DataObjects.EntitySpaces");
            expected.AppendLine("using BlogApp.DataObjects.Interfaces;");
            expected.AppendLine("using BlogApp.DataObjects.EntitySpaces.EntityMapper;");
            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class BlogBaseDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class SQLServerBlogDao : BlogBaseDao, IBlogDao");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            expected.AppendLine("namespace BlogApp.DataObjects.Interfaces");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic interface IBlogDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            // Assert
            Assert.AreEqual(expected.ToString(), _output.text);
        }

        [TestMethod]
        public void full_test_with_concrete_entityframework_class_and_usings()
        {
            // Arrange
            IORMFramework orm = new EntityFrameworkWrapperORMFramework(_context);

            // Act
            orm.Generate();
            StringBuilder expected = new StringBuilder();
            expected.AppendLine("using System;");
            expected.AppendLine("using System.Collections.Generic;");
            expected.AppendLine("using System.Data;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine("");
            expected.AppendLine("using BlogApp.DataObjects.Interfaces;");
            expected.AppendLine("using BlogApp.DataObjects.EntityFramework.EntityMapper;");
            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class BlogBaseDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            expected.AppendLine("using System;");
            expected.AppendLine("using System.Collections.Generic;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine("using System.Linq.Dynamic;");
            expected.AppendLine("using BlogApp.DataObjects.Interfaces;");
            expected.AppendLine("using BlogApp.DataObjects.EntityFramework.EntityMapper;");
            expected.AppendLine("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic class SQLServerBlogDao : BlogBaseDao, IBlogDao");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            expected.AppendLine("using System;");
            expected.AppendLine("using System.Collections.Generic;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine("namespace BlogApp.DataObjects.Interfaces");
            expected.AppendLine("{");
            expected.AppendLine("\tpublic interface IBlogDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            expected.AppendLine("\t{");
            expected.AppendLine("\t");
            expected.AppendLine("\t}");
            expected.AppendLine("}");

            // Assert
            Assert.AreEqual(expected.ToString(), _output.text);
        }
    }

    #region Helper Classes

    /// <summary>
    /// This is a sample, actual implementation as it would be created and 
    /// executed via the DataObjectsFactory class.
    /// </summary>
    class EntityFrameworkWrapperORMFramework : IORMFramework
    {
        private readonly RequestContext _context;

        public EntityFrameworkWrapperORMFramework(RequestContext context)
        {
            this._context = context;
        }

        public void Generate()
        {
            // Create the appropriate usings for the EntityFramework;
            IORMUsings baseUsings = new EntityFrameworkBaseORMUsings(_context);
            IORMUsings concreteUsings = new EntityFrameworkConcreteORMUsings(_context);
            IORMUsings interfaceUsings = new EntityFrameworkInterfaceORMUsings(_context);

            // Instantiate and new derived class from the StandardORMFramework class 
            // that accepts "usings".
            StandardORMFramework orm = new DerivedWithUsingsFromStandardORMFramework(_context, baseUsings, concreteUsings, interfaceUsings);
            orm.Generate();
        }

        public string Name
        {
            get { return "Entity Framework"; }
        }
    }


    /// <summary>
    /// This is a derived class from the StandardORMFramework class
    /// that accepts an IORMUsings implemented class that contains the 
    /// usings for the specific ORM Framework which could be different for each.
    /// </summary>
    class DerivedWithUsingsFromStandardORMFramework : StandardORMFramework
    {
        public DerivedWithUsingsFromStandardORMFramework(RequestContext context, IORMUsings baseUsings, IORMUsings concreteUsings, IORMUsings interfaceUsings)
            : base(context, baseUsings, concreteUsings, interfaceUsings)
        {

        }
    }

    /// <summary>
    /// This class contains just the usings for the EntityFramework ORM Framework.
    /// </summary>
    class EntityFrameworkBaseORMUsings : IORMUsings
    {
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;
        private readonly ScriptSettings _script;

        public EntityFrameworkBaseORMUsings(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
            this._script = context.ScriptSettings;
        }

        public void Render()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Data;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + "." + _script.Settings.DataOptions.ORMFramework.Selected + ".EntityMapper;");
        }
    }

    class EntityFrameworkConcreteORMUsings : IORMUsings
    {
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;

        public EntityFrameworkConcreteORMUsings(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
        }

        public void Render()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Linq.Dynamic;");
            _output.autoTabLn("using BlogApp.DataObjects.Interfaces;");
            _output.autoTabLn("using BlogApp.DataObjects.EntityFramework.EntityMapper;");
        }
    }

    class EntityFrameworkInterfaceORMUsings : IORMUsings
    {
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;

        public EntityFrameworkInterfaceORMUsings(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
        }
        public void Render()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
        }
    }

    /// <summary>
    /// This class contains just the usings for the EntitySpaces ORM Framework.
    /// </summary>
    class EntitySpacesBaseORMUsings : IORMUsings
    {
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;

        public EntitySpacesBaseORMUsings(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
        }
        public void Render()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Data;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("using BlogApp.DataObjects.EntitySpaces");
            _output.autoTabLn("using BlogApp.DataObjects.Interfaces;");
            _output.autoTabLn("using BlogApp.DataObjects.EntitySpaces.EntityMapper;");
        }
    }


    /// <summary>
    /// This acts as the base class for the ORMFramework to build the common
    /// class wrapper code.
    /// </summary>
    class StandardORMFramework : IORMFramework
    {
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;
        private readonly IORMUsings _baseUsings;
        private readonly IORMUsings _concreteUsings;
        private readonly IORMUsings _interfaceUsings;

        public StandardORMFramework(RequestContext context)
            : this(context, null, null, null)
        {
        }

        public StandardORMFramework(RequestContext context, IORMUsings baseUsings, IORMUsings concreteUsings,
            IORMUsings interfaceUsings)
        {
            this._interfaceUsings = interfaceUsings;
            this._concreteUsings = concreteUsings;
            this._context = context;
            this._output = context.Zeus.Output;
            this._baseUsings = baseUsings;
        }

        public string Name
        {
            get { return "Entity Framework"; }
        }

        public void Generate()
        {
            CreateBaseClass();
            CreateConcreteClass();
            CreateDaoInterface();
        }

        private void CreateBaseClass()
        {
            if (_baseUsings != null)
            {
                _baseUsings.Render();
            }
            _output.autoTabLn("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class BlogBaseDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
        }

        private void CreateConcreteClass()
        {
            if (_concreteUsings != null)
                _concreteUsings.Render();

            _output.autoTabLn("namespace BlogApp.DataObjects.EntityFramework.SQLServer");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class SQLServerBlogDao : BlogBaseDao, IBlogDao");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
        }

        private void CreateDaoInterface()
        {
            if (_interfaceUsings != null)
                _interfaceUsings.Render();

            _output.autoTabLn("namespace BlogApp.DataObjects.Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface IBlogDao : ICRUDDao<BlogApp.BusinessObjects.Blog>");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
        }
    }

    #endregion
}
