namespace Maptz.Editing.TimeCodeDocuments
{

    public interface ITimeCodeDocumentItem : ITimeCodeTimelineContentSpan
    {
    }

    public interface ITimeCodeDocumentItem<T> : ITimeCodeDocumentItem, ITimeCodeTimelineContentSpan<T>
    {
        ITextSpan TextSpan { get; }
        ITextSpan ContentTextSpan { get; }
        ITextSpan PrefixTextSpan { get; }
    }
}