using System;
using System.Linq;
using System.Xml.Serialization;

namespace Condor.Core
{
    [Serializable]
    public class WebAppSettings
    {
        public WebAppSettings() { }
        public string Version;
        public string Namespace;

        [XmlElement("Tables")]
        public TableObject Tables;
        public ServiceLayer ServiceLayer;
        public BusinessObjects BusinessObjects;
        public Common Common;
        public DataOptions DataOptions;
        public UI UI;
        public IoC IoC;
        public DotNet DotNet;
        public Pluralizer Pluralizer;
    }

}