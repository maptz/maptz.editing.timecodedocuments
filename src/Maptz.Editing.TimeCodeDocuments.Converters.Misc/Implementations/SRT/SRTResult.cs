namespace Maptz.Editing.TimeCodeDocuments.Converters.SRT
{


    public class SRTResult : ContentStreamableResult, ISRTResult
    {
        public SRTResult(string content) : base(content, "text/plain", ".srt")
        {
        }
    }
}