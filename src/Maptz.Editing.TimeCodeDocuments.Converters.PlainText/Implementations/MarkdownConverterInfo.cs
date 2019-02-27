namespace Maptz.Editing.TimeCodeDocuments.Converters.PlainText
{
    public class MarkdownConverterInfo : ITimeCodeDocumentConverterInfo
    {
        public MarkdownConverterInfo()
        {
        }

    
        public string Name
        {
            get;
        }

        = "Markdown";
        public string Description
        {
            get;
        }

        = "Markdown";
        public string Tags
        {
            get;
        }

        = "Editing,Markdown";
    }
}