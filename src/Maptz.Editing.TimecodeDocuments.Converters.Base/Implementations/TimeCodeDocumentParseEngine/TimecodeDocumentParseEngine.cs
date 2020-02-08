using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maptz.Editing.TimeCodeDocuments.Converters
{

    public class TimeCodeDocumentParseEngineOptions<T, TContent>
    {
        public long Offset { get; set; }
        public Func<ITimeCodeDocument<TContent>, ITimeCodeDocument<TContent>> BeforeExport { get; set; }
    }

    /// <summary>
    /// A service used to parse formatted files from loosly timecoded documents. The exact form is determined by the services used. 
    /// </summary>
    public class TimeCodeDocumentParseEngine<T, TContent> : ITimeCodeDocumentParseEngine<T>
    {
        public TimeCodeDocumentParseEngineOptions<T, TContent> Settings { get; }

        /* #region Public Properties */
        public ILogger Logger { get; private set; }
        public ITimeCodeDocumentTimeValidator<TContent> TimeCodeDocumentTimeValidator { get; private set; }
        public ITimeCodeDocumentContentValidator<TContent> TimeCodeDocumentContentValidator { get; private set; }
        public ITimeCodeDocumentParser<TContent> TimeCodeDocumentParser { get; private set; }
        public ITimeCodeDocumentConverter<T, TContent> TimeCodeDocumentConverter { get; private set; }
        /* #endregion Public Properties */
        /* #region Public Constructors */
        public TimeCodeDocumentParseEngine(
            IOptions<TimeCodeDocumentParseEngineOptions<T, TContent>> options,
            ILoggerFactory loggerFactory,
            ITimeCodeDocumentParser<TContent> timeCodeDocumentParser,
            ITimeCodeDocumentTimeValidator<TContent> timeCodeDocumentTimeValidator,
            ITimeCodeDocumentContentValidator<TContent> timeCodeDocumentContentValidator,
            ITimeCodeDocumentConverter<T, TContent> timeCodeDocumentConverter)
        {
            this.Settings = options.Value;
            this.Logger = loggerFactory.CreateLogger(typeof(TimeCodeDocumentParseEngine<T, TContent>).Name);
            this.TimeCodeDocumentParser = timeCodeDocumentParser;
            this.TimeCodeDocumentTimeValidator = timeCodeDocumentTimeValidator;
            this.TimeCodeDocumentContentValidator = timeCodeDocumentContentValidator;
            this.TimeCodeDocumentConverter = timeCodeDocumentConverter;
        }
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Avid.Ds.ITimecodeDocumentTransformerService' Methods */

        public T Parse(string source, IList<string> warnings)
        {

            ITimeCodeDocument<TContent> timeCodeDocument;
            this.Logger.LogInformation("Looking for TimeCoded spans in text");
            {
                timeCodeDocument = this.TimeCodeDocumentParser.Parse(source);
                this.Logger.LogInformation($"Found {timeCodeDocument.Items.Count()} TimeCoded spans.");
            }

            this.Logger.LogInformation("Cleaning TimeCoded spans.");
            {
                var warningsO = this.TimeCodeDocumentTimeValidator.IssueWarnings(timeCodeDocument);
                foreach (var warning in warningsO)
                {
                    this.Logger.LogError(warning);
                    if (warnings != null)
                        warnings.Add(warning);
                }

                //Time validator ensures that the timecodes are in the right order and are non-overlapping. 
                timeCodeDocument = this.TimeCodeDocumentTimeValidator.EnsureValidTimes(timeCodeDocument);
                //Content validator cleans the text in the TimeCodeDocument.
                timeCodeDocument = this.TimeCodeDocumentContentValidator.EnsureValidContent(timeCodeDocument);
            }


            this.Logger.LogInformation("Performing BeforeExport function");
            if (this.Settings != null && this.Settings.BeforeExport != null)
            {
                timeCodeDocument = this.Settings.BeforeExport(timeCodeDocument);
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
