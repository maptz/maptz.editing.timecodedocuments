namespace Maptz.Editing.TimeCodeDocuments.StringDocuments
{

    /// <summary>
    /// Settings used by the ITimeCodeContentSpanParserService. 
    /// </summary>
    public class TimeCodeStringDocumentParserSettings
    {
        public ParserMode ParserMode { get; set; }
        public SmpteFrameRate FrameRate { get; set; }
        public bool IgnoreFrames { get; set; }
        public bool IgnoreTimeCodeLineRemainder { get; set; }
    }
}