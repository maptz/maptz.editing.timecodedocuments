using System.Linq;
using Maptz.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Maptz.Editing.TimeCodeDocuments.StringDocuments
{
    public class TimeCodeStringDocumentParser : ITimeCodeDocumentParser<string>
    {

        public TimeCodeStringDocumentParser(ILoggerFactory loggerFactory, IOptions<TimeCodeStringDocumentParserSettings> settings)
        {
            this.Settings = settings.Value;
            this.LoggerFactory = loggerFactory;
        }

        public ILoggerFactory LoggerFactory { get; private set; }
        public TimeCodeStringDocumentParserSettings Settings { get; private set; }


        public ITimeCodeDocument<string> Parse(string content)
        {
            if (content == null) return new TimeCodeDocument<string>
            {
                Items = new TimeCodeDocumentItem<string>[0]
            };
            var items = new List<ITimeCodeDocumentItem<string>>();
            var lines = content.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);
            ITimeCodeDocumentItem<string> currentItem = null;
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                var lineTrim = line.Trim();
                var tcMatchPattern = "^[0-9]+[\\:\\.\\;][0-9]+[\\:\\.\\;][0-9]+([\\:\\.\\;][0-9]+)*";
                var regexMatch = Regex.Match(lineTrim, tcMatchPattern);
                if (regexMatch.Success)
                {

                    var startStr = CleanTcString(lineTrim.Substring(0, regexMatch.Length));
                    if (this.Settings.IgnoreFrames)
                    {
                        var parts = startStr.Split(':');
                        if (parts.Length == 4)
                        {
                            startStr = $"{parts[0]}:{parts[1]}:{parts[2]}:00";
                        }
                    }

                    ITimeCode startTC;
                    try
                    {
                        startTC = new TimeCode(startStr, this.Settings.FrameRate);
                    }
                    catch (Exception)
                    {
                        System.Diagnostics.Debugger.Launch();
                        //Skip over an exception parsing a line
                        continue;
                    }

                    var remainder = lineTrim.Length > regexMatch.Length ? lineTrim.Substring(regexMatch.Length).Trim() : string.Empty;
                    var remainderIsTCMatch = Regex.Match(remainder, tcMatchPattern);
                    if (remainderIsTCMatch.Success)
                    {
                        var endStr = CleanTcString(remainder.Substring(0, remainderIsTCMatch.Length));

                        ITimeCode endTC;
                        try
                        {
                            endTC = new TimeCode(endStr, this.Settings.FrameRate);
                        }
                        catch (Exception)
                        {
                            //Skip over an exception parsing a line
                            continue;
                        }

                        if (currentItem != null) { items.Add(currentItem); }

                        var s = new TimeCodeDocumentItem<string>(startTC.TotalFrames, endTC.TotalFrames - startTC.TotalFrames, string.Empty, this.Settings.FrameRate);
                        currentItem = s;
                    }
                    else
                    {
                        //If you don't want to store the remainder..just pass string.empty.
                        var str = this.Settings.IgnoreTimeCodeLineRemainder ? string.Empty : remainder;
                        if (currentItem != null) { items.Add(currentItem); }
                        currentItem = new TimeCodeDocumentItem<string>(startTC.TotalFrames, 0, str, this.Settings.FrameRate);
                    }

                }
                else
                {
                    if (currentItem == null) { continue; }
                    string currentContent = currentItem.Content;
                    if (!string.IsNullOrEmpty(currentContent))
                    {
                        currentContent += Environment.NewLine;
                    }
                    currentContent += line;
                    currentItem = new TimeCodeDocumentItem<string>(currentItem.Start, currentItem.Length, currentContent, this.Settings.FrameRate);
                }
            }

            if (currentItem != null && currentItem.Content != null)
            {
                items.Add(currentItem);
            }
            return new TimeCodeDocument<string>
            {
                Items = items.ToArray()
            };
        }
     

        private string CleanTcString(string tc)
        {
            //var mode = "hh:mm:ss";
            var clean = tc.Replace('.', ':');
            var split = clean.Split(new char[] { ':', ';' }).ToList();
            if (split.Count == 3)
            {
                if (this.Settings.ParserMode == ParserMode.HHMMSS)
                {
                    split.Add("00");
                }
                else if (this.Settings.ParserMode == ParserMode.MMSSFF)
                {
                    split.Insert(0, "00");
                }
                else throw new NotSupportedException();
                
            }

            split = split.Select(p => int.Parse(p).ToString("00")).ToList();

            var ret = string.Join(":", split);
            return ret;
        }
    }
}