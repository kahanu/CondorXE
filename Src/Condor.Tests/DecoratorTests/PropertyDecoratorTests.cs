using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Condor.Tests.Context;
using Condor.Core;

namespace Condor.Tests
{
    [TestFixture]
    public class PropertyDecoratorTests
    {
        private ScriptSettings _script;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            TempZeusInput input = new TempZeusInput();
            input["defaultOutputPath"] = @"C:\VSProjects\BlogApp8\";

            _script = ScriptSettings.InitInstance(input, new MyMeta.dbRoot(), new Dnp.Utils.Utils(), "3");
        }

        [Test]
        public void create_simple_property()
        {
            // Arrange
            MyMeta.ITable table = new TempTable()
            {
                Alias = "Customer"
            };

            MyMeta.IColumn c = new StringColumn(table)
            {
                Alias = "FirstName"
            };

            RequestContext context = new RequestContext();
            context.ScriptSettings = _script;
            context.Zeus = new TempZeusContext();
            context.Utility = new CommonUtility();

            // Act
            Condor.Core.Property prop = new RenderDataAnnotationsProperty(c, context);
            prop.Render();
            string expected = "public string FirstName { get; set; }\r\n";

            // Assert
            Assert.AreEqual(expected, context.Zeus.Output.text);
        }
    }

    class RenderDataAnnotationsProperty : Property
    {
        public RenderDataAnnotationsProperty(MyMeta.IColumn column, RequestContext context)
            : base(column, context)
        {

        }

        public override void Render()
        {
            _output.autoTabLn("public " + LanguageType + " " + ToPropertyName() + " { get; set; }");
        }
    }
}
