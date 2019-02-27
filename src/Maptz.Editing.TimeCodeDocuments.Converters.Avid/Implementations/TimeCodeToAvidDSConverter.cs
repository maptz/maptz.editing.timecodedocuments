using System.Text;
using Maptz.Text;
using Microsoft.Extensions.Logging;
namespace Maptz.Editing.TimeCodeDocuments.Converters.Avid
{

    /// <summary>
    /// A converter used to create Avid DS files from groups of ITimeCodeContentSpans. 
    /// </summary>
    public class TimeCodeToAvidDSConverter : ITimeCodeDocumentToAvidDSConverter
    {
        /* #region Public Properties */
        public ILogger Logger { get; private set; }
        public ITextCleanerService TextCleanerService { get; private set; }
        /* #endregion Public Properties */
        /* #region Public Constructors */
        public TimeCodeToAvidDSConverter(ILoggerFactory loggerFactory, ITextCleanerService textCleanerService)
        {
            this.Logger = loggerFactory.CreateLogger(typeof(TimeCodeToAvidDSConverter).Name);
            this.TextCleanerService = textCleanerService;
        }
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */

        public IAvidDSResult Convert(ITimeCodeDocument<string> timeCodeDocument)
        {
            var items = timeCodeDocument.Items;
            #region Sample Headers
            //@ This is a DS Subtitles file that contains a full header section.
            //@ All possible properties are specified.

            //@~~~~~~~~~~~~~~~~~~~~~~~~
            //@ Header section
            //@~~~~~~~~~~~~~~~~~~~~~~~~

            //<generate clips> on

            //<single effect> off

            //<timecode offset> 01:00:00:00

            //<force non realtime> off

            //<font> Times New Roman
            //<font style> Regular
            //<font size> 32

            //<antialiasing> on
            //<antialiasing extra filtering> 0
            //<antialiasing supersampling> 4x4

            //<font kern pairs> off
            //<font hinting> off
            //<font filtering> on
            //<font subpixel positioning> on

            //<character transform x> 0
            //<character transform y> 0

            //<face color> 100 90 70
            //<face opacity> 100
            //<face softness> 0

            //<use edge> on
            //<edge color> 0 0 0
            //<edge opacity> 100
            //<edge softness> 0
            //<edge width> 1
            //<edge face cut> off

            //<use shadow> on
            //<shadow color> 0 0 0
            //<shadow opacity> 100
            //<shadow softness> 5
            //<shadow offset> 0
            //<shadow angle> 0
            //<shadow face cut> on

            //<kerning> 1.0
            //<leading> 3
            //<leading mode> full

            //<alignment> center

            //<safe area> safe title
            //<text body horizontal alignment> center
            //<text body vertical alignment> bottom
            //<text body horizontal offset> 0
            //<text body vertical offset> 0

            //@~~~~~~~~~~~~~~~~~~~~~~~~
            //@ Subtitles section
            //@~~~~~~~~~~~~~~~~~~~~~~~~
            #endregion

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<begin subtitles>");
            foreach (var item in items)
            {
                stringBuilder.AppendLine($"{item.RecordIn} {item.RecordOut}");
                var cleanedContent = this.TextCleanerService.CleanText(item.Content);
                stringBuilder.AppendLine(cleanedContent);
                stringBuilder.AppendLine(string.Empty);
            }
            stringBuilder.AppendLine("<end subtitles>");
            stringBuilder.AppendLine(string.Empty);

            var content = stringBuilder.ToString();
            var retval = new AvidDSResult(content);
            return retval;
        }


        object ITimeCodeDocumentConverter.Convert(ITimeCodeDocument timeCodeDocument)
        {
            return this.Convert(timeCodeDocument as ITimeCodeDocument<string>);
        }




        /* #endregion Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */
    }
}