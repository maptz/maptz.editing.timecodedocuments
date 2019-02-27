namespace Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro
{

    public class FinalCutXMLResult : ContentStreamableResult, IFinalCutXMLResult
    {
        public FinalCutXMLResult(string content) : base(content, "text/xml", ".xml")
        {
        }
    }
}