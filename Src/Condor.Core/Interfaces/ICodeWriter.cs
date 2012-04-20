using System;
using System.Linq;

namespace Condor.Core.Interfaces
{
    public interface ICodeWriter
    {
        void Write();
        string Read { get; }
    }
}
