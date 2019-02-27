namespace Maptz.Editing.TimeCodeDocuments.Converters.Avid
{

    /// <summary>
    /// A timecode content spans converter used to convert ITimeCodeContentSpans into Avid DS files. 
    /// </summary>
    public interface ITimeCodeDocumentToAvidDSConverter : ITimeCodeDocumentConverter<IAvidDSResult, string>
    {

    }
}