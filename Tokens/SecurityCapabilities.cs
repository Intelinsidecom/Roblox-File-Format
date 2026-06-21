using System.Xml;

namespace RobloxFiles.Tokens
{
    public class SecurityCapabilitiesToken : IXmlPropertyToken
    {
        public string XmlPropertyToken => "SecurityCapabilities";

        public bool ReadProperty(Property prop, XmlNode node)
        {
            ulong value;
            if (ulong.TryParse(node.InnerText, out value))
            {
                prop.Value = (SecurityCapabilities)value;
                return true;
            }

            return false;
        }

        public void WriteProperty(Property prop, XmlDocument doc, XmlNode node)
        {
            var value = prop.CastValue<ulong>();
            node.InnerText = value.ToString();
        }
    }
}
