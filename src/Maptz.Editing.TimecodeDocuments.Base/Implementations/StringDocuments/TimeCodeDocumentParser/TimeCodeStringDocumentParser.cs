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
            var prefix = string.Empty;
            var remain = content;

            ITimeCodeDocumentItem<string> currentItem = null;
            while (remain.Length > 0)
            {

                //var idx = remain.IndexOfAny(new char[] { '\n', '\r' });
                var regex = new Regex("[\\r\\n]+"); //Capture this line, plus any subsequence empty lines.
                var match = regex.Match(remain);
                var idx = match.Success ? match.Index : -1;

                string line;
                if (idx < 0)
                {
                    line = remain;
                    prefix += remain;
                    remain = string.Empty;
                }
                else
                {
                    line = remain.Substring(0, idx + match.Length);
                    prefix += line;
                    remain = remain.Substring(idx + match.Length);
                }


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

                        if (endTC.TotalFrames < startTC.TotalFrames)
                        {
                            var str = this.Settings.IgnoreTimeCodeLineRemainder ? string.Empty : remainder;
                            currentItem = new TimeCodeDocumentItem<string>(startTC.TotalFrames, 0, str, this.Settings.FrameRate,
                          textSpan: new TextSpan(prefix.Length - line.Length, line.Length, content),
                          contentTextSpan: new TextSpan(prefix.Length, 0, content),
                          prefixTextSpan: new TextSpan(prefix.Length - line.Length, line.Length, content));
                        }
                        else
                        {
                            var s = new TimeCodeDocumentItem<string>(startTC.TotalFrames, endTC.TotalFrames - startTC.TotalFrames, string.Empty, this.Settings.FrameRate,
                            textSpan: new TextSpan(prefix.Length - line.Length, line.Length, content),
                            contentTextSpan: new TextSpan(prefix.Length, 0, content),
                            prefixTextSpan: new TextSpan(prefix.Length - line.Length, line.Length, content));
                            currentItem = s;
                        }


                    }
                    else
                    {
                        //If you don't want to store the remainder..just pass string.empty.
                        var str = this.Settings.IgnoreTimeCodeLineRemainder ? string.Empty : remainder;
                        if (currentItem != null) { items.Add(currentItem); }
                        currentItem = new TimeCodeDocumentItem<string>(startTC.TotalFrames, 0, str, this.Settings.FrameRate,
                            textSpan: new TextSpan(prefix.Length - line.Length, line.Length, content),
                            contentTextSpan: new TextSpan(prefix.Length, 0, content),
                            prefixTextSpan: new TextSpan(prefix.Length - line.Length, line.Length, content));
                    }

                }
                else
                {
                    if (currentItem == null)
                    {
                        //We haven't started yet. 
                        continue;
                    }
                    string currentContent = currentItem.Content;
                    if (!string.IsNullOrEmpty(currentContent))
                    {
                        //currentContent += Environment.NewLine;
                    }
                    currentContent += line;
                    var newTextSpan = new TextSpan(currentItem.TextSpan.Start, prefix.Length - currentItem.TextSpan.Start, content);
                    var newContentTextSpan = new TextSpan(currentItem.ContentTextSpan.Start, prefix.Length - currentItem.ContentTextSpan.Start, content);
                    currentItem = new TimeCodeDocumentItem<string>(currentItem.Start, currentItem.Length, currentContent, this.Settings.FrameRate, newTextSpan, newContentTextSpan, currentItem.PrefixTextSpan);
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