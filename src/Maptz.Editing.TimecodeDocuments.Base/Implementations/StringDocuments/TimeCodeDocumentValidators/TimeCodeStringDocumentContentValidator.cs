using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
namespace Maptz.Editing.TimeCodeDocuments.StringDocuments
{

    public class TimeCodeStringDocumentContentValidator : ITimeCodeDocumentContentValidator<string>
    {

        public TimeCodeStringDocumentContentValidator(IOptions<TimeCodeStringDocumentContentValidatorSettings> settings)
        {
            this.Settings = settings.Value;
        }


        /* #region Private Static Methods */
        private static List<string> GetShortenedLines(string line, int maxLength)
        {
            var retval = new List<string>();
            while (line.Length > maxLength)
            {
                var idx = line.Substring(0, maxLength).LastIndexOfAny(new char[] { ' ', '.' });
                if (idx == -1)
                {
                    idx = maxLength - 1;
                }
                retval.Add(line.Substring(0, idx));
                line = line.Substring(idx);
            }
            retval.Add(line);
            return retval;
        }
        /* #endregion Private Static Methods */
        public TimeCodeStringDocumentContentValidatorSettings Settings { get; }

        /* #region Private Methods */
        private IEnumerable<ITimeCodeDocumentItem<string>> ReduceLines(IEnumerable<ITimeCodeDocumentItem<string>> spans)
        {
            var retval = new List<ITimeCodeDocumentItem<string>>();
            foreach (var span in spans)
            {
                var lines = span.Content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);


                List<List<string>> ls = new List<List<string>>();
                var current = new List<string>();
                ls.Add(current); //TODO Max Lines...
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    line = line.Replace('\u0096', '-');
                    line = line.Replace('\u0097', '-');
                    line = line.Replace('\u0092', '\'');
                    line = line.Replace("\u0085", "...");
                    line = line.Replace("\u0093", "\"");
                    line = line.Replace("\u0094", "\"");
                    if (this.Settings.MaxLineLength.HasValue && line.Length > this.Settings.MaxLineLength)
                    {
                        var shortened = GetShortenedLines(line, this.Settings.MaxLineLength.Value);
                        current.AddRange(shortened);
                    }
                    else
                    {
                        current.Add(line);
                    }
                }

                foreach (var l in ls)
                {
                    var newContent = string.Join(Environment.NewLine, l);
                    var newSpan = new TimeCodeDocumentItem<string>(span.Start, span.Length, newContent, span.FrameRate);
                    retval.Add(newSpan);
                }
            }
            return retval;
        }
        /* #endregion Private Methods */
        /* #region Public Properties */

        /* #endregion Public Properties */
        /* #region Public Constructors */

        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Editing.TimecodeDocuments.ITimeCodeDocumentValidator' Methods */


        public ITimeCodeDocument<string> EnsureValidContent(ITimeCodeDocument<string> timeCodeDocument)
        {
            var timelineItems = timeCodeDocument.Items.ToArray();
            timelineItems = this.ReduceLines(timelineItems).ToArray();
            //Ensure there are no whitespace only subtitles (These cause import issues!)
            timelineItems = timelineItems.Where(p => !string.IsNullOrWhiteSpace(p.Content)).ToArray();

            return new TimeCodeDocument<string>
            {
                Items = timelineItems
            };
        }
    }
}