using System.Linq;
using System.Reflection;
using Demo;
using Xunit;

namespace NuDoq
{
    public class MemberIdMapFixture
    {
        [Fact]
        public void when_adding_assembly_then_adds_all_ids_from_used_types()
        {
            var map = new MemberIdMap();
            map.Add(typeof(Sample).Assembly);

            Assert.NotNull(map.FindId(typeof(Provider)));
            Assert.NotNull(map.FindId(typeof(Provider).GetConstructors()[0]));
            Assert.NotNull(map.FindId(typeof(SampleExtensions)));
            Assert.NotNull(map.FindId(Reflect.GetMethod(() => SampleExtensions.Do(null))));

            Assert.NotNull(map.FindId(typeof(Sample)));
            Assert.NotNull(map.FindId(typeof(Sample).GetConstructors()[0]));
            Assert.NotNull(map.FindId(Reflect<Sample>.GetMethod(x => x.GetValue(0))));
            Assert.NotNull(map.FindId(Reflect<Sample>.GetProperty(x => x.Id)));

            Assert.NotNull(map.FindId(typeof(Sample.NestedType)));
            Assert.NotNull(map.FindId(typeof(Sample.NestedType).GetConstructors()[0]));
            Assert.NotNull(map.FindId(Reflect<Sample.NestedType>.GetProperty(x => x.NestedTypeProperty)));
            Assert.NotNull(map.FindId(typeof(Sample).GetNestedTypes(BindingFlags.NonPublic).Single()));
        }

        [Fact]
        public void when_mapping_type_then_can_retrieve_from_ids_or_members()
        {
            var map = new MemberIdMap();
            map.Add(typeof(Sample));

            var id = map.FindId(typeof(Sample));

            Assert.True(map.Ids.Contains(id));
            Assert.True(map.Members.Contains(typeof(Sample)));
        }

        [Fact]
        public void when_mapping_generic_type_then_matches_xml_format()
        {
            var expected = "T:Demo.SampleGeneric`2";
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));

            var actual = map.FindId(typeof(SampleGeneric<,>));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_mapping_generic_method_on_non_generic_type_then_matches_xml_format()
        {
            var expected = "M:Demo.Sample.Do``1(System.Func{``0})";
            var map = new MemberIdMap();
            map.Add(typeof(Sample));

            var actual = map.FindId(typeof(Sample).GetMethods().Single(m => m.IsGenericMethod));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_mapping_generic_method_on_generic_type_then_matches_xml_format()
        {
            var expected = "M:Demo.SampleGeneric`2.Do``1(System.Func{`0,`1,``0})";
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));

            var actual = map.FindId(typeof(SampleGeneric<,>).GetMethods().Single(m => m.IsGenericMethod));

            Assert.Equal(expected, actual);
        }
    }
}