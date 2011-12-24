using System;
using System.Linq;
using MyMeta;

namespace Condor.Core.Interfaces
{
    public interface IAutoMapperFramework
    {
        void BuildModelClass(ITable table, bool useWebService);
        void RenderAutoMapperExtensionClass(bool useWebService);
        void RenderAutoMapperConfiguration(bool useWebService);
        void RenderAutoMapperAppStart();
    }
}
