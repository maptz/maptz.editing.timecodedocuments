using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT.Xml;
using Maptz.Text;
using Microsoft.Extensions.Logging;

namespace Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT
{


    /// <summary>
    /// A converter used to create Avid DS files from groups of ITimeCodeContentSpans. 
    /// </summary>
    public class TimeCodeToSMPTETTConverter : ITimeCodeDocumentToSMPTETTConverter
    {
        object ITimeCodeDocumentConverter.Convert(ITimeCodeDocument timeCodeDocument)
        {
            return this.Convert(timeCodeDocument as ITimeCodeDocument<string>);
        }

        /* #region Public Properties */
        public ILogger Logger { get; private set; }
        public ITextCleanerService TextCleanerService { get; private set; }
        /* #endregion Public Properties */
        /* #region Public Constructors */
        public TimeCodeToSMPTETTConverter(ILoggerFactory loggerFactory, ITextCleanerService textCleanerService)
        {
            this.Logger = loggerFactory.CreateLogger(typeof(TimeCodeToSMPTETTConverter).Name);
            this.TextCleanerService = textCleanerService;
        }

        public string GetFractionalTimecode(ITimeCode timecode)
        {
            return $"{timecode.HoursSegment.ToString("00")}:{timecode.MinutesSegment.ToString("00")}:{timecode.GetSecondsFraction().ToString("00.000")}";
        }
        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */
        public ISMPTETTResult Convert(ITimeCodeDocument<string> timeCodeDocument)
        {
            var spans = timeCodeDocument.Items;
            var smptett = new SMPTETTRoot();

            List<Paragraph> ps = new List<Paragraph>();
            foreach (var span in spans)
            {
                var p = new Paragraph();
                p.begin = this.GetFractionalTimecode(span.RecordIn);
                p.end = this.GetFractionalTimecode(span.RecordOut);

                var cleanedContent = this.TextCleanerService.CleanText(span.Content);
                p.Content = cleanedContent;
                ps.Add(p);
            }
            smptett.body.div = ps.ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SMPTETTRoot));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(nameof(Namespaces.tts), Namespaces.tts);
            ns.Add(nameof(Namespaces.ttp), Namespaces.ttp);
            ns.Add(nameof(Namespaces.ttm), Namespaces.ttm);
            //        ns.Add(nameof(Namespaces.xml), Namespaces.xml);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true;
            settings.OmitXmlDeclaration = false;
            //settings.NewLineHandling = NewLineHandling.Replace;
            //settings.NewLineChars = "<br />";

            string retvalContent;
            using (var textWriter = new Utf8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {

                    xmlSerializer.Serialize(xmlWriter, smptett, ns);
                }
                retvalContent = textWriter.ToString();
            }
            return new SMPTETTResult(retvalContent);
        }
        /* #endregion Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */
    }
}