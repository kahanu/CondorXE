using System;
using System.Linq;
using GizmoBeach.Components.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMeta;

namespace GizmoBeach.Components.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class EntitySpacesMappingTests
    {
        public EntitySpacesMappingTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
        public void categoryname_column_in_table()
        {
            // Arrange
            IColumns columns = new CategoryColumns();
            ITable table = new CategoryTable(columns, null);

            // Act
            IColumn categoryColumn = (CategoryNameColumn)table.Columns[1];
            string name = categoryColumn.Name;

            // Assert
            Assert.AreEqual("CategoryName", name);
        }

        [TestMethod]
        public void loop_through_all_columns_in_CategoryTable()
        {
            // Arrange
            IColumns columns = new CategoryColumns();
            ITable table = new CategoryTable(columns, null);

            // Act
            foreach (IColumn column in table.Columns)
            {
                Console.WriteLine(column.Name + " - Type: " + column.LanguageType + " - IsPrimaryKey: " + column.IsInPrimaryKey + " - IsNullable: " + column.IsNullable);
            }
        }

        [TestMethod]
        public void create_mapper_property_mapping_for_identity_column()
        {
            // Arrange
            IColumns columns = new CategoryColumns();
            ITable table = new CategoryTable(columns, null);
            string expected = "model.Id = (int)entity.Id;";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.IsInPrimaryKey)
                {
                    actual = "model." + c.Name + " = (int)entity." + c.Name + ";";
                }
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_mapper_property_for_rowversion_column()
        {
            // Arrange
            IColumns columns = new CategoryColumns();
            ITable table = new CategoryTable(columns, null);
            string expected = "model.rowversion = entity.Rowversion.AsBase64String();";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.Name.ToLower() == "rowversion")
                {
                    actual = "model." + c.Name.ToLower() + " = entity." + c.Name + ".AsBase64String();";
                }
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_mapper_property_for_string_column()
        {
            IColumns columns = new CategoryColumns();
            ITable table = new CategoryTable(columns, null);
            string expected = "model.CategoryName = entity.CategoryName";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.LanguageType == "string")
                {
                    actual = "model." + c.Name + " = entity." + c.Name;
                }              
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_mapper_property_for_datetime_column()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new DateTimeNullableColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.DateCreated = entity.DateCreated";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.LanguageType.ToLower() == "datetime")
                {
                    actual = "model." + c.Name + " = entity." + c.Name;
                }
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_mapper_property_for_nullable_datetime_column()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new DateTimeNullableColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.DateCreated = entity.DateCreated.HasValue ? (DateTime)entity.DateCreated : default(DateTime)";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.LanguageType.ToLower() != "string")
                {
                    if (c.IsNullable)
                    {
                        actual = "model." + c.Name + " = entity." + c.Name + ".HasValue ? (DateTime)entity.DateCreated : default(DateTime)";
                    }
                }
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_mapper_property_for_not_nullable_datetime_column()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new DateTimeColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.DateCreated = entity.DateCreated;";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                actual = "model." + c.Name + " = entity." + c.Name;
                if (c.LanguageType.ToLower() != "string")
                {
                    if (c.IsNullable)
                    {
                        actual += ".HasValue ? (" + c.LanguageType + ")entity." + c.Name + " : default(" + c.LanguageType + ")";
                    }
                }
                actual += ";";
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_mapper_property_for_int_column()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new ViewCountIntColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.ViewCount = entity.ViewCount;";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                actual = "model." + c.Name + " = entity." + c.Name;
                if (c.LanguageType.ToLower() != "string")
                {
                    if (c.IsNullable)
                    {
                        actual += ".HasValue ? (" + c.LanguageType + ")entity." + c.Name + " : default(" + c.LanguageType + ")";
                    }
                }
                actual += ";";
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_mapper_property_for_nullable_int_column()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new ViewCountIntNullableColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.ViewCount = entity.ViewCount.HasValue ? (int)entity.ViewCount : default(int);";

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                actual = "model." + c.Name + " = entity." + c.Name;
                if (c.LanguageType.ToLower() != "string")
                {
                    if (c.IsNullable)
                    {
                        actual += ".HasValue ? (" + c.LanguageType + ")entity." + c.Name + " : default(" + c.LanguageType + ")";
                    }
                }
                actual += ";";
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_complete_mapper_for_category_table()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new DateTimeColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.Id = (int)entity.Id;" + Environment.NewLine;
            expected += "model.CategoryName = entity.CategoryName;" + Environment.NewLine;
            expected += "model.rowversion = entity.Rowversion.AsBase64String();" + Environment.NewLine;
            expected += "model.DateCreated = entity.DateCreated;" + Environment.NewLine;

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.IsInPrimaryKey)
                {
                    actual += "model." + c.Name + " = (int)entity." + c.Name + ";" + Environment.NewLine;
                }
                else if (c.Name.ToLower() == "rowversion")
                {
                    actual += "model." + c.Name.ToLower() + " = entity." + c.Name + ".AsBase64String();" + Environment.NewLine;
                }
                else
                {
                    actual += "model." + c.Name + " = entity." + c.Name;
                    if (c.LanguageType.ToLower() != "string")
                    {
                        if (c.IsNullable)
                        {
                            actual += ".HasValue ? (" + c.LanguageType + ")entity." + c.Name + " : default(" + c.LanguageType + ")";
                        }
                    }
                    actual += ";" + Environment.NewLine;
                }
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_complete_mapper_for_category_table_with_datetime_column()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new DateTimeColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.Id = (int)entity.Id;" + Environment.NewLine;
            expected += "model.CategoryName = entity.CategoryName;" + Environment.NewLine;
            expected += "model.rowversion = entity.Rowversion.AsBase64String();" + Environment.NewLine;
            expected += "model.DateCreated = entity.DateCreated;" + Environment.NewLine;

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.IsInPrimaryKey)
                {
                    actual += "model." + c.Name + " = (int)entity." + c.Name + ";" + Environment.NewLine;
                }
                else if (c.Name.ToLower() == "rowversion")
                {
                    actual += "model." + c.Name.ToLower() + " = entity." + c.Name + ".AsBase64String();" + Environment.NewLine;
                }
                else
                {
                    actual += "model." + c.Name + " = entity." + c.Name;
                    if (c.LanguageType.ToLower() != "string")
                    {
                        if (c.IsNullable)
                        {
                            actual += ".HasValue ? (" + c.LanguageType + ")entity." + c.Name + " : default(" + c.LanguageType + ")";
                        }
                    }
                    actual += ";" + Environment.NewLine;
                }
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void create_complete_mapper_for_category_table_with_nullable_datetime_column()
        {
            IColumns columns = new CategoryColumns();
            columns.Add(new DateTimeNullableColumn());

            ITable table = new CategoryTable(columns, null);
            string expected = "model.Id = (int)entity.Id;" + Environment.NewLine;
            expected += "model.CategoryName = entity.CategoryName;" + Environment.NewLine;
            expected += "model.rowversion = entity.Rowversion.AsBase64String();" + Environment.NewLine;
            expected += "model.DateCreated = entity.DateCreated.HasValue ? (DateTime)entity.DateCreated : default(DateTime);" + Environment.NewLine;

            // Act
            string actual = string.Empty;
            foreach (IColumn c in table.Columns)
            {
                if (c.IsInPrimaryKey)
                {
                    actual += "model." + c.Name + " = (int)entity." + c.Name + ";" + Environment.NewLine;
                }
                else if (c.Name.ToLower() == "rowversion")
                {
                    actual += "model." + c.Name.ToLower() + " = entity." + c.Name + ".AsBase64String();" + Environment.NewLine;
                }
                else
                {
                    actual += "model." + c.Name + " = entity." + c.Name;
                    if (c.LanguageType.ToLower() != "string")
                    {
                        if (c.IsNullable)
                        {
                            actual += ".HasValue ? (" + c.LanguageType + ")entity." + c.Name + " : default(" + c.LanguageType + ")";
                        }
                    }
                    actual += ";" + Environment.NewLine;
                }
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
