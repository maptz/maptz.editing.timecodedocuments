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
    public class Head
    {

        public Metadata metadata { get; set; } = new Metadata();
        [XmlArray("styling")]
        public Style[] styles { get; set; } = new Style[] { new Style() };

    }
}