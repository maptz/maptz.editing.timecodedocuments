using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Maptz.Editing.TimeCodeDocuments.Converters.FinalCutPro.Xml
{

    public class Sequence
    {
        [XmlAttribute]
        public string id { get; set; }
        public string updatebehavior { get; set; } = "add";
        public string uuid { get; set; } = "ce536d85-eeb3-45e7-bc21-773c08f12ce9";// Guid.NewGuid().ToString();

        public string name { get; set; }
        public int duration { get; set; }

        public Rate rate { get; set; } = new Rate();
        public TimeCodeF timecode { get; set; } = new TimeCodeF();

        public int @in { get; set; } = 0;
        public int @out { get; set; } = 0;
        public Media media { get; set; } = new Media();
        public string ismasterclip { get; set; } = "FALSE";
    }

    public class Media
    {
        public Video video { get; set; } = new Video();
    }

    public class Video
    {
        public Format format { get; set; } = new Format();
        [XmlElement("track")]
        public Track[] tracks { get; set; } = new Track[0];
    }

    [XmlType("track")]
    public class Track
    {
        [XmlElement("generatoritem")]
        public GeneratorItem[] generatorItems { get; set; } = new GeneratorItem[0];

        public string enabled { get; set; } = "TRUE";
        public string locked { get; set; } = "TRUE";
    }

    public class Format
    {
        public SampleCharactersitics samplecharacteristics { get; set; } = new SampleCharactersitics();
    }
    public class SampleCharactersitics
    {
        public int width { get; set; } = 1920;
        public int height { get; set; } = 1080;
        public string anamorphic { get; set; } = "FALSE";
        public string pixelaspectratio { get; set; } = "square";
        public string fielddominance { get; set; } = "none";
        public Rate rate { get; set; } = new Rate();
        public int colordepth { get; set; } = 24;

        public Codec codec { get; set; }
    }

    public class Codec
    {
        public string name { get; set; } = "Apple DVCPRO HD 720p50";
    }

    [XmlType(Namespace = "")]
    public class TimeCodeF
    {
        public Rate rate { get; set; } = new Rate();
        public string @string { get; set; } = "01:00:00:00";
        public int frame { get; set; } = 90000;
        public string source { get; set; } = "source";
        public string displayformat = "NDF";
    }



    public class Rate
    {

        public string ntsc { get; set; } = "FALSE";
        public string timebase { get; set; } = "25";
    }


    [XmlInclude(typeof(Origin))]
    [XmlInclude(typeof(ColorF))]
    [XmlRoot(ElementName = "xmeml", Namespace = "")]
    public class xmeml
    {
        

        [XmlAttribute]
        public int version { get; set; } = 5;

        public Sequence sequence { get; set; } = new Sequence();
    }

    public class LoggingInfo
    {
        /* #region Public Properties */
        public string good { get; set; } = "FALSE";
        public string lognote { get; set; }
        public string scene { get; set; }
        public string shottake { get; set; }
        /* #endregion Public Properties */
    }
    public class Labels
    {
        /* #region Public Properties */
        public string label { get; set; } = "Good take";
        public string label2 { get; set; }
        /* #endregion Public Properties */
    }

    public class comments
    {
        /* #region Public Properties */
        public string mastercomment1 { get; set; }
        public string mastercomment2 { get; set; }
        public string mastercomment3 { get; set; }
        public string mastercomment4 { get; set; }
        /* #endregion Public Properties */
    }

    [XmlType("generatoritem")]
    public class GeneratorItem
    {
        /* #region Public Properties */
        public string alphatype { get; set; } = "black";
        public string anamorphic { get; set; } = "FALSE";
        public int duration { get; set; } = 0;
        public Effect effect { get; set; } = new Effect();
        public string enabled { get; set; } = "TRUE";
        public int end { get; set; } = 0;
        [XmlElement("filter")]
        public Filter[] filters { get; set; } = new Filter[0];
        [XmlAttribute]
        public string id { get; set; } = "Text";
        public int @in { get; set; } = 0;
        public int @out { get; set; } = 0;
        public ItemHistory itemhistory { get; set; } = new ItemHistory();
        public Labels labels { get; set; } = new Labels();
        public LoggingInfo logginginfo { get; set; } = new LoggingInfo();
        public string name { get; set; } = "Text";
        public Rate rate { get; set; } = new Rate();
        public SourceTrack sourcetrack { get; set; } = new SourceTrack();
        public int start { get; set; } = 0;
        /* #endregion Public Properties */
    }

    public class ItemHistory
    {
        /* #region Public Properties */
        public string uuid { get; set; }
        /* #endregion Public Properties */
    }
    public class SourceTrack
    {
        /* #region Public Properties */
        public string mediatype { get; set; } = "video";
        /* #endregion Public Properties */
    }

    [XmlRoot("value")]
    public class ColorF
    {
        public int alpha { get; set; } = 255;
        public int red { get; set; } = 255;
        public int green { get; set; } = 255;
        public int blue { get; set; } = 255;
    }

    [XmlRoot("value")]
    public class Origin
    {
        public double horiz { get; set; } = 0;
        public double vert { get; set; } = 0.34375;
    }



    public class Parameter : IXmlSerializable
    {
        /* #region Public Properties */
        public string name { get; set; } = "Text";
        public string parameterid { get; set; } = "str";

        public object value { get; set; } = "Some text goes here";

        [XmlArray("valuelist")]
        public ValueEntry[] valuelist { get; set; }
        public double? valuemax { get; set; }
        public double? valuemin { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(name), this.name);
            writer.WriteElementString(nameof(parameterid), this.parameterid);
            if (this.valuemax.HasValue)
            {
                writer.WriteElementString(nameof(valuemax), this.valuemax.ToString());
            }
            if (this.valuemin.HasValue)
            {
                writer.WriteElementString(nameof(valuemin), this.valuemin.ToString());
            }
            if (this.valuelist != null)
            {
                writer.WriteStartElement("valuelist");
                foreach(var valueentry in this.valuelist)
                {
                    // write xml decl and root elts here
                    var s = new XmlSerializer(typeof(ValueEntry));
                    s.Serialize(writer, valueentry);
                }
                writer.WriteEndElement();
            }
            if (this.value != null)
            {
                if (value is string)
                {
                    writer.WriteStartElement(nameof(value));
                    writer.WriteRaw(value as string);
                    writer.WriteEndElement();
                }
                else if (value is int)
                {
                    writer.WriteElementString(nameof(value), ((int)value).ToString());
                }
                else if (value is double)
                {
                    writer.WriteElementString(nameof(value), ((double)value).ToString());
                }
                else if (value is Origin)
                {
                    //writer.WriteStartElement(nameof(value));
                    var s = new XmlSerializer(typeof(Origin));
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");
                    s.Serialize(writer, value as Origin, namespaces);
                    //writer.WriteEndElement();
                }
                else if (value is ColorF)
                {
                    //writer.WriteStartElement(nameof(value));
                    var s = new XmlSerializer(typeof(ColorF));
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");
                    s.Serialize(writer, value as ColorF, namespaces);
                    //writer.WriteEndElement();
                }
                else throw new NotSupportedException();
            }
        }
        /* #endregion Public Properties */
    }

    [XmlType("valueentry")]
    public class ValueEntry
    {
        /* #region Public Properties */
        public string name { get; set; }
        public string value { get; set; }
        /* #endregion Public Properties */
    }

    public class Effect  // : IXmlSerializable
    {
        /* #region Public Properties */
        public string effectcategory { get; set; } = "Text";
        public string effectid { get; set; } = "Text";
        public string effecttype { get; set; } = "generator";
        public string mediatype { get; set; } = "video";
        public string name { get; set; } = "Text";

        [XmlElement("parameter")]
        public Parameter[] parameters { get; set; } = new Parameter[0];

        //public XmlSchema GetSchema()
        //{
        //    return null;
        //}

        //public void ReadXml(XmlReader reader)
        //{
        //    throw new NotImplementedException();
        //}

        //public void WriteXml(XmlWriter writer)
        //{

        //    writer.WriteElementString(nameof(effectcategory), this.effectcategory);
        //    writer.WriteElementString(nameof(effectid), this.effectid);
        //    writer.WriteElementString(nameof(effecttype), this.effecttype);
        //    writer.WriteElementString(nameof(mediatype), this.mediatype);
        //    writer.WriteElementString(nameof(name), this.name);
        //    foreach (var parameter in parameters)
        //    {
        //        writer.WriteStartElement("parameter");
        //        parameter.WriteXml(writer);
        //        writer.WriteEndElement();
        //    }
        //}
        ///* #endregion Public Properties */
    }

    public class Filter
    {
        public string enabled { get; set; } = "TRUE";
        /* #region Public Properties */
        public Effect effect { get; set; } = new Effect();
        /* #endregion Public Properties */
    }


}