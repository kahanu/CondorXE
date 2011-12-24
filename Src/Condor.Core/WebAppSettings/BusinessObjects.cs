using System.Xml.Serialization;
using System;

namespace Condor.Core
{
    [Serializable]
    public class BusinessObjects
    {
        public BusinessObjects()
        {
        }

        [XmlAttribute()]
        public bool Use;

        public string Selected;
        public string BusinessObjectsNamespace;
        public string ClassName;
    }
}