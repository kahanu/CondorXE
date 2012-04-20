using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Fakes;
using GizmoBeach.Components.CodeWriters;
using GizmoBeach.Components.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMeta;

namespace GizmoBeach.Components.Tests
{
    [TestClass]
    public class BusinessObjectsTests
    {
        [TestMethod]
        public void dataannotationswriter_test()
        {
            // Arrange
            IColumns columns = new CategoryColumns();
            columns.Add(new IdentityColumn());
            columns.Add(new DateTimeColumn());
            columns.Add(new CategoryNameColumn());
            columns.Add(new RowversionColumn());

            ITable table = new CategoryTable(columns, null);

            RequestContext context = new RequestContext();
            context.Zeus = new TempZeusContext();

            // Act
            ICodeWriter writer = new DataAnnotationsWriter(context, table);
            writer.Write();
            string actual = writer.Read;

            Console.WriteLine(actual);
        }
    }
}
