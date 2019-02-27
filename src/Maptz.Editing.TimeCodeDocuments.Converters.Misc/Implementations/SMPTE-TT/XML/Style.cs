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
    [XmlType(TypeName = "style")]
    public class Style
    {
        public Style()
        {
            this.id = "s0";
            this.backgroundColor = "black";
            this.fontStyle = "normal";
            this.fontFamily = "arial";
            this.color = "white";
        }

        [XmlAttribute]
        public string id { get; set; }
        [XmlAttribute(Namespace = Namespaces.tts)]
        public string backgroundColor { get; set; }
        [XmlAttribute(Namespace = Namespaces.tts)]
        public string fontStyle { get; set; }
        [XmlAttribute(Namespace = Namespaces.tts)]
        public string fontSize { get; set; }
        [XmlAttribute(Namespace = Namespaces.tts)]
        public string fontFamily { get; set; }
        [XmlAttribute(Namespace = Namespaces.tts)]
        public string color { get; set; }

    }
}