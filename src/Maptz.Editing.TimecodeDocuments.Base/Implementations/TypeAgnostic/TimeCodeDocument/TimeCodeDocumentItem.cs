namespace Maptz.Editing.TimeCodeDocuments
{

    public class TimeCodeDocumentItem<T> : TimeCodeTimelineContentSpan<T>, ITimeCodeDocumentItem<T>
    {
        public TimeCodeDocumentItem(long startFrame, long lengthFrames, T content, SmpteFrameRate frameRate, ITextSpan textSpan, ITextSpan contentTextSpan, ITextSpan prefixTextSpan) : base(startFrame, lengthFrames, content, frameRate)
        {
            this.TextSpan = textSpan;
            this.ContentTextSpan = contentTextSpan;
            this.PrefixTextSpan = prefixTextSpan;
        }

        public ITextSpan TextSpan { get; }
        public ITextSpan ContentTextSpan { get; }
        public ITextSpan PrefixTextSpan { get; }
    }
}