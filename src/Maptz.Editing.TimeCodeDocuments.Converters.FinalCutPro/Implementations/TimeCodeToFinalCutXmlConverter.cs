using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro.Xml;
using Maptz.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro
{

    /// <summary>
    /// A converter used to create Avid DS files from groups of ITimeCodeContentSpans. 
    /// </summary>
    public class TimeCodeToFinalCutXmlConverter : ITimeCodeDocumentToFinalCutXMLConverter
    {
        object ITimeCodeDocumentConverter.Convert(ITimeCodeDocument timeCodeDocument)
        {
            return this.Convert(timeCodeDocument as ITimeCodeDocument<string>);
        }

        /* #region Public Properties */
        public ILogger Logger
        {
            get;
            private set;
        }

        public ITextCleanerService TextCleanerService
        {
            get;
            private set;
        }

        public TimeCodeToFinalCutXmlConverterSettings Settings
        {
            get;
        }

        /* #endregion Public Properties */
        /* #region Public Constructors */
        public TimeCodeToFinalCutXmlConverter(ILoggerFactory loggerFactory, ITextCleanerService textCleanerService, IOptions<TimeCodeToFinalCutXmlConverterSettings> settings)
        {
            this.TextCleanerService = textCleanerService;
            this.Settings = settings.Value;
            this.LoggerFactory = loggerFactory;
            this.Logger = this.LoggerFactory.CreateLogger(this.GetType().Name);
        }

        public string GetFractionalTimecode(ITimeCode timecode)
        {
            return $"{timecode.HoursSegment.ToString("00")}:{timecode.MinutesSegment.ToString("00")}:{timecode.GetSecondsFraction().ToString("00.000")}";
        }

        /* #endregion Public Constructors */
        /* #region Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */
        public IFinalCutXMLResult Convert(ITimeCodeDocument<string> timeCodeDocument)
        {
            var spans = timeCodeDocument.Items;
            var xmeml = new xmeml();
            var start = new TimeCode();
            var last = spans.Last();
            var durationFrames = last.RecordOut.TotalFrames - start.TotalFrames;
            xmeml.sequence.id = this.Settings.SequenceName;
            xmeml.sequence.name = this.Settings.SequenceName;
            xmeml.sequence.duration = (int)durationFrames;
            xmeml.sequence.timecode.@string = start.ToString();
            xmeml.sequence.timecode.frame = (int)start.TotalFrames;
            xmeml.sequence.media.video.format.samplecharacteristics.width = this.Settings.Width;
            xmeml.sequence.media.video.format.samplecharacteristics.height = this.Settings.Height;
            var track = new Track();
            xmeml.sequence.media.video.tracks = new Track[] { track };
            var generatorItems = new List<GeneratorItem>();
            for (int i = 0; i < spans.Count(); i++)
            {
                //Alignment is missing a valuelist!!!
                var span = spans.ElementAt(i);
                var genI = new GeneratorItem();
                generatorItems.Add(genI);
                genI.id = $"Text{(i + 1).ToString()}";
                genI.duration = (int)(span.RecordOut.TotalFrames - span.RecordIn.TotalFrames);
                genI.@in = 0;
                genI.@out = 0;
                genI.start = (int)span.RecordIn.TotalFrames;
                genI.end = (int)span.RecordOut.TotalFrames;
                var cleanedContent = this.TextCleanerService.CleanText(span.Content);
                cleanedContent = cleanedContent.Replace(Environment.NewLine, "&#13;");
                /* #regaion Parameters */
                var parameters = new List<Parameter>();
                {
                    parameters.Add(new Parameter()
                    { parameterid = "str", name = "Text", value = cleanedContent });
                    parameters.Add(new Parameter()
                    { parameterid = "fontname", name = "Font", value = "Arial" });
                    parameters.Add(new Parameter()
                    { parameterid = "fontsize", name = "Size", valuemin = 0, valuemax = 1000, value = this.Settings.FontSize });
                    /* #region font style */
                    var fontstyleParm = new Parameter()
                    { parameterid = "fontstyle", name = "Style", valuemin = 1, valuemax = 4, value = "0" };
                    fontstyleParm.valuelist = new ValueEntry[]{new ValueEntry()
                    {name = "Plain", value = "1"}, new ValueEntry()
                    {name = "Bold", value = "2"}, new ValueEntry()
                    {name = "Italic", value = "3"}, new ValueEntry()
                    {name = "Bold/Italic", value = "4"}, };
                    parameters.Add(fontstyleParm);
                    /* #endregion*/
                    /* #region fontalign */
                    var fontalignParm = new Parameter()
                    { parameterid = "fontalign", name = "Alignment", valuemin = 1, valuemax = 3, value = "2" };
                    fontstyleParm.valuelist = new ValueEntry[]{new ValueEntry()
                    {name = "Left", value = "1"}, new ValueEntry()
                    {name = "Center", value = "2"}, new ValueEntry()
                    {name = "Right", value = "3"}, };
                    parameters.Add(fontalignParm);
                    /* #endregion*/
                    parameters.Add(new Parameter()
                    { parameterid = "fontcolor", name = "Font Color", value = new ColorF() });
                    parameters.Add(new Parameter()
                    { parameterid = "origin", name = "Origin", value = new Origin() });
                    parameters.Add(new Parameter()
                    { parameterid = "fonttrack", name = "Tracking", valuemin = -200, valuemax = 200, value = 1 });
                    parameters.Add(new Parameter()
                    { parameterid = "leading", name = "Leading", valuemin = -100, valuemax = 100, value = 0 });
                    parameters.Add(new Parameter()
                    { parameterid = "aspect", name = "Aspect", valuemin = 0.1, valuemax = 5, value = 1 });
                    parameters.Add(new Parameter()
                    { parameterid = "autokern", name = "Auto Kerning", value = "TRUE" });
                    parameters.Add(new Parameter()
                    { parameterid = "subpixel", name = "Use Subpixel", value = "TRUE" });
                }

                genI.effect.parameters = parameters.ToArray();
                /* #endregion*/
                /* #region Filters */
                var filters = new List<Filter>();
                {
                    /* #region Basic Motion */
                    filters.Add(new Filter()
                    {
                        enabled = null,
                        effect = new Effect()
                        {
                            name = "Basic Motion",
                            effectid = "basic",
                            effectcategory = "motion",
                            effecttype = "motion",
                            mediatype = "video",
                            parameters = new Parameter[]{new Parameter()
                    {parameterid = "center", name = "Center", value = new Origin()
                    {horiz = 0, vert = 0}}, new Parameter()
                    {parameterid = "scale", name = "Scale", valuemin = 0, valuemax = 1000, value = 100}, new Parameter()
                    {parameterid = "rotation", name = "Rotation", valuemin = -8640, valuemax = 8640, value = 0}, new Parameter()
                    {parameterid = "centerOffset", name = "Anchor Point", value = new Origin()
                    {horiz = 0, vert = 0}}}
                        }
                    });
                    /* #endregion*/
                    /* #region Drop Shadow */
                    filters.Add(new Filter()
                    {
                        enabled = "TRUE",
                        effect = new Effect()
                        {
                            name = "Drop Shadow",
                            effectid = "dropshadow",
                            effectcategory = "motion",
                            effecttype = "motion",
                            mediatype = "video",
                            parameters = new Parameter[]{new Parameter()
                    {parameterid = "offset", name = "offset", valuemin = -100, valuemax = 100, value = 1}, new Parameter()
                    {parameterid = "angle", name = "angle", valuemin = -720, valuemax = 720, value = 135}, new Parameter()
                    {parameterid = "color", name = "color", value = new ColorF()
                    {alpha = 0, red = 0, green = 0, blue = 0}}, new Parameter()
                    {parameterid = "softness", name = "softness", valuemin = 0, valuemax = 100, value = 100}, new Parameter()
                    {parameterid = "opacity", name = "opacity", valuemin = 0, valuemax = 100, value = 100}}
                        }
                    });
                    /* #endregion*/
                    /* #region Motion Blur */
                    filters.Add(new Filter()
                    {
                        enabled = "FALSE",
                        effect = new Effect()
                        {
                            name = "Motion Blur",
                            effectid = "motionblur",
                            effecttype = "motion",
                            mediatype = "video",
                            parameters = new Parameter[]{new Parameter()
                    {parameterid = "duration", name = "% Blur", valuemin = 0, valuemax = 1000, value = 500}, new Parameter()
                    {parameterid = "samples", name = "Samples", valuemin = 1, valuemax = 16, value = 4}}
                        }
                    });
                    /* #endregion*/
                    /* #region Crop */
                    filters.Add(new Filter()
                    {
                        enabled = null,
                        effect = new Effect()
                        {
                            name = "Crop",
                            effectid = "crop",
                            effectcategory = "motion",
                            effecttype = "motion",
                            parameters = new Parameter[]{new Parameter()
                    {parameterid = "left", name = "left", valuemin = 0, valuemax = 100, value = 0}, new Parameter()
                    {parameterid = "right", name = "right", valuemin = 0, valuemax = 100, value = 0}, new Parameter()
                    {parameterid = "top", name = "top", valuemin = 0, valuemax = 100, value = 0}, new Parameter()
                    {parameterid = "bottom", name = "bottom", valuemin = 0, valuemax = 100, value = 0}}
                        }
                    });
                    /* #endregion*/
                    /* #region Distort */
                    filters.Add(new Filter()
                    {
                        enabled = "TRUE",
                        effect = new Effect()
                        {
                            name = "Distort",
                            effectid = "deformation",
                            effectcategory = "motion",
                            effecttype = "motion",
                            mediatype = "video",
                            parameters = new Parameter[]{new Parameter()
                    {parameterid = "ulcorner", name = "Upper Left", value = new Origin()
                    {horiz = -0.5, vert = -0.5}}, new Parameter()
                    {parameterid = "urcorner", name = "Upper Right", value = new Origin()
                    {horiz = 0.5, vert = -0.5}}, new Parameter()
                    {parameterid = "lrcorner", name = "Lower Right", value = new Origin()
                    {horiz = 0.5, vert = 0.5}}, new Parameter()
                    {parameterid = "llcorner", name = "Lower Left", value = new Origin()
                    {horiz = -0.5, vert = -0.5}}, new Parameter()
                    {parameterid = "aspect", name = "Aspect", valuemin = -10000, valuemax = 10000, value = 0}}
                        }
                    });
                    /* #endregion*/
                    /* #region Opacity */
                    filters.Add(new Filter()
                    {
                        enabled = null,
                        effect = new Effect()
                        {
                            name = "Opacity",
                            effectid = "opacity",
                            effectcategory = "motion",
                            effecttype = "motion",
                            mediatype = "video",
                            parameters = new Parameter[]{new Parameter()
                    {parameterid = "opacity", name = "opacity", valuemin = 0, valuemax = 100, value = 100}}
                        }
                    });
                    /* #endregion*/
                }

                genI.filters = filters.ToArray();
                /* #endregion*/
            }

            track.generatorItems = generatorItems.ToArray();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(xmeml));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true;
            settings.OmitXmlDeclaration = false;
            string retvalContent;
            using (var textWriter = new Utf8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    xmlSerializer.Serialize(xmlWriter, xmeml, ns);
                }

                retvalContent = textWriter.ToString();
            }

            return new FinalCutXMLResult(retvalContent);
        }

        public ILoggerFactory LoggerFactory
        {
            get;
        }
        /* #endregion Interface: 'Maptz.Avid.Ds.ITimeCodeContentSpansStringConverter<string>' Methods */
    }
}