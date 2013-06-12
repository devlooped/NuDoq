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

    public class ReaderFixture
    {
        private static readonly Assembly assembly = typeof(Provider).Assembly;

        [Fact]
        public void when_reading_non_existent_xml_then_throws()
        {
            Assert.Throws<FileNotFoundException>(() => Reader.Read(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), ".xml")));
        }

        [Fact]
        public void when_xml_not_found_alongside_assembly_then_throws()
        {
            var clr = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoProject\bin\ClariusLabs.DemoProject.dll").FullName;
            var temp = Path.GetTempFileName();
            File.Copy(clr, temp, true);

            Assert.Throws<FileNotFoundException>(() => Reader.Read(Assembly.LoadFrom(temp)).Traverse().Count());
        }

        [Fact]
        public void when_reading_assemblies_from_different_platforms_then_succeeds()
        {
            var metro = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoMetro\bin\ClariusLabs.DemoMetro.dll").FullName;
            var sl = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoSilverlight\bin\ClariusLabs.DemoSilverlight.dll").FullName;
            var wp = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoPhone\bin\ClariusLabs.DemoPhone.dll").FullName;
            var clr = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoProject\bin\ClariusLabs.DemoProject.dll").FullName;

            var countMetro = new CountingVisitor();
            var countSl = new CountingVisitor();
            var countWp = new CountingVisitor();
            var countClr = new CountingVisitor();

            Reader.Read(Assembly.LoadFrom(metro)).Accept(countMetro);
            Reader.Read(Assembly.LoadFrom(sl)).Accept(countSl);
            Reader.Read(Assembly.LoadFrom(wp)).Accept(countWp);
            Reader.Read(Assembly.LoadFrom(clr)).Accept(countClr);

            Assert.Equal(countMetro.TypeCount, countClr.TypeCount);
            Assert.Equal(countSl.TypeCount, countClr.TypeCount);
            Assert.Equal(countWp.TypeCount, countClr.TypeCount);

            Assert.Equal(countMetro.ElementCount, countClr.ElementCount);
            Assert.Equal(countMetro.ElementCount, countClr.ElementCount);

            Assert.Equal(countSl.ContainerCount, countClr.ContainerCount);
            Assert.Equal(countSl.ContainerCount, countClr.ContainerCount);

            Assert.Equal(countWp.ContainerCount, countClr.ContainerCount);
            Assert.Equal(countWp.ContainerCount, countClr.ContainerCount);
        }

        [Fact]
        public void when_reading_extension_method_then_provides_typed_member()
        {
            var typed = Reader.Read(assembly)
                .Elements
                .OfType<ExtensionMethod>()
                .FirstOrDefault();

            Assert.NotNull(typed);
            Assert.NotNull(typed.Info);
        }

        [Fact]
        public void when_reading_provider_then_reads_summary()
        {
            var map = new MemberIdMap();
            map.Add(assembly);

            var id = map.FindId(typeof(Provider));
            Assert.NotNull(id);

            var member = Reader.Read(typeof(Provider).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == id);
            Assert.NotNull(member);

            var element = member.Elements.OfType<Summary>().FirstOrDefault();

            Assert.NotNull(element);
            Assert.Equal(1, element.Elements.Count());
            Assert.True(element.Elements.OfType<Text>().Any());
            Assert.Equal("Summary.", element.Elements.OfType<Text>().First().Content);
        }

        [Fact]
        public void when_reading_provider_then_reads_remarks()
        {
            var map = new MemberIdMap();
            map.Add(assembly);

            var id = map.FindId(typeof(Provider));
            Assert.NotNull(id);

            var member = Reader.Read(typeof(Provider).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == id);
            Assert.NotNull(member);

            var element = member.Elements.OfType<Remarks>().FirstOrDefault();

            Assert.NotNull(element);

            var children = element.Elements.ToList();
            Assert.Equal(4, children.Count);

            Assert.IsType<Text>(children[0]);
            Assert.IsType<Example>(children[1]);
            Assert.IsType<List>(children[2]);
            Assert.IsType<Para>(children[3]);

            Assert.Equal("Remarks.", ((Text)children[0]).Content);
            Assert.Equal("Example with code:", ((Example)children[1]).Elements.OfType<Text>().First().Content);
            Assert.Equal(@"var code = new Code();
var length = code.Length + 1;", ((Example)children[1]).Elements.OfType<Code>().First().Content);

            Assert.Equal("inline code", ((Example)children[1]).Elements.OfType<Para>().First().Elements.OfType<C>().First().Content);

            Assert.Equal("Term", ((List)children[2])
                .Elements.OfType<ListHeader>().First()
                .Elements.OfType<Term>().First().Elements.OfType<Text>().First().Content);
            Assert.Equal("Description", ((List)children[2])
                .Elements.OfType<ListHeader>().First()
                .Elements.OfType<Description>().First().Elements.OfType<Text>().First().Content);
            Assert.NotNull(((List)children[2]).Header.Term);
            Assert.NotNull(((List)children[2]).Header.Description);

            Assert.Equal("ItemTerm", ((List)children[2])
                .Elements.OfType<Item>().First()
                .Elements.OfType<Term>().First().Elements.OfType<Text>().First().Content);

            Assert.NotNull(((List)children[2]).Items.First().Term);
            Assert.NotNull(((List)children[2]).Items.First().Description);

            Assert.Equal("ItemDescription", ((List)children[2])
                .Elements.OfType<Item>().First()
                .Elements.OfType<Description>().First().Elements.OfType<Text>().First().Content);

            Assert.Equal("A ", ((Para)children[3]).Elements.OfType<Text>().First().Content);
            Assert.Equal(id, ((Para)children[3]).Elements.OfType<See>().First().Cref);
            Assert.Equal(" element.", ((Para)children[3]).Elements.OfType<Text>().Last().Content);
        }

        [Fact]
        public void when_reading_provider_then_reads_seealso()
        {
            var map = new MemberIdMap();
            map.Add(assembly);

            var providerId = map.FindId(typeof(Provider));
            Assert.NotNull(providerId);

            var member = Reader.Read(typeof(Provider).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == providerId);
            Assert.NotNull(member);

            var element = member.Elements.OfType<SeeAlso>().FirstOrDefault();

            Assert.NotNull(element);
            Assert.Equal(providerId, element.Cref);
        }

        [Fact]
        public void when_reading_provider_then_reads_exception()
        {
            var map = new MemberIdMap();
            map.Add(assembly);

            var providerId = map.FindId(typeof(Provider));
            Assert.NotNull(providerId);

            var member = Reader.Read(typeof(Provider).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == providerId);
            Assert.NotNull(member);

            var element = member.Elements.OfType<Exception>().FirstOrDefault();

            Assert.NotNull(element);
            Assert.Equal(providerId, element.Cref);
        }

        [Fact]
        public void when_reading_from_assembly_then_provides_event_info()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var providerId = map.FindId(typeof(Provider));

            var member = Reader.Read(assembly).Elements.OfType<Event>().FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(Provider).GetEvents()[0], member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Event));
        }

        [Fact]
        public void when_reading_from_assembly_then_provides_nested_type_info()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var providerId = map.FindId(typeof(Sample.NestedType));

            var member = Reader.Read(assembly).Elements.OfType<TypeDeclaration>().Where(c => c.Id == providerId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(Sample.NestedType), member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Type));
        }

        [Fact]
        public void when_reading_from_assembly_then_provides_class_info()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var providerId = map.FindId(typeof(Provider));

            var member = Reader.Read(assembly).Elements.OfType<Class>().Where(c => c.Id == providerId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(Provider), member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Class));
            Assert.True(member.Kind.HasFlag(MemberKinds.Type));
        }

        [Fact]
        public void when_reading_from_assembly_then_provides_interface_info()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var typeId = map.FindId(typeof(IProvider));

            var member = Reader.Read(assembly).Elements.OfType<Interface>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(IProvider), member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Interface));
            Assert.True(member.Kind.HasFlag(MemberKinds.Type));
        }

        [Fact]
        public void when_reading_from_assembly_then_provides_enum_info()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var typeId = map.FindId(typeof(ProviderType));

            var member = Reader.Read(assembly).Elements.OfType<Enum>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(ProviderType), member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Enum));
            Assert.True(member.Kind.HasFlag(MemberKinds.Type));
        }

        [Fact]
        public void when_reading_from_assembly_then_provides_struct_info()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var typeId = map.FindId(typeof(SampleStruct));

            var member = Reader.Read(assembly).Elements.OfType<Struct>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(SampleStruct), member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Struct));
            Assert.True(member.Kind.HasFlag(MemberKinds.Type));
        }

        [Fact]
        public void when_using_to_string_then_renders_text_content()
        {
            var xml = new FileInfo(@"..\..\..\Demo\ClariusLabs.DemoProject\ClariusLabs.DemoProject.xml").FullName;
            var members = Reader.Read(xml);
            var member = members.Elements.OfType<TypeDeclaration>().FirstOrDefault(x => x.Id == "T:ClariusLabs.Demo.SampleExtensions");

            Assert.NotNull(member);
            Assert.Equal("Extension class for ClariusLabs.Demo.Sample.", member.Elements.OfType<Summary>().First().ToString());

            member = members.Elements.OfType<TypeDeclaration>().FirstOrDefault(x => x.Id == "T:ClariusLabs.Demo.Sample");
            Assert.NotNull(member);

            var content = @"This is the remarks section, which can also have c tag code.
var code = new SomeCodeTagWithinRemarks();
You can use ClariusLabs.Demo.Provider see tag within sections.
We can have paragraphs anywhere.
"; 

            Assert.Equal(content, member.Elements.OfType<Remarks>().First().ToString());            
        }

        [Fact]
        public void when_reading_list_then_can_access_type_header_and_items()
        {
            var list = Reader.Read(assembly).Traverse().OfType<List>().Single();

            Assert.Equal(ListType.Table, list.Type);
            Assert.NotNull(list.Header);
            Assert.Equal(1, list.Items.Count());
        }

        [Fact]
        public void when_reading_unknown_list_type_then_marks_as_unknown()
        {
            var xml = Path.GetTempFileName();
            File.WriteAllText(xml, @"<doc>
    <members>
        <member name='T:ClariusLabs.Demo.Provider'>
            <summary>
                <list type='blah'>
                    <item>
                        <term>ItemTerm</term>
                        <description>ItemDescription</description>
                    </item>
                </list>
            </summary>
            <remarks>
                <list>
                    <item>
                        <term>ItemTerm</term>
                        <description>ItemDescription</description>
                    </item>
                </list>
            </remarks>
        </member>
    </members>
</doc>");

            var member = Reader.Read(xml).Elements.First();

            var list = member.Traverse().OfType<List>().First();
            Assert.Equal(ListType.Unknown, list.Type);

            list = member.Traverse().OfType<List>().Last();
            Assert.Equal(ListType.Unknown, list.Type);
        }

        [Fact]
        public void when_reading_unknown_member_then_marks_as_unknown()
        {
            var xml = Path.GetTempFileName();
            File.WriteAllText(xml, @"<doc>
    <members>
        <member name='!:Error'>
            <summary>
                blah
            </summary>
        </member>
    </members>
</doc>");

            var member = Reader.Read(xml).Elements.OfType<UnknownMember>().FirstOrDefault();

            Assert.NotNull(member);
            Assert.Equal(MemberKinds.Unknown, member.Kind);

            var visited = default(UnknownMember);
            var delegates = new VisitorDelegates { VisitUnknownMember = x => visited = x };
            member.Accept(new DelegateVisitor(delegates));

            Assert.Same(member, visited);
        }

        [Fact]
        public void when_documenting_generic_type_then_can_match_with_mapper()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var typeId = map.FindId(typeof(SampleGeneric<,>));

            var member = Reader.Read(assembly).Elements.OfType<Class>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(SampleGeneric<,>), member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Class));
            Assert.True(member.Kind.HasFlag(MemberKinds.Type));
        }

        [Fact]
        public void when_documenting_generic_type_then_parses_type_param_and_ref()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var typeId = map.FindId(typeof(SampleGeneric<,>));
            var member = Reader.Read(assembly).Elements.OfType<Class>().Where(c => c.Id == typeId).Single();

            var paramRef = member.Elements.OfType<Summary>().Single().Elements.OfType<TypeParamRef>().FirstOrDefault();

            Assert.NotNull(paramRef);
            Assert.Equal("T", paramRef.Name);

            var param = member.Elements.OfType<TypeParam>().FirstOrDefault();

            Assert.NotNull(param);
            Assert.Equal("T", param.Name);
        }

        [Fact]
        public void when_documenting_generic_method_then_sets_info()
        {
            var map = new MemberIdMap();
            map.Add(assembly);
            var typeId = map.FindId(typeof(Sample).GetMethods().Single(m => m.IsGenericMethod));
            var member = Reader.Read(assembly).Elements.OfType<Method>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
        }

        [Fact]
        public void when_documenting_generic_method_on_generic_type_then_sets_info()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));
            var typeId = map.FindId(typeof(SampleGeneric<,>).GetMethods()[0]);
            var member = Reader.Read(assembly).Elements.OfType<Method>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
        }

        [Fact]
        public void when_documenting_method_then_parses_type_param_and_ref()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));
            var typeId = map.FindId(typeof(SampleGeneric<,>).GetMethods()[0]);
            var member = Reader.Read(assembly).Elements.OfType<Method>().Where(c => c.Id == typeId).Single();

            var paramRef = member.Elements.OfType<Summary>().Single().Elements.OfType<ParamRef>().FirstOrDefault();

            Assert.NotNull(paramRef);
            Assert.Equal("func", paramRef.Name);

            var param = member.Elements.OfType<Param>().FirstOrDefault();

            Assert.NotNull(param);
            Assert.Equal("func", param.Name);
        }

        [Fact]
        public void when_parsing_empty_summary_then_removes_empty_text()
        {
            var map = new MemberIdMap();
            map.Add(typeof(IProvider));
            var typeId = map.FindId(typeof(IProvider));
            var member = Reader.Read(assembly).Elements.OfType<Interface>().Where(c => c.Id == typeId).Single();

            var element = member.Elements.OfType<Summary>().Single();
            Assert.Empty(element.ToString());
        }

        [Fact]
        public void when_parsing_empty_code_then_removes_empty_text()
        {
            var map = new MemberIdMap();
            map.Add(typeof(IProvider));
            var typeId = map.FindId(typeof(IProvider));
            var member = Reader.Read(assembly).Elements.OfType<Interface>().Where(c => c.Id == typeId).Single();

            var element = member.Elements.OfType<Remarks>().Single().Elements.OfType<Code>().Single();
            Assert.Equal(0, element.Content.Length);
        }

        [Fact]
        public void when_parsing_property_then_reads_value_tag()
        {
            var map = new MemberIdMap();
            map.Add(typeof(Sample));
            var typeId = map.FindId(typeof(Sample).GetProperties()[0]);
            var member = Reader.Read(assembly).Elements.OfType<Property>().Where(c => c.Id == typeId).Single();

            var element = member.Elements.OfType<Value>().Single();
            Assert.Equal("The id of this sample.", element.ToString());
        }

        private class CountingVisitor : Visitor
        {
            public int TypeCount { get; set; }
            public int ContainerCount { get; set; }
            public int ElementCount { get; set; }

            public override void VisitType(TypeDeclaration type)
            {
                base.VisitType(type);
                TypeCount++;
            }

            protected override void VisitContainer(Container container)
            {
                base.VisitContainer(container);
                ContainerCount++;
            }

            protected override void VisitElement(Element element)
            {
                base.VisitElement(element);
                ElementCount++;
            }
        }
    }
}