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

namespace Maptz.Editing.TimeCodeDocuments.Converters.SRT
{
 
    /// <summary>
    /// A converter used to create Avid DS files from groups of ITimeCodeContentSpans. 
    /// </summary>
    public class TimeCodeDocumentToSRTConverter : ITimeCodeDocumentToSRTConverter
    {
        object ITimeCodeDocumentConverter.Convert(ITimeCodeDocument timeCodeDocument)
        {
            return this.Convert(timeCodeDocument as ITimeCodeDocument<string>);
        }

        /* #region Public Properties */
        public ILogger Logger { get; private set; }
        public ITextCleanerService TextCleanerService { get; private set; }
        /* #endregion Public Properties */
        /* #region Public Constructors */
        public TimeCodeDocumentToSRTConverter(ILoggerFactory loggerFactory, ITextCleanerService textCleanerService)
        {
            this.Logger = loggerFactory.CreateLogger(typeof(TimeCodeDocumentToSRTConverter).Name);
            this.TextCleanerService = textCleanerService;
        }

     
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */
        public ISRTResult Convert(ITimeCodeDocument<string> timeCodeDocument)
        {
            var spans = timeCodeDocument.Items;
            var stringBuilder = new StringBuilder();

            var spansArr = spans.ToArray();
            for (int i = 0; i < spansArr.Length; i++)
            {
                var span = spansArr[i];
                stringBuilder.AppendLine((i + 1).ToString());
                stringBuilder.AppendLine(span.RecordIn.ToFractionalString(",") + " --> " + span.RecordOut.ToFractionalString(","));
                var cleanedContent = this.TextCleanerService.CleanText(span.Content);
                stringBuilder.AppendLine(cleanedContent);
                stringBuilder.AppendLine();
            }
            var retvalContent = stringBuilder.ToString();

            return new SRTResult(retvalContent);
        }
        /* #endregion Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */
    }

}