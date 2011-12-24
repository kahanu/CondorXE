using System;
using System.Linq;

namespace Condor.ORM
{
    public interface IValidator
    {
        bool IsValid();
        string Message { get; set; }
    }	
}
