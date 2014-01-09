using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Condor.Core;
using NUnit.Framework;

namespace Condor.Tests
{
    [TestFixture]
    public class PropertyTests
    {

        [TestFixtureSetUp]
        public void FixtureSetup()
        {

        }

        [Test]
        public void cleanupproperty_method_removes_underscores_with_underscore_modifier_enabled()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string input = "test_column";
            string expected = "";

            // Act
            string actual = util.CleanUpProperty(input, false, PropertyModifications.Underscore);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
