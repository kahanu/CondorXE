using System;
using System.Linq;

namespace Condor.Core
{
    public class EnumFactory
    {
        public static T Parse<T>(string typeName)
        {
            return (T)Enum.Parse(typeof(T), typeName);
        }
    }
}
