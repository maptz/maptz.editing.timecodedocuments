namespace Maptz.Editing.TimeCodeDocuments
{

    public interface ITimeCodeDocumentConverter
    {
        object Convert(ITimeCodeDocument timeCodeDocument);
    }

    public interface ITimeCodeDocumentConverter<T, TDocumentContent> : ITimeCodeDocumentConverter
    {
        T Convert(ITimeCodeDocument<TDocumentContent> timeCodeDocument);
    }

}