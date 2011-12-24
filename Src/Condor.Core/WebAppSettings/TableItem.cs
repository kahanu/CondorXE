using System.Xml.Serialization;
using System;

namespace Condor.Core
{
    [Serializable]
    public class TableItem
    {
        public TableItem()
        {
        }

        public TableItem(string name)
        {
            _name = name;
        }

        private string _name;

        [XmlAttribute()]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}