namespace Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT
{


    public class SMPTETTResult : ContentStreamableResult, ISMPTETTResult
    {
        public SMPTETTResult(string content) : base(content, "text/plain", ".txt")
        {
        }
    }
}