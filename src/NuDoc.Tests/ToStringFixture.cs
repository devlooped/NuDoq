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
    using System.Linq;
    using ClariusLabs.Demo;
    using Xunit;

    public class ToStringFixture
    {
        [Fact]
        public void when_rendering_c_then_renders_text()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleStruct));
            var id = map.FindId(typeof(SampleStruct));

            var member = Reader.Read(typeof(SampleStruct).Assembly).Elements.OfType<Struct>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToString();
            var expected = "Sample struct.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_code_then_renders_text()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleStruct));
            var id = map.FindId(typeof(SampleStruct));

            var member = Reader.Read(typeof(SampleStruct).Assembly).Elements.OfType<Struct>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Remarks>().First().ToString();
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

            var member = Reader.Read(typeof(SampleStruct).Assembly).Elements.OfType<Method>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToString();
            var expected = "Gets the value for the given id.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_typeparamref_then_renders_type_param_name()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));
            var id = map.FindId(typeof(SampleGeneric<,>));

            var member = Reader.Read(typeof(Sample).Assembly).Elements.OfType<Class>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToString();
            var expected = "Sample with generic type T.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_see_then_renders_cref()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleExtensions));
            var id = map.FindId(typeof(SampleExtensions));

            var member = Reader.Read(typeof(SampleExtensions).Assembly).Elements.OfType<Class>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToString();
            var expected = "Extension class for ClariusLabs.Demo.Sample.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void when_rendering_paragram_then_renders_wrapping_new_lines()
        {
            var map = new MemberIdMap();
            map.Add(typeof(ProviderType));
            var id = map.FindId(typeof(ProviderType));

            var member = Reader.Read(typeof(ProviderType).Assembly).Elements.OfType<Enum>().Single(x => x.Id == id);

            var actual = member.Elements.OfType<Summary>().First().ToString();
            var expected = @"The type of provider.
With a paragraph
Or two.
And then some.";

            Assert.Equal(expected, actual);
        }
    }
}