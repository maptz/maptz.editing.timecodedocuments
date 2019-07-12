namespace Maptz.Editing.TimeCodeDocuments
{
    public interface ITextSpan
    {
        int Start { get; }
        int Length { get; }
        string Document { get; }
    }
}