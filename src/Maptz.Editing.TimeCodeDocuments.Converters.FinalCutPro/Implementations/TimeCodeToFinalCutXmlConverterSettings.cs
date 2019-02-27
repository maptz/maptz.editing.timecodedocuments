using Maptz.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro.Xml;
namespace Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro
{
    public class TimeCodeToFinalCutXmlConverterSettings
    {
        public int FontSize
        {
            get;
            set;
        }

        = 21;
        public string SequenceName
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        = 1080; //3840 x 2160
        public int Width
        {
            get;
            set;
        }

        = 1920; //2160
    }
}