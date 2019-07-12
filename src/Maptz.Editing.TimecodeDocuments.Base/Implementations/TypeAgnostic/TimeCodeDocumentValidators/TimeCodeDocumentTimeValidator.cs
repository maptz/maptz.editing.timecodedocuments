using System;
using System.Linq;
using Microsoft.Extensions.Options;
namespace Maptz.Editing.TimeCodeDocuments
{


    public class TimeCodeDocumentTimeValidator<T> : ITimeCodeDocumentTimeValidator<T>
    {
        public TimeCodeDocumentTimeValidatorSettings Settings { get; private set; }

        public TimeCodeDocumentTimeValidator(IOptions<TimeCodeDocumentTimeValidatorSettings> settings)
        {
            this.Settings = settings.Value;
        }


        public ITimeCodeDocument<T> EnsureValidTimes(ITimeCodeDocument<T> timeCodeDocument)
        {
            var timelineItems = timeCodeDocument.Items.ToArray();

            timelineItems = timelineItems.OrderBy(p => p.RecordIn.TotalFrames).ToArray();

            //Step 1 ensure that no items are at the same timecode. 
            /* #region Step 1 - Correct any items that are at the same timecode. */
            var startFrameGroups = timelineItems.GroupBy(p => p.RecordIn.TotalFrames).ToArray();
            var startFrameIssueItems = startFrameGroups.Where(p => p.Count() > 1).ToArray();

            //this.Logger.LogInformation($"Found {startFrameIssueItems.Count()} items with identical timecode.");
            foreach (var startFrameIssueGroup in startFrameIssueItems)
            {
                var startFrame = startFrameIssueGroup.Key;
                var nextMarkedFrames = timelineItems.Select(p => p.RecordIn.TotalFrames).Where(p => p > startFrame).OrderBy(p => p);
                var nextMarkedFrame = nextMarkedFrames.Any() ? nextMarkedFrames.First() : long.MaxValue;

                var availableFrameGap = nextMarkedFrame - startFrame;
                var actualFrameGap = Math.Min(availableFrameGap, startFrameIssueGroup.Count() * this.Settings.DefaultDurationFrames);
                if (actualFrameGap == 0) throw new InvalidOperationException("Not enough room for all the subtitles.");

                var totalFramesPerItem = actualFrameGap / startFrameIssueGroup.Count();

                //Add them back in reverse order...
                var originalTimelineitemslist = timelineItems.ToList();
                for (var i = 0; i < startFrameIssueGroup.Count(); i++)
                {
                    var issueItem = startFrameIssueGroup.ElementAt(i);
                    var newStart = TimeCode.FromFrames(startFrame + i * totalFramesPerItem, issueItem.FrameRate);
                    var indexOfItem = originalTimelineitemslist.IndexOf(issueItem);
                    timelineItems[indexOfItem] = new TimeCodeDocumentItem<T>(newStart.TotalFrames, issueItem.Length, issueItem.Content, issueItem.FrameRate, issueItem.TextSpan, issueItem.ContentTextSpan, issueItem.PrefixTextSpan);
                }
            }
            /* #endregion*/

            /* #region Step 2 - Ensure the subtitles are non overlapping, and have duration. */
            for (var j = 1; j < timelineItems.Count(); j++)
            {
                var currentItem = timelineItems.ElementAt(j - 1);
                var nextItem = timelineItems.ElementAt(j);

                long maxEnd;
                if (this.Settings.MaxDurationFrames.HasValue)
                {
                    maxEnd = Math.Min(currentItem.RecordIn.TotalFrames + this.Settings.MaxDurationFrames.Value, nextItem.RecordIn.TotalFrames - 1);
                }
                else
                {
                    maxEnd = nextItem.RecordIn.TotalFrames - 1;
                }
                

                if (currentItem.RecordOut == null || currentItem.RecordOut.TotalFrames <= currentItem.RecordIn.TotalFrames)
                {
                    var newRecordOut = TimeCode.FromFrames(maxEnd, currentItem.FrameRate);
                    currentItem = new TimeCodeDocumentItem<T>(currentItem.Start, newRecordOut.TotalFrames - currentItem.Start, currentItem.Content, currentItem.FrameRate, currentItem.TextSpan, currentItem.ContentTextSpan, currentItem.PrefixTextSpan);
                    timelineItems[j - 1] = currentItem;
                }
                else if (currentItem.RecordOut.TotalFrames > maxEnd)
                {
                    var newRecordOut = TimeCode.FromFrames(maxEnd, currentItem.FrameRate);
                    currentItem = new TimeCodeDocumentItem<T>(currentItem.Start, newRecordOut.TotalFrames - currentItem.Start, currentItem.Content, currentItem.FrameRate, currentItem.TextSpan, currentItem.ContentTextSpan, currentItem.PrefixTextSpan);
                    timelineItems[j - 1] = currentItem;
                }

                if (j == timelineItems.Count() - 1)
                {
                    if (nextItem.RecordOut == null || nextItem.RecordOut.TotalFrames <= nextItem.RecordIn.TotalFrames)
                    {
                        var newRecordOut = TimeCode.FromFrames(nextItem.RecordIn.TotalFrames + this.Settings.DefaultDurationFrames, nextItem.FrameRate);

                        nextItem = new TimeCodeDocumentItem<T>(nextItem.Start, newRecordOut.TotalFrames - nextItem.Start, nextItem.Content, nextItem.FrameRate, currentItem.TextSpan, currentItem.ContentTextSpan, currentItem.PrefixTextSpan);
                        timelineItems[j] = nextItem;
                    }
                }

            }
            /* #endregion*/

            if (this.Settings.OffsetFrames != 0)
            {
                for (int i = 0; i < timelineItems.Length; i++)
                {
                    var timelineItem = timelineItems[i];
                    var offset = this.Settings.OffsetFrames;
                    var updatedItem = new TimeCodeDocumentItem<T>(timelineItem.RecordIn.TotalFrames + offset,
                        timelineItem.RecordOut.TotalFrames + offset,
                        timelineItem.Content,
                        timelineItem.FrameRate, timelineItem.TextSpan, timelineItem.ContentTextSpan, timelineItem.PrefixTextSpan);

                    timelineItems[i] = updatedItem;
                }
            }
            

            return new TimeCodeDocument<T>
            {
                Items = timelineItems
            };
        }
    }
}