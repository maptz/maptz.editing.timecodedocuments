namespace Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro
{


    public class FinalCutXMLConverterInfo : ITimeCodeDocumentConverterInfo
    {
        public FinalCutXMLConverterInfo()
        {
        }


        public string Name
        {
            get;
        }

        = "FinalCutXML";
        public string Description
        {
            get;
        }

        = "FinalCutXML";
        public string Tags
        {
            get;
        }

        = "Editing,FinalCut";
    }
}