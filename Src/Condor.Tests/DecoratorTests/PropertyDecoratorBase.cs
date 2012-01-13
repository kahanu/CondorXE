using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Condor.Core;

namespace Condor.Tests.DecoratorTests
{
    public abstract class PropertyDecoratorBase : Property
    {
        public PropertyDecoratorBase(MyMeta.IColumn column, RequestContext context)
            : base(column, context)
        {

        }
    }
}
