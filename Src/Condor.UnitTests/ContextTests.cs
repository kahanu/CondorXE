using System;
using System.Linq;
using Condor.Core;
using Condor.Core.PropertyObjects;
using Condor.Fakes;
using Condor.UnitTests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMeta;

namespace Condor.UnitTests
{
    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            ITable table = null;
            IColumns columns = new CategoryColumns();
            columns.Add(new IdentityColumn(table));
            columns.Add(new DateTimeColumn(table));
            columns.Add(new CategoryNameColumn(table));

            table = new CategoryTable(columns, null);

            RequestContext context = new RequestContext();
            context.Zeus = new TempZeusContext();
            //context.ScriptSettings = new ScriptSettings(null, null, null, null);

            Condor.Core.Property prop = null;
            foreach (IColumn c in table.Columns)
            {
                prop = new BusinessObjectsPropertyRenderDataAnnotationsForDbContext(c, context);
                prop.Render();
            }

        }
    }
}
