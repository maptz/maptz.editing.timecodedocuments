namespace Maptz.Editing.TimeCodeDocuments
{
    /// <summary>
    /// Settings used by the content span cleaner. 
    /// </summary>
    public class TimeCodeDocumentTimeValidatorSettings
    {
        public int OffsetFrames { get; set; }


        /// <summary>
        /// The default duration for timeline spans. 
        /// </summary>
        public int DefaultDurationFrames { get; set; }

        /// <summary>
        /// The default duration for timeline spans. 
        /// </summary>
        public int? MaxDurationFrames { get; set; }
    }
}