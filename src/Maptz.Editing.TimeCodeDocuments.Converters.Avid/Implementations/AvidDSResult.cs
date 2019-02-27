namespace Maptz.Editing.TimeCodeDocuments.Converters.Avid
{

    public class AvidDSResult : ContentStreamableResult, IAvidDSResult
    {
        public AvidDSResult(string content) : base(content, "text/plain", ".ds.txt")
        {

        }
    }
}