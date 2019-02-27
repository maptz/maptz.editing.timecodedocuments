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

    //[XmlType(TypeName ="p")]
    public class Paragraph : IXmlSerializable
    {
        //  [XmlAttribute]
        public string begin { get; set; }
        //[XmlAttribute]
        public string end { get; set; }
        //[XmlText]
        public string Content { get; set; }

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
            writer.WriteAttributeString(nameof(begin), this.begin);
            writer.WriteAttributeString(nameof(end), this.end);
            if (this.Content != null)
            {
                var lines = this.Content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                var output = string.Join("<br />", lines);
                writer.WriteRaw(output);
            }
        }
    }
}