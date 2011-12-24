using System;
using System.Linq;
using Condor.Core;
using Condor.Core.PropertyObjects;
using Condor.Tests.Context;
using NUnit.Framework;

namespace Condor.Tests.PropertyDispenserTests
{
    [TestFixture]
    public class PropertyImplementationTests
    {
        private ScriptSettings _script;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            TempZeusInput input = new TempZeusInput();
            input["defaultOutputPath"] = @"C:\VSProjects\Condor\Generated\";
            _script = ScriptSettings.InitInstance(input, null, null, "3");
        }

        [Test]
        public void short_property_class_returns_proper_result()
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

            Condor.Core.Property prop = new BusinessObjectsPropertyRenderShortProperty(c, context);

            // Act
            prop.Render();
            var actual = context.Zeus.Output.text;
            var expected = "public string FirstName { get; set; }\r\n";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void get_the_tostring_value_from_the_shortproperty()
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

            Condor.Core.Property prop = new BusinessObjectsPropertyRenderShortProperty(c, context);

            // Act
            var actual = prop.ToString();
            var expected = "public string FirstName { get; set; }";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void render_data_annotations_property()
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

            Condor.Core.Property prop = new BusinessObjectsPropertyRenderDataAnnotations(c, context);

            // Act
            prop.Render();
            var expected = "[Required(ErrorMessage = \"First Name is required.\")]\r\n";
            expected += "[StringLength(50, ErrorMessage=\"First Name must be between 1 and 50 characters.\")]\r\n";
            expected += "[DisplayName(\"First Name\")]\r\npublic string FirstName { get; set; }\r\n\r\n";

            // Assert
            Assert.AreEqual(expected, context.Zeus.Output.text);
        }
    }

    #region Helper Classes
    #endregion
}
