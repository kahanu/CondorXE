using System.Xml.Serialization;
using System;

namespace Condor.Core
{
    [Serializable]
    public class Common
    {
        public Common()
        {
        }

        [XmlAttribute()]
        public bool SaveToFile;
    }
}