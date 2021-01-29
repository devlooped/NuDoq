using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Demo;
using Xunit;
using Xunit.Abstractions;

namespace NuDoq
{
    public class XmlVisitorFixture
    {
        readonly ITestOutputHelper output;

        public XmlVisitorFixture(ITestOutputHelper output) => this.output = output;

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

        [Fact]
        public void when_reading_custom_xml_then_document_preserves_element_and_attributes()
        {
            var members = DocReader.Read(Assembly.GetExecutingAssembly(), new ReaderOptions { KeepNewLinesInText = true });

            var xml = members.Accept(new XmlVisitor()).Xml;

            var summary = xml.Root
                .Element("members")
                .Elements("member")
                .First(x => x.Attribute("name").Value == "T:NuDoq.CustomXml")
                .Element("summary");

            Assert.NotNull(summary.Element("custom"));
            Assert.Equal("value", summary.Element("custom").Attribute("id").Value);

            var remarks = xml.Root
                .Element("members")
                .Elements("member")
                .First(x => x.Attribute("name").Value == "T:NuDoq.CustomXml")
                .Element("remarks");

            Assert.Equal("foo.cs", remarks.Element("code").Attribute("source").Value);
            Assert.Equal("example", remarks.Element("code").Attribute("region").Value);
        }

        void WriteXml(XDocument xml)
        {
            using var writer = XmlWriter.Create(new TestOutputTextWriter(output), new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true });
            xml.WriteTo(writer);
        }

        static void WriteXml(XDocument xml, string file)
        {
            using var writer = XmlWriter.Create(file, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true });
            xml.WriteTo(writer);
        }
    }
}