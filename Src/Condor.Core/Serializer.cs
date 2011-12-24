using System.IO;
using System.Xml.Serialization;

namespace Condor.Core
{
    public class Serializer<T> where T : class
    {
        public static void Serialize(T request, string fileName)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer xmlFormat = new XmlSerializer(typeof(T));
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            xmlFormat.Serialize(fs, request, ns);
            fs.Close();
            fs.Dispose();
        }

        public static T Deserialize(string xmlString)
        {
            FileStream fs = new FileStream(xmlString, FileMode.Open);
            XmlSerializer xs = new XmlSerializer(typeof(T));

            T response = (T)xs.Deserialize(fs);
            fs.Close();
            fs.Dispose();
            return response;
        }
    }	
}
