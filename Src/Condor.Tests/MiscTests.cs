using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Tests.Context;
using NUnit.Framework;

namespace Condor.Tests
{
    [TestFixture]
    public class MiscTests
    {
        private ScriptSettings _script;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            //TempZeusInput input = new TempZeusInput();
            //input["defaultOutputPath"] = @"C:\VSProjects\Condor\Generated\";
            //_script = ScriptSettings.InitInstance(input, null, null, "3");
        }

        [Test]
        public void test_orms()
        {
            // Arrange
            IORMFramework derived = new DerivedORMFramework();

            // Act
            var actual = derived.Name;
            var expected = "Entity Framework";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Chop_provider_test()
        {
            // Arrange
            string fullConnString = @"Provider=SQLOLEDB.1;Data Source=kingwilder-pc\mssqlserver2008;Initial Catalog=BlogApp;Integrated Security=SSPI;";

            // Act
            int providerPos = fullConnString.IndexOf("Provider=");
            string connString = "";

            if (providerPos == 0)
            {
                int semiColon = fullConnString.IndexOf(';', 0);
                connString = fullConnString.Substring((semiColon+1), (fullConnString.Length - (semiColon+1)));
            }

            string expected = @"Data Source=kingwilder-pc\mssqlserver2008;Initial Catalog=BlogApp;Integrated Security=SSPI;";

            // Assert
            Assert.AreEqual(expected, connString);
        }

        [Test]
        public void string_array()
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



    }

    abstract class TempOrmFrameworkBase : IORMFramework
    {

        #region IORMFramework Members

        //public string Name
        //{
        //    get { return "Linq-To-Sql"; }
        //}

        public abstract string Name { get; }

        public void Generate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    class DerivedORMFramework : TempOrmFrameworkBase
    {
        public override string Name
        {
            get { return "Entity Framework"; }
        }
    }

}
