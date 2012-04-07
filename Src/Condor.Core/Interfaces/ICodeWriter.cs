using System;
using System.Linq;
using Zeus;

namespace Condor.Core.Interfaces
{
    public interface ICodeWriter
    {
        void Write();
        IZeusOutput Read { get; }
    }
}
