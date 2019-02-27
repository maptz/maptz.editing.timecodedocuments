using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
namespace Maptz.Editing.TimeCodeDocuments.Converters.PlainText
{
    /// <summary>
    /// A converter used to create markdown files from timecode spans.
    /// </summary>
    public class TimeCodeToMarkdownConverter : ITimeCodeDocumentToMarkdownConverter
    {
        object ITimeCodeDocumentConverter.Convert(ITimeCodeDocument timeCodeDocument)
        {
            return this.Convert(timeCodeDocument as ITimeCodeDocument<string>);
        }

        public TimeCodeToMarkdownConverter(ILoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory.CreateLogger(typeof(TimeCodeToMarkdownConverter).Name);
        }

        public ILogger Logger { get; private set; }

        
        public IMarkdownResult Convert(ITimeCodeDocument<string> timeCodeDocument)
        {
            var spans = timeCodeDocument.Items;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("| In | Content |");
            stringBuilder.AppendLine("|:----:|:----|");
            foreach (var span in spans)
            {
                stringBuilder.AppendLine($"| {span.RecordIn.ToString()} | {span.Content.Replace("\r\n", " ")} |");
            }

            var retvalContent = stringBuilder.ToString();
            return new MarkdownResult(retvalContent);
        }
    }
}