using Maptz.Text;
using Maptz.Timelines;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
namespace Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT.Xml
{


    public class Body : IXmlSerializable
    {
        [XmlAttribute(Namespace = Namespaces.tts)]
        public string textAlign { get; set; }
        [XmlAttribute]
        public string style { get; set; }

        public Body()
        {
            this.style = "s0";
            this.textAlign = "center";
        }
        [XmlArray("div")]
        public Paragraph[] div { get; set; } = new Paragraph[0];

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            //writer.WriteStartElement(GetType().ToString());
            writer.WriteAttributeString(nameof(textAlign), Namespaces.tts, this.textAlign);
            writer.WriteAttributeString(nameof(style), this.style);

            writer.WriteStartElement("div");
            foreach (var paragraph in div)
            {
                writer.WriteStartElement("p");
                paragraph.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}