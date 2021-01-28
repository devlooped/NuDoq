using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NuDoq
{
    public static class MyExtensions
    {
        public static string ToStringAlignAttributes(this XDocument document)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;
            var stringBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                document.WriteTo(xmlWriter);
            return stringBuilder.ToString();
        }
    }

    public static class XmlNormalizer
    {
        static class Xsi
        {
            public static XNamespace ns = "http://www.w3.org/2001/XMLSchema-instance";
            public static XName schemaLocation = ns + "schemaLocation";
            public static XName noNamespaceSchemaLocation = ns + "noNamespaceSchemaLocation";
        }

        public static XDocument Normalize(this XDocument source)
        {
            return source.Normalize(null);
        }

        public static XDocument Normalize(this XDocument source, XmlSchemaSet schema)
        {
            var havePSVI = false;
            // validate, throw errors, add PSVI information
            if (schema != null)
            {
                source.Validate(schema, null, true);
                havePSVI = true;
            }

            return new XDocument(
                source.Declaration,
                source.Nodes()
                    // Remove comments, processing instructions, and text nodes that are
                    // children of XDocument.  Only white space text nodes are allowed as
                    // children of a document, so we can remove all text nodes.
                    .Where(n => !(n is XComment || n is XProcessingInstruction || n is XText))
                    .Select(n => n is XElement ? NormalizeElement((XElement)n, havePSVI) : n));
        }

        public static bool NormalizedEquals(this XDocument doc1, XDocument doc2)
        {
            return doc1.NormalizedEquals(doc2, null);
        }

        public static bool NormalizedEquals(this XDocument doc1, XDocument doc2, XmlSchemaSet schemaSet)
        {
            var d1 = Normalize(doc1, schemaSet);
            var d2 = Normalize(doc2, schemaSet);
            return XNode.DeepEquals(d1, d2);
        }

        static IEnumerable<XAttribute> NormalizeAttributes(XElement element, bool havePSVI)
        {
            return element.Attributes()
                    .Where(a => !a.IsNamespaceDeclaration &&
                        a.Name != Xsi.schemaLocation &&
                        a.Name != Xsi.noNamespaceSchemaLocation)
                    .OrderBy(a => a.Name.NamespaceName)
                    .ThenBy(a => a.Name.LocalName)
                    .Select(
                        a =>
                        {
                            if (havePSVI)
                            {
                                var dt = a.GetSchemaInfo().SchemaType.TypeCode;
                                switch (dt)
                                {
                                    case XmlTypeCode.Boolean:
                                        return new XAttribute(a.Name, (bool)a);
                                    case XmlTypeCode.DateTime:
                                        return new XAttribute(a.Name, (DateTime)a);
                                    case XmlTypeCode.Decimal:
                                        return new XAttribute(a.Name, (decimal)a);
                                    case XmlTypeCode.Double:
                                        return new XAttribute(a.Name, (double)a);
                                    case XmlTypeCode.Float:
                                        return new XAttribute(a.Name, (float)a);
                                    case XmlTypeCode.HexBinary:
                                    case XmlTypeCode.Language:
                                        return new XAttribute(a.Name,
                                            ((string)a).ToLower());
                                }
                            }
                            return a;
                        }
                    );
        }

        static XNode NormalizeNode(XNode node, bool havePSVI)
        {
            // trim comments and processing instructions from normalized tree
            if (node is XComment || node is XProcessingInstruction)
                return null;

            var e = node as XElement;
            if (e != null)
                return NormalizeElement(e, havePSVI);
            
            // Only thing left is XCData and XText, so clone them
            return node;
        }

        static XElement NormalizeElement(XElement element, bool havePSVI)
        {
            if (havePSVI)
            {
                var dt = element.GetSchemaInfo();
                switch (dt.SchemaType.TypeCode)
                {
                    case XmlTypeCode.Boolean:
                        return new XElement(element.Name,
                            NormalizeAttributes(element, havePSVI),
                            (bool)element);
                    case XmlTypeCode.DateTime:
                        return new XElement(element.Name,
                            NormalizeAttributes(element, havePSVI),
                            (DateTime)element);
                    case XmlTypeCode.Decimal:
                        return new XElement(element.Name,
                            NormalizeAttributes(element, havePSVI),
                            (decimal)element);
                    case XmlTypeCode.Double:
                        return new XElement(element.Name,
                            NormalizeAttributes(element, havePSVI),
                            (double)element);
                    case XmlTypeCode.Float:
                        return new XElement(element.Name,
                            NormalizeAttributes(element, havePSVI),
                            (float)element);
                    case XmlTypeCode.HexBinary:
                    case XmlTypeCode.Language:
                        return new XElement(element.Name,
                            NormalizeAttributes(element, havePSVI),
                            ((string)element).ToLower());
                    default:
                        return new XElement(element.Name,
                            NormalizeAttributes(element, havePSVI),
                            element.Nodes().Select(n => NormalizeNode(n, havePSVI)).Where(n => n != null)
                        );
                }
            }
            else
            {
                return new XElement(element.Name,
                    NormalizeAttributes(element, havePSVI),
                    element.Nodes().Select(n => NormalizeNode(n, havePSVI)).Where(n => n != null)
                );
            }
        }
    }
}