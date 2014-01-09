using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Condor.UnitTests
{
    [TestClass]
    public class PropertyListTests
    {
        [TestMethod]
        public void empty_omitlist()
        {
            // Arrange
            string[] fullPropertyList = new string[] { "Id", "FirstName", "LastName", "Phone", "rowversion" };
            string omitString = string.Empty;
            string[] omitList = omitString.ToLower().Split(',');

            // Act
            string actual = string.Empty;
            foreach (string item in fullPropertyList)
            {
                if (!omitList.Where(o => o == item.ToLower()).Any())
                {
                    actual += item.ToLower() + ",";
                }
            }

            string expected = "firstname,lastname,phone,";

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
