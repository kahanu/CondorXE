using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus;

namespace CodeWriterTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ImplementationTests
    {
        public ImplementationTests()
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
        public void TestMethod1()
        {
            // Arrange
            RequestContext context = new RequestContext();
            context.Zeus = new TempZeusContext();
            ICodeWriter writer = new BusinessObjectsWriter(context);

            // Act
            writer.Write();

            // Assert
            Console.WriteLine(writer.Read.ToString());
        }
    }
  
    public class BusinessObjectsWriter : ICodeWriter
    {
        private readonly RequestContext _context;
        private IZeusOutput _output;

        public BusinessObjectsWriter(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
        }

        public void Write()
        {
            _output.autoTabLn("This is a test");
        }


        public IZeusOutput Read
        {
            get { return _output; }
        }
    }
}
