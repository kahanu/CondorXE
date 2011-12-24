using System.Xml.Serialization;
using System;

namespace Condor.Core
{
    [Serializable]
    public class IoC
    {
        public IoC()
        {
        }

        [XmlAttribute()]
        public bool Use;
        public string ClassName;

        /// <summary>
        /// Deprecated - use the ClassName instead
        /// </summary>
        public IoCProvider IoCProvider;
    }
}