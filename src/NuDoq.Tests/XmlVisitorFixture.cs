using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Demo;
using Xunit;

namespace NuDoq
{
    public class XmlVisitorFixture
    {
        [Fact]
        public void when_visiting_xml_then_adds_source_assembly()
        {
            var member = DocReader.Read(typeof(ProviderType).Assembly);
            var visitor = new XmlVisitor();

            member.Accept(visitor);

            Assert.Equal("doc", visitor.Xml.Root.Name);
            Assert.Equal("assembly", visitor.Xml.Root.Elements().First().Name);
            Assert.Equal(typeof(ProviderType).Assembly.GetName().Name, visitor.Xml.Root.Elements().First().Elements().First().Value);

            //WriteXml(visitor.Xml);
        }

        [Fact]
        public void when_visiting_xml_then_adds_members()
        {
            var member = DocReader.Read(Path.ChangeExtension(typeof(ProviderType).Assembly.Location, ".xml"));
            var visitor = new XmlVisitor();

            member.Accept(visitor);

            Assert.Equal("doc", visitor.Xml.Root.Name);
            Assert.Equal("members", visitor.Xml.Root.Elements().First().Name);

            //WriteXml(visitor.Xml);
        }

        [Fact(Skip = "We do not support roundtriping for now.")]
        public void when_writing_xml_then_can_roundtrip()
        {
            var originalXml = XDocument.Load(Path.ChangeExtension(typeof(ProviderType).Assembly.Location, ".xml"));
            var member = DocReader.Read(typeof(ProviderType).Assembly);
            var visitor = new XmlVisitor();

            member.Accept(visitor);

            //WriteXml(originalXml.Normalize(), "C:\\Temp\\source.xml");
            //WriteXml(visitor.Xml.Normalize(), "C:\\Temp\\target.xml");

            //Assert.True(originalXml.NormalizedEquals(visitor.Xml));
        }

        static void WriteXml(XDocument xml)
        {
            using (var writer = XmlWriter.Create(Console.Out, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true }))
            {
                xml.WriteTo(writer);
            }
        }

        static void WriteXml(XDocument xml, string file)
        {
            using (var writer = XmlWriter.Create(file, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true }))
            {
                xml.WriteTo(writer);
            }
        }
    }
}