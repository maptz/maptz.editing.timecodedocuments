using Maptz.Editing.TimeCodeDocuments.Converters.Avid;
using Maptz.Text;
using Maptz.Timelines;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Maptz.Editing.TimeCodeDocuments.Converters.Avid
{

    public class AvidDSConverterInfo : ITimeCodeDocumentConverterInfo
    {
        public AvidDSConverterInfo()
        {

        }


        public string Name { get; } = "AvidDSConverter";

        public string Description { get; } = "AvidDSConverter";

        public string Tags { get; } = "Editing,Avid";
    }
}