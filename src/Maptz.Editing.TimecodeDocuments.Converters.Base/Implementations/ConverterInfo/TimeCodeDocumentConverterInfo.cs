using System;
using System.Collections.Generic;
using System.Text;
namespace Maptz.Editing.TimeCodeDocuments.Converters
{
    public class  TimeCodeDocumentConverterInfo : ITimeCodeDocumentConverterInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
    }
}