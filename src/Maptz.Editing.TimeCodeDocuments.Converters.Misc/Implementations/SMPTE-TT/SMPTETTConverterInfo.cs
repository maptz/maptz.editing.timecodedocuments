namespace Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT
{
    public class SMPTETTConverterInfo : ITimeCodeDocumentConverterInfo
    {
        public SMPTETTConverterInfo()
        {
        }

        public string Name
        {
            get;
        }

        = "SMPTETT";
        public string Description
        {
            get;
        }

        = "SMPTETT";
        public string Tags
        {
            get;
        }

        = "Editing,SMPETT";
    }
}