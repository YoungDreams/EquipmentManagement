using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Foundation.Core
{
    public static class XmlExtensions
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = true};
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            var xmlserializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    xmlserializer.Serialize(writer, value, namespaces);
                    return stringWriter.ToString();
                }
            }
        }

        public static T Deserialize<T>(this string value)
        {
            if (value == null)
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(value));
            return (T) serializer.Deserialize(memStream);
        }



        public static string SerializeContainHeader<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Indent = false,
                Encoding = new UTF8Encoding(false)
            };
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            var xmlserializer = new XmlSerializer(typeof(T), new XmlRootAttribute("businessdata") { });
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    xmlserializer.Serialize(writer, value, namespaces);
                    return Encoding.UTF8.GetString(stream.ToArray()).Replace("utf-8", "UTF-8");
                }
            }
        }
    }
}
