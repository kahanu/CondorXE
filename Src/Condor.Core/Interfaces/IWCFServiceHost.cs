using System;
using System.Linq;

namespace Condor.Core.Interfaces
{
    public interface IWCFServiceHost
    {
        void RenderServiceHostFile();
        void RenderServiceHostWebConfig();
    }
}
