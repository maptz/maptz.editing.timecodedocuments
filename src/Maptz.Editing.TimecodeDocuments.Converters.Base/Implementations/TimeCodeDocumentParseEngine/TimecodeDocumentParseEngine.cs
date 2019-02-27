using System.Linq;
using Microsoft.Extensions.Logging;

namespace Maptz.Editing.TimeCodeDocuments.Converters
{


    /// <summary>
    /// A service used to parse formatted files from loosly timecoded documents. The exact form is determined by the services used. 
    /// </summary>
    public class TimeCodeDocumentParseEngine<T, TContent> : ITimeCodeDocumentParseEngine<T>
    {
        /* #region Public Properties */
        public ILogger Logger { get; private set; }
        public ITimeCodeDocumentTimeValidator<TContent> TimeCodeDocumentTimeValidator { get; private set; }
        public ITimeCodeDocumentContentValidator<TContent> TimeCodeDocumentContentValidator { get; private set; }
        public ITimeCodeDocumentParser<TContent> TimeCodeDocumentParser { get; private set; }
        public ITimeCodeDocumentConverter<T, TContent> TimeCodeDocumentConverter { get; private set; }
        /* #endregion Public Properties */
        /* #region Public Constructors */
        public TimeCodeDocumentParseEngine(
            ILoggerFactory loggerFactory,
            ITimeCodeDocumentParser<TContent> timeCodeDocumentParser,
            ITimeCodeDocumentTimeValidator<TContent> timeCodeDocumentTimeValidator,
            ITimeCodeDocumentContentValidator<TContent> timeCodeDocumentContentValidator,
            ITimeCodeDocumentConverter<T, TContent> timeCodeDocumentConverter)
        {
            this.Logger = loggerFactory.CreateLogger(typeof(TimeCodeDocumentParseEngine<T, TContent>).Name);
            this.TimeCodeDocumentParser = timeCodeDocumentParser;
            this.TimeCodeDocumentTimeValidator = timeCodeDocumentTimeValidator;
            this.TimeCodeDocumentContentValidator = timeCodeDocumentContentValidator;
            this.TimeCodeDocumentConverter = timeCodeDocumentConverter;
        }
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Avid.Ds.ITimecodeDocumentTransformerService' Methods */

        public T Parse(string source)
        {

            ITimeCodeDocument<TContent> timeCodeDocument;
            this.Logger.LogInformation("Looking for TimeCoded spans in text");
            {
                timeCodeDocument = this.TimeCodeDocumentParser.Parse(source);
                this.Logger.LogInformation($"Found {timeCodeDocument.Items.Count()} TimeCoded spans.");
            }

            this.Logger.LogInformation("Cleaning TimeCoded spans.");
            {
                timeCodeDocument = this.TimeCodeDocumentTimeValidator.EnsureValidTimes(timeCodeDocument);
                timeCodeDocument = this.TimeCodeDocumentContentValidator.EnsureValidContent(timeCodeDocument);
            }

            this.Logger.LogInformation("Converting TimeCoded spans.");
            {
                var retval = this.TimeCodeDocumentConverter.Convert(timeCodeDocument);
                return retval;
            }
        }
        /* #endregion Interface: 'Maptz.Avid.Ds.ITimecodeDocumentTransformerService' Methods */
    }
}
