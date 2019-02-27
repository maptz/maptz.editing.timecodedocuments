namespace Maptz.Editing.TimeCodeDocuments.Converters.SRT
{
    public class SRTConverterInfo : ITimeCodeDocumentConverterInfo
    {
        public SRTConverterInfo()
        {
        }


        public string Name
        {
            get;
        }

        = "SRT";
        public string Description
        {
            get;
        }

        = "SRT";
        public string Tags
        {
            get;
        }

        = "Editing,SRT";
    }
}