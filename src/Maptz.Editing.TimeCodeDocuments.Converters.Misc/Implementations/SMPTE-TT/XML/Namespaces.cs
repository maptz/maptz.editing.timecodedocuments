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


    public class Namespaces
    {
        public const string tts = "http://www.w3.org/2006/10/ttaf1#style";
        public const string ttp = "http://www.w3.org/2006/10/ttaf1#parameter";
        public const string ttm = "http://www.w3.org/2006/10/ttaf1#metadata";
        public const string xml = "http://www.w3.org/XML/1998/namespace";
    }
}