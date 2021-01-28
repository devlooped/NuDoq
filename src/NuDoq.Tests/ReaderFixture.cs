using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Demo;
using Xunit;

namespace NuDoq
{
    public class ReaderFixture
    {
        static readonly Assembly assembly = typeof(Provider).Assembly;

        [Fact]
        public void when_reading_non_existent_xml_then_throws()
        {
            Assert.Throws<FileNotFoundException>(() => DocReader.Read(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), ".xml")));
        }

        [Fact(Skip = "If this test runs before the next, the LoadFrom context will keep the temp-loaded assembly and cause it to fail.")]
        public void when_xml_not_found_alongside_assembly_then_throws()
        {
            var clr = new FileInfo(@"DemoProject.dll").FullName;
            var temp = Path.GetTempFileName();
            File.Copy(clr, temp, true);

            Assert.Throws<FileNotFoundException>(() => DocReader.Read(Assembly.LoadFrom(temp)).Traverse().Count());
        }

        [Fact]
        public void when_reading_assembly_then_succeeds()
        {
            var clr = new FileInfo(@"DemoProject.dll").FullName;

            var countClr = new CountingVisitor("NuDoq-net");

            DocReader.Read(Assembly.LoadFrom(clr)).Accept(countClr);
        }

        [Fact]
        public void when_reading_assembly_then_can_access_source_assembly()
        {
            var members = DocReader.Read(assembly);

            Assert.Same(assembly, members.Assembly);
        }

        [Fact]
        public void when_reading_assembly_then_can_access_source_document()
        {
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var members = DocReader.Read(assembly);

            Assert.NotNull(members.Xml);
            Assert.Equal(xmlFile, new Uri(members.Xml.BaseUri).LocalPath);
        }

        [Fact]
        public void when_reading_xml_then_can_access_source_document()
        {
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var members = DocReader.Read(xmlFile);

            Assert.NotNull(members.Xml);
            Assert.Equal(xmlFile, new Uri(members.Xml.BaseUri).LocalPath);
        }

        [Fact]
        public void when_reading_element_then_can_access_xml_line_info()
        {
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var member = DocReader.Read(xmlFile).Elements.SelectMany(x => x.Traverse()).OfType<Summary>().First();

            var lineInfo = member as IXmlLineInfo;

            Assert.NotNull(lineInfo);
            Assert.True(lineInfo.HasLineInfo());
            Assert.NotEqual(0, lineInfo.LineNumber);
            Assert.NotEqual(0, lineInfo.LinePosition);
        }

        [Fact]
        public void when_reading_assembly_then_can_access_id_map()
        {
            var members = DocReader.Read(assembly);

            Assert.NotNull(members.IdMap);
            Assert.NotNull(members.IdMap.FindId(typeof(IProvider)));
        }

        [Fact]
        public void when_reading_assembly_then_visits_assembly_and_document()
        {
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var members = DocReader.Read(assembly);
            AssemblyMembers asmMembers = null;
            DocumentMembers docMembers = null;

            members.Accept(new DelegateVisitor(new VisitorDelegates
            {
                VisitAssembly = asm => asmMembers = asm,
                VisitDocument = doc => docMembers = doc,
            }));

            Assert.NotNull(asmMembers);
            Assert.NotNull(docMembers);
        }

        [Fact]
        public void when_reading_document_then_visits_document()
        {
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var members = DocReader.Read(xmlFile);
            DocumentMembers docMembers = null;

            members.Accept(new DelegateVisitor(new VisitorDelegates
            {
                VisitDocument = doc => docMembers = doc,
            }));

            Assert.NotNull(docMembers);
        }

        [Fact]
        public void when_reading_xml_then_visits_document()
        {
            var xmlFile = Path.ChangeExtension(assembly.Location, ".xml");
            var members = DocReader.Read(xmlFile);

            Assert.NotNull(members.Xml);
            Assert.Equal(xmlFile, new Uri(members.Xml.BaseUri).LocalPath);
        }

        [Fact]
        public void when_reading_extension_method_then_provides_typed_member()
        {
            var typed = DocReader.Read(assembly)
                .Elements
                .OfType<ExtensionMethod>()
                .FirstOrDefault();

            Assert.NotNull(typed);
            Assert.NotNull(typed.Info);
        }

        [Fact]
        public void when_reading_provider_type_then_reads_summary()
        {
            var map = new MemberIdMap();
            map.Add(assembly);

            var id = map.FindId(typeof(ProviderType));
            Assert.NotNull(id);

            var member = DocReader.Read(typeof(ProviderType).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == id);
            Assert.NotNull(member);

            var element = member.Elements.OfType<Summary>().FirstOrDefault();

            Assert.NotNull(element);
            Assert.Equal(4, element.Elements.Count());
            Assert.True(element.Elements.OfType<Text>().Any());
            Assert.Equal("The type of provider.", element.Elements.OfType<Text>().First().Content);
            Assert.Equal("And then some.", element.Elements.OfType<Text>().Last().Content);
        }

        /// <summary>
        /// When_reading_provider_then_reads_remarkses this instance.
        /// </summary>
        [Fact]
        public void when_reading_provider_then_reads_remarks()
        {
            var map = new MemberIdMap();
            map.Add(assembly);

            var id = map.FindId(typeof(Provider));
            Assert.NotNull(id);

            var member = DocReader.Read(typeof(Provider).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == id);
            Assert.NotNull(member);

            var element = member.Elements.OfType<Remarks>().FirstOrDefault();

            Assert.NotNull(element);

            var children = element.Elements.ToList();
            Assert.Equal(3, children.Count);

            Assert.IsType<Text>(children[0]);
            Assert.IsType<Example>(children[1]);
            Assert.IsType<List>(children[2]);

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
        }

        [Fact]
        public void when_reading_provider_then_reads_seealso()
        {
            var map = new MemberIdMap();
            map.Add(assembly);

            var providerId = map.FindId(typeof(Provider));
            Assert.NotNull(providerId);

            var member = DocReader.Read(typeof(Provider).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == providerId);
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

            var member = DocReader.Read(typeof(Provider).Assembly).Elements.OfType<Member>().FirstOrDefault(x => x.Id == providerId);
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

            var member = DocReader.Read(assembly).Elements.OfType<Event>().FirstOrDefault();

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

            var member = DocReader.Read(assembly).Elements.OfType<TypeDeclaration>().Where(c => c.Id == providerId).FirstOrDefault();

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

            var member = DocReader.Read(assembly).Elements.OfType<Class>().Where(c => c.Id == providerId).FirstOrDefault();

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

            var member = DocReader.Read(assembly).Elements.OfType<Interface>().Where(c => c.Id == typeId).FirstOrDefault();

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

            var member = DocReader.Read(assembly).Elements.OfType<Enum>().Where(c => c.Id == typeId).FirstOrDefault();

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

            var member = DocReader.Read(assembly).Elements.OfType<Struct>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
            Assert.Same(typeof(SampleStruct), member.Info);
            Assert.True(member.Kind.HasFlag(MemberKinds.Struct));
            Assert.True(member.Kind.HasFlag(MemberKinds.Type));
        }

        [Fact]
        public void when_using_to_text_then_renders_text_content()
        {
            var xml = new FileInfo(@"DemoProject.xml").FullName;
            var members = DocReader.Read(xml);
            var member = members.Elements.OfType<TypeDeclaration>().FirstOrDefault(x => x.Id == "T:Demo.SampleExtensions");

            Assert.NotNull(member);
            Assert.Equal("Extension class for Demo.Sample.", member.Elements.OfType<Summary>().First().ToText());

            member = members.Elements.OfType<TypeDeclaration>().FirstOrDefault(x => x.Id == "T:Demo.Sample");
            Assert.NotNull(member);

            var content = @"This is the remarks section, which can also have c tag code.
var code = new SomeCodeTagWithinRemarks();
You can use Demo.Provider see tag within sections.
We can have paragraphs anywhere.
";

            Assert.Equal(content, member.Elements.OfType<Remarks>().First().ToText());
        }

        [Fact]
        public void when_reading_list_then_can_access_type_header_and_items()
        {
            var list = DocReader.Read(assembly).Traverse().OfType<List>().Single();

            Assert.Equal(ListType.Table, list.Type);
            Assert.NotNull(list.Header);
            Assert.Single(list.Items);
        }

        [Fact]
        public void when_reading_unknown_list_type_then_marks_as_unknown()
        {
            var xml = Path.GetTempFileName();
            File.WriteAllText(xml, @"<doc>
    <members>
        <member name='T:Demo.Provider'>
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

            var member = DocReader.Read(xml).Elements.First();

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

            var member = DocReader.Read(xml).Elements.OfType<UnknownMember>().FirstOrDefault();

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

            var member = DocReader.Read(assembly).Elements.OfType<Class>().Where(c => c.Id == typeId).FirstOrDefault();

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
            var member = DocReader.Read(assembly).Elements.OfType<Class>().Where(c => c.Id == typeId).Single();

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
            var member = DocReader.Read(assembly).Elements.OfType<Method>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
        }

        [Fact]
        public void when_documenting_generic_method_on_generic_type_then_sets_info()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));
            var typeId = map.FindId(typeof(SampleGeneric<,>).GetMethods()[0]);
            var member = DocReader.Read(assembly).Elements.OfType<Method>().Where(c => c.Id == typeId).FirstOrDefault();

            Assert.NotNull(member);
            Assert.NotNull(member.Info);
        }

        [Fact]
        public void when_documenting_method_then_parses_type_param_and_ref()
        {
            var map = new MemberIdMap();
            map.Add(typeof(SampleGeneric<,>));
            var typeId = map.FindId(typeof(SampleGeneric<,>).GetMethods()[0]);
            var member = DocReader.Read(assembly).Elements.OfType<Method>().Where(c => c.Id == typeId).Single();

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
            map.Add(typeof(Provider));
            var typeId = map.FindId(typeof(Provider));
            var member = DocReader.Read(assembly).Elements.OfType<Class>().Where(c => c.Id == typeId).Single();

            var element = member.Elements.OfType<Summary>().Single();
            Assert.Empty(element.ToText());
        }

        [Fact]
        public void when_parsing_empty_code_then_removes_empty_text()
        {
            var map = new MemberIdMap();
            map.Add(typeof(IProvider));
            var typeId = map.FindId(typeof(IProvider));
            var member = DocReader.Read(assembly).Elements.OfType<Interface>().Where(c => c.Id == typeId).Single();

            var element = member.Elements.OfType<Remarks>().Single().Elements.OfType<Code>().Single();
            Assert.Equal(0, element.Content.Length);
        }

        [Fact]
        public void when_parsing_property_then_reads_value_tag()
        {
            var map = new MemberIdMap();
            map.Add(typeof(Sample));
            var typeId = map.FindId(typeof(Sample).GetProperties()[0]);
            var member = DocReader.Read(assembly).Elements.OfType<Property>().Where(c => c.Id == typeId).Single();

            var element = member.Elements.OfType<Value>().Single();
            Assert.Equal("The id of this sample.", element.ToText());
        }

        [Fact]
        public void when_parsing_unknown_element_then_reads_inner_content()
        {
            var map = new MemberIdMap();
            map.Add(typeof(IProvider));
            var typeId = map.FindId(typeof(IProvider));
            var member = DocReader.Read(assembly).Elements.OfType<TypeDeclaration>().Where(c => c.Id == typeId).Single();

            var element = member.Elements.OfType<Summary>().Single();

            Assert.True(element.Elements.OfType<UnknownElement>().Any());
            Assert.True(element.Elements.OfType<UnknownElement>().Single().Elements.OfType<Text>().Any());
        }

        [Fact]
        public void when_reading_custom_xml_then_can_access_elements_and_attributes()
        {
            var member = DocReader.Read(Assembly.GetExecutingAssembly());

            var custom = member.Elements.OfType<Class>().First(c => c.Info == typeof(CustomXml));
            var summary = custom.Elements.OfType<Summary>().FirstOrDefault();

            Assert.NotNull(summary);

            var unk = summary.Elements.OfType<UnknownElement>().FirstOrDefault();

            Assert.NotNull(unk);
            Assert.Equal("value", unk.Xml.Attribute("id")?.Value);
            Assert.Equal("element with ", unk.Elements.OfType<Text>().FirstOrDefault()?.Content);

            var nested = unk.Elements.OfType<UnknownElement>().FirstOrDefault();

            Assert.NotNull(nested);
            Assert.Equal("help", nested.Xml.Attribute("class")?.Value);
            Assert.Equal("nested", nested.Elements.OfType<Text>().FirstOrDefault()?.Content);
        }

        [Fact]
        public void when_reading_known_element_then_can_access_custom_attributes()
        {
            var member = DocReader.Read(Assembly.GetExecutingAssembly());

            var custom = member.Elements.OfType<Class>().First(c => c.Info == typeof(CustomXml));
            var code = custom.Elements.OfType<Remarks>().FirstOrDefault()?.Elements?.OfType<Code>().FirstOrDefault();

            Assert.NotNull(code);

            Assert.True(code.Attributes.TryGetValue("source", out var source));
            Assert.Equal("foo.cs", source);

            Assert.True(code.Attributes.TryGetValue("region", out var region));
            Assert.Equal("example", region);
        }

        [Fact]
        public void when_reading_unknown_element_then_can_be_empty()
        {
            var member = DocReader.Read(Assembly.GetExecutingAssembly());
            var method = member.Elements.OfType<Method>()
                .FirstOrDefault(m => m.Info?.DeclaringType == typeof(CustomXml) && m.Info?.Name == nameof(CustomXml.Preliminary));

            Assert.NotNull(method);
            Assert.Equal("preliminary", method.Elements.OfType<UnknownElement>().First().Xml.Name.LocalName);

            Assert.Empty(method.Elements.OfType<UnknownElement>().First().Elements);
            Assert.Empty(method.Elements.OfType<UnknownElement>().First().Attributes);
        }

        [Fact]
        public void when_reading_byref_element_then_can_read_parameter_doc()
        {
            var member = DocReader.Read(Assembly.GetExecutingAssembly());
            var method = member.Elements.OfType<Method>()
                .FirstOrDefault(m => m.Info?.DeclaringType == typeof(CustomXml) && m.Info?.Name == nameof(CustomXml.ByRef));

            Assert.NotNull(method);
            Assert.Equal("p1", method.Elements.OfType<Param>().First().Name);
            Assert.Equal("byref", method.Elements.OfType<Param>().First().Elements.OfType<Text>().First().Content);
        }

        [Fact]
        public void when_reading_out_element_then_can_read_parameter_doc()
        {
            var member = DocReader.Read(Assembly.GetExecutingAssembly());
            var method = member.Elements.OfType<Method>()
                .FirstOrDefault(m => m.Info?.DeclaringType == typeof(CustomXml) && m.Info?.Name == nameof(CustomXml.Out));

            Assert.NotNull(method);
            Assert.Equal("p1", method.Elements.OfType<Param>().First().Name);
            Assert.Equal("out", method.Elements.OfType<Param>().First().Elements.OfType<Text>().First().Content);
        }

        [Fact]
        public void when_reading_with_keep_lines_then_preserves_original_text()
        {
            var member = DocReader.Read(Assembly.GetExecutingAssembly(), new ReaderOptions { KeepNewLinesInText = true });
            var method = member.Elements.OfType<Property>()
                .FirstOrDefault(m => m.Info?.DeclaringType == typeof(CustomXml) && m.Info?.Name == nameof(CustomXml.NewLines));

            Assert.NotNull(method);
            Assert.Equal(@"With

new


lines", method.Elements.OfType<Summary>().First().Elements.OfType<Text>().First().Content);
        }

        class CountingVisitor : Visitor
        {
            readonly string platform;
            readonly string fileName;

            public int TypeCount { get; set; }
            public int ContainerCount { get; set; }
            public int ElementCount { get; set; }

            public CountingVisitor(string platform)
            {
                this.platform = platform;
                fileName = "C:\\Temp\\" + platform + ".txt";
                //File.WriteAllText(fileName, "");
            }

            public override void VisitType(TypeDeclaration type)
            {
                base.VisitType(type);
                TypeCount++;
                //File.AppendAllLines("C:\\Temp\\" + platform + ".txt", new [] { type.ToString() });
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
                //File.AppendAllLines("C:\\Temp\\" + platform + ".txt", new[] { element.ToString() });
            }
        }

        /// <summary>
        /// This element contains 
        /// <custom id="value">
        /// element with <nested class="help">nested</nested> elements.
        /// </custom>
        /// </summary>
        /// <remarks>
        /// <code source="foo.cs" region="example" />
        /// </remarks>
        public class CustomXml
        {
            /// <preliminary />
            public void Preliminary() { }

            /// <summary>example method</summary>
            /// <param name="p1">byref</param>
            public void ByRef(ref int p1) { }


            /// <summary>example method</summary>
            /// <param name="p1">out</param>
            public void Out(out int p1)
            {
                p1 = default;
            }

            /// <summary>
            /// With
            /// 
            /// new
            /// 
            /// 
            /// lines
            /// </summary>
            public string NewLines { get; set; }
        }
    }
}