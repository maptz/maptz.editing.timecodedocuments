namespace Maptz.Editing.TimeCodeDocuments
{

    public interface ITimeCodeDocumentItem : ITimeCodeTimelineContentSpan
    {
    }

    public interface ITimeCodeDocumentItem<T> : ITimeCodeDocumentItem, ITimeCodeTimelineContentSpan<T>
    {

    }
}