namespace Maptz.Editing.TimeCodeDocuments
{

    public class TimeCodeDocumentItem<T> : TimeCodeTimelineContentSpan<T>, ITimeCodeDocumentItem<T>
    {
        public TimeCodeDocumentItem(long startFrame, long lengthFrames, T content, SmpteFrameRate frameRate) : base(startFrame, lengthFrames, content, frameRate)
        {
        }
    }
}