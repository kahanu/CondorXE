using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Condor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Condor.UnitTests
{
    [TestClass]
    public class PluralizerFactoryTests
    {
        [TestMethod]
        public void table_name_with_underscore_is_correctly_kept_using_unchanged_pluralizerfactory()
        {
            // Arrange
            string tableName = "aspnet_Applications";
            string actual = "";
            string expected = "aspnet_Applications";

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>("Unchanged");

            PluralizerFactory factory = new PluralizerFactory();

            // Act
            actual = factory.SetWord(tableName, classType);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void plural_table_name_is_replaced_with_singular_table_name_using_singular_type()
        {
            // Arrange
            string tableName = "aspnet_Applications";
            string actual = "";
            string expected = "aspnet_Application";

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>("Singular");

            PluralizerFactory factory = new PluralizerFactory();

            // Act
            actual = factory.SetWord(tableName, classType);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
