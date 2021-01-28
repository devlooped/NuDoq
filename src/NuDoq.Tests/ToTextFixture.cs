using System.IO;
using System.Linq;
using Demo;
using Xunit;

namespace NuDoq
{
    public class ToTextFixture
    {
        [Fact]
        public void when_rendering_to_string_then_renders_tag_name_for_known_elements()
        {
            DocReader.Read(typeof(IProvider).Assembly)
                .Elements
                .SelectMany(x => x.Traverse())
                .Where(x => !(x is Member || x is UnknownElement))
                .Select(x => new { Element = x, ToString = x.ToString() })
                .ToList()
                .ForEach(x => Assert.True(
                    x.ToString.StartsWith("<" + x.Element.GetType().Name.ToLowerInvariant() + ">"),
                    "Element " + x.Element.ToString() + " ToString() was expected to start with <" + x.Element.GetType().Name.ToLowerInvariant() + ">"));
        }

        [Fact]
        public void when_rendering_to_string_then_renders_tag_name_for_unknown_elements()
        {
            DocReader.Read(typeof(IProvider).Assembly)
                .Elements
                .SelectMany(x => x.Traverse())
                .OfType<UnknownElement>()
                .Select(x => new { Element = x, ToString = x.ToString() })
                .ToList()
                .ForEach(x => Assert.True(
                    x.ToString.Contains("<" + x.Element.Xml.Name.LocalName + ">"),
                    "Element " + x.Element.ToString() + " ToString() was expected to contain <" + x.Element.Xml.Name.LocalName + ">"));
        }

        [Fact]
        public void when_reading_assembly_then_to_string_renders_assembly_location()
        {
            var assembly = typeof(IProvider).Assembly;
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var members = DocReader.Read(assembly);

            Assert.Contains(assembly.Location, members.ToString());
        }

        [Fact]
        public void when_reading_xml_then_to_string_renders_xml_location()
        {
            var assembly = typeof(IProvider).Assembly;
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var members = DocReader.Read(xmlFile);

            Assert.Contains(xmlFile, members.ToString());
        }

        [Fact]
        public void when_reading_member_then_to_string_contains_member_id()
        {
            var assembly = typeof(IProvider).Assembly;
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var member = DocReader.Read(xmlFile).Elements.OfType<Member>().First();

            Assert.Contains(member.Id, member.ToString());
        }

        [Fact]
        public void when_rendering_c_then_renders_text()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleStruct));
            var id = map.FindId(typeof(SampleStruct));

            var member = DocReader.Read(typeof(SampleStruct).Assembly).Elements.OfType<Struct>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToText();
            var expected = "Sample struct.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_code_then_renders_text()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleStruct));
            var id = map.FindId(typeof(SampleStruct));

            var member = DocReader.Read(typeof(SampleStruct).Assembly).Elements.OfType<Struct>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Remarks>().First().ToText();
            var expected = @"Code:
var code = new Code();
var new = code.New();
cool!";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_paramref_then_renders_param_name()
        {
            var map = new MemberIdMap();
            map.Add(typeof(Sample));
            var id = map.FindId(typeof(Sample).GetMethod("GetValue"));

            var member = DocReader.Read(typeof(SampleStruct).Assembly).Elements.OfType<Method>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToText();
            var expected = "Gets the value for the given id.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_typeparamref_then_renders_type_param_name()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));
            var id = map.FindId(typeof(SampleGeneric<,>));

            var member = DocReader.Read(typeof(Sample).Assembly).Elements.OfType<Class>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToText();
            var expected = "Sample with generic type T.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_see_then_renders_cref()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleExtensions));
            var id = map.FindId(typeof(SampleExtensions));

            var member = DocReader.Read(typeof(SampleExtensions).Assembly).Elements.OfType<Class>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToText();
            var expected = "Extension class for Demo.Sample.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_paragraph_then_renders_wrapping_new_lines()
        {
            var map = new MemberIdMap();
            map.Add(typeof(ProviderType));
            var id = map.FindId(typeof(ProviderType));

            var member = DocReader.Read(typeof(ProviderType).Assembly).Elements.OfType<Enum>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToText();
            var expected = @"The type of provider.
With a paragraph
Or two.
And then some.";

            Assert.Equal(expected, actual);
        }
    }
}