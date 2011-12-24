using System;
using System.Linq;

namespace Condor.Core
{
    [Serializable]
    public class Pluralizer
    {
        public Pluralizer() { }

        public ClassNames ClassNames;
        public FieldNames FieldNames;
        public PropertyNames PropertyNames;
    }
}
