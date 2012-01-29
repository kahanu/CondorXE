using System;
using System.Linq;
using NUnit.Framework;
using Condor.Core;

namespace Condor.Tests
{
    [TestFixture]
    public class CoreTests
    {


        [Test]
        public void pluralizer_factory_test()
        {
            // Arrange
            string tableName = "aspnet_Applications";
            string result = tableName;

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>("Unchanged");

            PluralizerFactory factory = new PluralizerFactory();

            // Act
            result = factory.SetWord(tableName, classType);

            // Assert
            Assert.AreEqual(tableName, result);
        }

        [Test]
        public void pluralizer_factory_isentityset_returns_correct_result_for_input_with_underscore()
        {
            // Arrange
            string tableName = "aspnet_Applications";
            string result = tableName;
            bool isEntitySet = true;

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>("Unchanged");

            PluralizerFactory factory = new PluralizerFactory();

            // Act
            result = factory.SetWord(tableName, classType, isEntitySet);

            // Assert
            Assert.AreEqual(tableName, result);
        }

        [Test]
        public void pluralizer_factory_isentityset_returns_plural_entitysetname_for_non_underscore_class_name()
        {
            // Arrange
            string tableName = "Project";
            string result = tableName;
            bool isEntitySet = true;

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>("Unchanged");

            PluralizerFactory factory = new PluralizerFactory();

            // Act
            result = factory.SetWord(tableName, classType, isEntitySet);
            string expected = "Projects";

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void pluralizer_factory_returns_unchanged_class_name_for_non_entity_type()
        {
            // Arrange
            string tableName = "Customer";
            string result = tableName;

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>("Unchanged");

            PluralizerFactory factory = new PluralizerFactory();

            // Act
            result = factory.SetWord(tableName, classType);

            // Assert
            Assert.AreEqual(tableName, result);
        }

        [Test]
        public void pluralizer_factory_returns_plural_result_for_singular_class_name_based_on_plural_setting()
        {
            // Arrange
            string tableName = "Customer";
            string result = tableName;

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>("Plural");

            PluralizerFactory factory = new PluralizerFactory();

            // Act
            result = factory.SetWord(tableName, classType);
            string expected = "Customers";

            // Assert
            Assert.AreEqual(expected, result);
        }


    }
}
