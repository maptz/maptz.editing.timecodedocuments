using System.Xml;
using System.Xml.Serialization;
namespace Maptz.Editing.TimeCodeDocuments.Converters.SMPTETT.Xml
{

    [XmlRootAttribute("tt", Namespace = "http://www.w3.org/2006/10/ttaf1")]
    public class SMPTETTRoot
    {
        [XmlAttribute(Namespace = Namespaces.xml)]
        public string lang { get; set; } = "en";
        [XmlAttribute(Namespace = Namespaces.ttp)]
        public string timeBase { get; set; } = "media";
        public Head head { get; set; } = new Head();
        public Body body { get; set; } = new Body();

    }
}