using System.Xml.Serialization;
using System;

namespace Condor.Core
{
    [Serializable]
    public class UI
    {
        public UI()
        {
        }

        [XmlAttribute()]
        public bool Use;
        public string UINamespace;
        public string UIWcfServiceNamespace;
        public string ClassName;

        // This could be deprecated
        public UIFramework UIFramework;
    }
}