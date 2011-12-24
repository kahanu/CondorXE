using System;
using System.Linq;

namespace Condor.Core.Interfaces
{
    public interface IORMFramework
    {
        string Name { get; }

        void Generate();
    }
}
