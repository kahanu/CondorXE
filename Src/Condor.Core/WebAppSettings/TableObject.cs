using System.Collections;
using System.Xml.Serialization;
using System;

namespace Condor.Core
{
    [Serializable]
    public class TableObject
    {
        private ArrayList _tables;

        public TableObject()
        {
            _tables = new ArrayList();
        }

        public void AddItem(TableItem item)
        {
            _tables.Add(item);
        }

        public TableItem[] GetItems()
        {
            TableItem[] items = new TableItem[_tables.Count];
            _tables.CopyTo(items);
            return items;
        }

        [XmlElement("Table")]
        public TableItem[] Tables
        {
            get
            {
                TableItem[] items = new TableItem[_tables.Count];
                _tables.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null) return;
                TableItem[] items = value;
                _tables.Clear();
                foreach (TableItem item in items)
                    _tables.Add(item);
            }
        }
    }
}