using System.Collections.Generic;
using System.Xml;

namespace Condor.Core
{
    public class UILoader
    {
        private XmlDocument _xmlDoc;
        private string _fileName = @"CondorXELoader.xml";

        public UILoader()
        {
            Load();
        }

        public string Version
        {
            get
            {
                XmlNode node = _xmlDoc.DocumentElement.SelectSingleNode("//version");
                return node.InnerText;
            }
        }

        public List<UINode> GetORMFrameworks()
        {
            string xpath = "//add[@type='ORMFramework']";
            XmlNodeList nodes = _xmlDoc.DocumentElement.SelectNodes(xpath);

            List<UINode> frameworks = new List<UINode>();
            foreach (XmlNode item in nodes)
            {
                frameworks.Add(new UINode(item.Attributes["value"].Value, item.Attributes["text"].Value));
            }

            return frameworks;
        }

        public XmlNode GetNode(string xpath)
        {
            XmlNode node = _xmlDoc.DocumentElement.SelectSingleNode(xpath);
            return node;
        }

        public XmlNodeList GetNodes(string xpath)
        {
            XmlNodeList nodes = _xmlDoc.DocumentElement.SelectNodes(xpath);
            return nodes;
        }

        private void Load()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_fileName);
            _xmlDoc = xmlDoc;
        }
    }

    public class UINode
    {
        public UINode(string value, string text)
        {
            this._value = value;
            this._text = text;
        }

        private string _value;

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private string _text;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
    }
}
