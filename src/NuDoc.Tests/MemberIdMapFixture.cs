#region Apache Licensed
/*
 Copyright 2013 Clarius Consulting, Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

namespace ClariusLabs.NuDoc
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ClariusLabs.Demo;
    using Xunit;

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
        public void when_mapping_generic_type_then_matches_xml_format()
        {
            var expected = "T:ClariusLabs.Demo.SampleGeneric`2";
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));

            var actual = map.FindId(typeof(SampleGeneric<,>));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_mapping_generic_method_on_non_generic_type_then_matches_xml_format()
        {
            var expected = "M:ClariusLabs.Demo.Sample.Do``1(System.Func{``0})";
            var map = new MemberIdMap();
            map.Add(typeof(Sample));

            var actual = map.FindId(typeof(Sample).GetMethods().Single(m => m.IsGenericMethod));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_mapping_generic_method_on_generic_type_then_matches_xml_format()
        {
            var expected = "M:ClariusLabs.Demo.SampleGeneric`2.Do``1(System.Func{`0,`1,``0})";
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));

            var actual = map.FindId(typeof(SampleGeneric<,>).GetMethods().Single(m => m.IsGenericMethod));

            Assert.Equal(expected, actual);
        }
    }
}