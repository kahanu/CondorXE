using System;
using System.Linq;
using System.Xml.Serialization;

namespace Condor.Core
{
    [Serializable]
    public class ServiceLayer
    {
        public ServiceLayer() { }

        [XmlAttribute()]
        public bool Use;

        public string ServiceNamespace;
        public string DataContract;
        public string Selected;
        public string ClassName;

        [XmlAttribute()]
        public bool SupportWCF;
    }
}
