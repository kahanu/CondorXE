using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Condor.UnitTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //private TestContext testContextInstance;

        ///// <summary>
        /////Gets or sets the test context which provides
        /////information about and functionality for the current test run.
        /////</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void test_string_split()
        {
            // Arrange
            string originalString = "Id,rowversion";
            string[] omitList = originalString.ToLower().Split(',');

            // Act
            bool actual = omitList.Contains("id");
            bool expected = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void validate_alias_exists_in_array_equals_true()
        {
            // Arrange
            string originalString = "Id,rowversion";
            string[] omitList = originalString.ToLower().Split(',');
            string alias = "id";

            // Act
            bool actual = omitList.Where(o => o == alias).Any();
            bool expected = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void validate_alias_does_not_exist_in_array_equals_true()
        {
            // Arrange
            string originalString = "Id,rowversion";
            string[] omitList = originalString.ToLower().Split(',');
            string alias = "phone";

            // Act
            bool actual = omitList.Where(o => o == alias).Any();
            bool expected = false;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void iterate_over_property_array_and_validate_only_non_omitted_properties_are_created()
        {
            // Arrange
            string[] fullPropertyList = new string[] { "Id", "FirstName", "LastName", "Phone", "rowversion" };
            string omitString = "Id,rowversion";
            string[] omitList = omitString.ToLower().Split(',');

            // Act
            string actual = string.Empty;
            string actualOmitted = string.Empty;

            foreach (var item in fullPropertyList)
            {
                if (!omitList.Where(o => o == item.ToLower()).Any())
                {
                    actual += item.ToLower() + ",";
                }
                else
                {
                    actualOmitted += item.ToLower() + ",";
                }
            }

            actual += " - " + actualOmitted;

            string expected = "firstname,lastname,phone, - id,rowversion,";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void succeeds_with_empty_omitlist()
        {
            // Arrange
            string[] fullPropertyList = new string[] { "Id", "FirstName", "LastName", "Phone", "rowversion" };
            string omitString = null;
            string[] omitList;

            if (!string.IsNullOrEmpty(omitString))
            {
                omitList = omitString.ToLower().Split(',');
            }
            else
            {
                omitList = new string[0];
            }
            

            // Act
            string actual = string.Empty;
            foreach (string item in fullPropertyList)
            {
                if (!omitList.Where(o => o == item.ToLower()).Any())
                {
                    actual += item.ToLower() + ",";
                }
            }

            string expected = "id,firstname,lastname,phone,rowversion,";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_with_amateur_golf_articles_list_table()
        {
            // Arrange
            string[] fullPropertyList = new string[] { "typeID", "type_name", "type_sort", "type_desc" };
            string omitString = null;
            string[] omitList;

            if (!string.IsNullOrEmpty(omitString))
            {
                omitList = omitString.ToLower().Split(',');
            }
            else
            {
                omitList = new string[0];
            }

            // Act
            string actual = string.Empty;
            foreach (string item in fullPropertyList)
            {
                if (!omitList.Where(o => o == item.ToLower()).Any())
                {
                    actual += item.ToLower() + ",";
                }
            }

            string expected = "typeid,type_name,type_sort,type_desc,";

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
