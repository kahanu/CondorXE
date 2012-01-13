using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using NUnit.Framework;
using Condor.Core;

namespace Condor.Tests
{
    [TestFixture]
    public class RegExTests
    {
        private string pattern = @"[_\/\.\s()-]";

        #region Raw Regex Tests
        [Test]
        public void regex_test_compress_string()
        {
            // Arrange
            string stringToSearch = "Patent / Application No. (first only)";

            // Act
            Regex regex = new Regex(pattern);
            string expected = "PatentApplicationNofirstonly";
            string actual = regex.Replace(stringToSearch, "");

            // 
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void regex_test_to_replace_with_underscore()
        {
            // Arrange
            string stringToSearch = "Patent / Application No. (first only)";

            // Act
            Regex regex = new Regex(pattern);
            string expected = "Patent___Application_No___first_only_";
            string actual = regex.Replace(stringToSearch, "_");

            // 
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void regex_test_with_string_that_has_underscore()
        {
            // Arrange
            string stringToSearch = "Patent / Application_No. (first only)";

            // Act
            Regex regex = new Regex(pattern);
            string expected = "Patent___Application_No___first_only_";
            string actual = regex.Replace(stringToSearch, "_");

            // 
            Assert.AreEqual(expected, actual);
        } 
        #endregion

        #region CommonUtility CleanUp Property Tests

        [Test]
        public void cleanup_property_returns_compressed_property()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string propertyToClean = "Patent / Application No. (first-class)";

            // Act
            string actual = util.CleanUpProperty(propertyToClean);
            string expected = "PatentApplicationNofirstclass";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void cleanup_property_returns_underscored_property()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string propertyToClean = "Patent / Application No. (first-class)";

            // Act
            string actual = util.CleanUpProperty(propertyToClean, false, PropertyModifications.Underscore);
            string expected = "Patent___Application_No___first_class_";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void cleanup_property_keeps_result_that_already_contains_underscore()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string propertyToClean = "Patent / Application_No. (first-class)";

            // Act
            string actual = util.CleanUpProperty(propertyToClean, false, PropertyModifications.Underscore);
            string expected = "Patent___Application_No___first_class_";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void cleanup_property_removes_underscore_in_original_property()
        {
            // Arrange
            CommonUtility util = new CommonUtility();
            string propertyToClean = "Patent / Application_No. (first-class)";

            // Act
            string actual = util.CleanUpProperty(propertyToClean);
            string expected = "PatentApplicationNofirstclass";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
