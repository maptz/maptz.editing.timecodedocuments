namespace Maptz.Editing.TimeCodeDocuments.Converters.PlainText
{


    public class MarkdownResult : ContentStreamableResult, IMarkdownResult
    {
        public MarkdownResult(string content) : base(content, "text/plain", ".md")
        {

        }
    }
}