using System.Xml.Serialization;
using System;

namespace Condor.Core
{
    [Serializable]
    public class ClassSuffix
    {
        public string Name;

        [XmlAttribute()]
        public bool IsDefault;
    }
}