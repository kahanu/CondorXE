using System;
using System.Linq;
using Condor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Condor.UnitTests
{
    [TestClass]
    public class PropertyTests
    {
        [TestMethod]
        public void cleanupproperty_method_removes_underscores_with_underscore_modifier_enabled()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string input = "test_column";
            string expected = "test_column";

            // Act
            string actual = util.CleanUpProperty(input, false, PropertyModifications.Underscore);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void cleanupproperty_method_replaces_dot_with_underscore_with_underscore_modifier_enabled()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string input = "test.column";
            string expected = "test_column";

            // Act
            string actual = util.CleanUpProperty(input, false, PropertyModifications.Underscore);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void cleanupproperty_method_removes_dot_with_compress_modifier_enabled()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string input = "test.column";
            string expected = "testcolumn";

            // Act
            string actual = util.CleanUpProperty(input, false, PropertyModifications.Compress);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void cleanupproperty_method_keeps_underscores_with_none_modifier_enabled()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string input = "test_column";
            string expected = "test_column";

            // Act
            string actual = util.CleanUpProperty(input, false, PropertyModifications.None);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void cleanupproperty_method_returns_column_name_unchanged_with_no_parameters()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string input = "test_column";
            string expected = "test_column";

            // Act
            string actual = util.CleanUpProperty(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }


    }
}
