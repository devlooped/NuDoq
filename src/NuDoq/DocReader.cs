using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace NuDoq
{
    /// <summary>
    /// Options for <see cref="DocReader.Read(string,ReaderOptions)"/>
    /// </summary>
    public class ReaderOptions
    {
        /// <summary>
        /// Allows to keep new lines when parsing text content.
        /// </summary>
        public bool KeepNewLinesInText { get; set; }
    }

    /// <summary>
    /// Reads .NET XML API documentation files, optionally augmenting 
    /// them with reflection information if reading from an assembly.
    /// </summary>
    public class DocReader
    {
        readonly ReaderOptions options;

        DocReader(ReaderOptions options) => this.options = options;

        /// <summary>
        /// Reads the specified documentation file and returns a lazily-constructed 
        /// set of members that can be visited.
        /// </summary>
        /// <param name="fileName">Path to the documentation file.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static DocumentMembers Read(string fileName)
            => Read(fileName, new ReaderOptions());


        /// <summary>
        /// Reads the specified documentation file and returns a lazily-constructed 
        /// set of members that can be visited.
        /// </summary>
        /// <param name="fileName">Path to the documentation file.</param>
        /// <param name="options">Options for reading the documentation</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static DocumentMembers Read(string fileName, ReaderOptions options)
            => new DocReader(options).ReadInternal(fileName);

        /// <summary>
        /// Uses the specified assembly to locate a documentation file alongside the assembly by 
        /// changing the extension to ".xml". If the file is found, it will be read and all 
        /// found members will contain extended reflection information in the <see cref="Member.Info"/> 
        /// property.
        /// </summary>
        /// <param name="assembly">The assembly to read the documentation from.</param>
        /// <returns>All documented members found in the given file, together with the reflection metadata 
        /// association from the assembly.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static AssemblyMembers Read(Assembly assembly)
            => Read(assembly, null, new ReaderOptions());

        /// <summary>
        /// Uses the specified assembly to locate a documentation file alongside the assembly by 
        /// changing the extension to ".xml". If the file is found, it will be read and all 
        /// found members will contain extended reflection information in the <see cref="Member.Info"/> 
        /// property.
        /// </summary>
        /// <param name="assembly">The assembly to read the documentation from.</param>
        /// <param name="documentationFilename">Path to the documentation file.</param>
        /// <returns>All documented members found in the given file, together with the reflection metadata 
        /// association from the assembly.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static AssemblyMembers Read(Assembly assembly, string? documentationFilename)
            => Read(assembly, documentationFilename, new ReaderOptions());

        /// <summary>
        /// Uses the specified assembly to locate a documentation file alongside the assembly by 
        /// changing the extension to ".xml". If the file is found, it will be read and all 
        /// found members will contain extended reflection information in the <see cref="Member.Info"/> 
        /// property.
        /// </summary>
        /// <param name="assembly">The assembly to read the documentation from.</param>
        /// <param name="options">Options for reading the documentation.</param>
        /// <returns>All documented members found in the given file, together with the reflection metadata 
        /// association from the assembly.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static AssemblyMembers Read(Assembly assembly, ReaderOptions options)
            => Read(assembly, null, options);

        /// <summary>
        /// Uses the specified assembly to locate a documentation file alongside the assembly by 
        /// changing the extension to ".xml". If the file is found, it will be read and all 
        /// found members will contain extended reflection information in the <see cref="Member.Info"/> 
        /// property.
        /// </summary>
        /// <param name="assembly">The assembly to read the documentation from.</param>
        /// <param name="documentationFilename">Path to the documentation file.</param>
        /// <param name="options">Options for reading the documentation.</param>
        /// <returns>All documented members found in the given file, together with the reflection metadata 
        /// association from the assembly.</returns>
        /// <exception cref="FileNotFoundException">Could not find documentation file to load.</exception>
        public static AssemblyMembers Read(Assembly assembly, string? documentationFilename, ReaderOptions options)
            => new DocReader(options).ReadInternal(assembly, documentationFilename);

        DocumentMembers ReadInternal(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            var doc = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            return new DocumentMembers(doc, doc.Root.Element("members").Elements("member")
                .Where(element => element.Attribute("name") != null)
                .Select(element => CreateMember(element.Attribute("name").Value, element, ReadContent(element),
                    element.Attributes().ToDictionary(x => x.Name.LocalName, x => x.Value))));
        }

        AssemblyMembers ReadInternal(Assembly assembly, string? documentationFilename)
        {
            var fileName = documentationFilename;

            if (string.IsNullOrEmpty(fileName))
                fileName = Path.ChangeExtension(assembly.Location, ".xml");

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load. Expected: " + fileName, fileName);

            var doc = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            var map = new MemberIdMap();
            map.Add(assembly);

            return new AssemblyMembers(assembly, map, doc, doc.Root.Element("members").Elements("member")
                .Where(element => element.Attribute("name") != null)
                .Select(element => CreateMember(element.Attribute("name").Value, element, ReadContent(element),
                    element.Attributes().ToDictionary(x => x.Name.LocalName, x => x.Value)))
                .Select(member => ReplaceExtensionMethods(member, map))
                .Select(member => ReplaceTypes(member, map))
                .Select(member => SetInfo(member, map)));
        }

        /// <summary>
        /// Sets the extended reflection info if found in the map.
        /// </summary>
        Member SetInfo(Member member, MemberIdMap map)
        {
            member.Info = map.FindMember(member.Id);

            return member;
        }

        /// <summary>
        /// Replaces the generic <see cref="TypeDeclaration"/> with 
        /// concrete types according to the reflection information.
        /// </summary>
        Member ReplaceTypes(Member member, MemberIdMap map)
        {
            if (member.Kind != MemberKinds.Type)
                return member;

            var type = (Type)map.FindMember(member.Id);
            if (type == null)
                return member;

            if (type.IsInterface)
                return new Interface(member.Id, member.Elements, member.Attributes);
            if (type.IsClass)
                return new Class(member.Id, member.Elements, member.Attributes);
            if (type.IsEnum)
                return new Enum(member.Id, member.Elements, member.Attributes);
            if (type.IsValueType)
                return new Struct(member.Id, member.Elements, member.Attributes);

            return member;
        }

        /// <summary>
        /// Replaces the generic method element with a more specific extension method 
        /// element as needed.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        Member ReplaceExtensionMethods(Member member, MemberIdMap map)
        {
            if (member.Kind != MemberKinds.Method)
                return member;

            var method = (MethodBase)map.FindMember(member.Id);
            if (method == null)
                return member;

            if (method.GetCustomAttributes(true).Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.ExtensionAttribute"))
            {
                var extendedTypeId = map.FindId(method.GetParameters()[0].ParameterType);
                if (!string.IsNullOrEmpty(extendedTypeId))
                    return new ExtensionMethod(member.Id, extendedTypeId, member.Elements, member.Attributes);
            }

            return member;
        }

        /// <summary>
        /// Creates the appropriate type of member according to the member id prefix.
        /// </summary>
        Member CreateMember(string memberId, XElement element, IEnumerable<Element> children, IDictionary<string, string> attributes)
        {
            Member? member = (memberId[0]) switch
            {
                'T' => new TypeDeclaration(memberId, children, attributes),
                'F' => new Field(memberId, children, attributes),
                'P' => new Property(memberId, children, attributes),
                'M' => new Method(memberId, children, attributes),
                'E' => new Event(memberId, children, attributes),
                _ => new UnknownMember(memberId, attributes),
            };
            member.SetLineInfo(element);
            return member;
        }

        /// <summary>
        /// Reads all supported documentation elements.
        /// </summary>
        IEnumerable<Element> ReadContent(XElement xml)
        {
            foreach (var node in xml.Nodes())
            {
                var element = default(Element);
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        var elementNode = (XElement)node;
                        var attributes = elementNode.Attributes().ToDictionary(x => x.Name.LocalName, x => x.Value);
                        element = elementNode.Name.LocalName switch
                        {
                            "summary" => new Summary(ReadContent(elementNode), attributes),
                            "remarks" => new Remarks(ReadContent(elementNode), attributes),
                            "example" => new Example(ReadContent(elementNode), attributes),
                            "para" => new Para(ReadContent(elementNode), attributes),
                            "param" => new Param(ReadContent(elementNode), attributes),
                            "paramref" => new ParamRef(attributes),
                            "typeparam" => new TypeParam(ReadContent(elementNode), attributes),
                            "typeparamref" => new TypeParamRef(attributes),
                            "code" => new Code(TrimCode(elementNode.Value), attributes),
                            "c" => new C(elementNode.Value, attributes),
                            "see" => new See(elementNode.Value, ReadContent(elementNode), attributes),
                            "seealso" => new SeeAlso(elementNode.Value, ReadContent(elementNode), attributes),
                            "list" => new List(ReadContent(elementNode), attributes),
                            "listheader" => new ListHeader(ReadContent(elementNode), attributes),
                            "term" => new Term(ReadContent(elementNode), attributes),
                            "description" => new Description(ReadContent(elementNode), attributes),
                            "item" => new Item(ReadContent(elementNode), attributes),
                            "exception" => new Exception(ReadContent(elementNode), attributes),
                            "value" => new Value(ReadContent(elementNode), attributes),
                            "returns" => new Returns(ReadContent(elementNode), attributes),
                            _ => new UnknownElement(elementNode, ReadContent(elementNode)),
                        };
                        break;
                    case XmlNodeType.Text:
                        element = new Text(TrimText(((XText)node).Value));
                        break;
                    case XmlNodeType.CDATA:
                        element = new Text(((XCData)node).Value);
                        break;
                    default:
                        break;
                }

                if (element != null)
                {
                    element.SetLineInfo(xml);
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Trims the text by removing new lines and trimming the indent.
        /// </summary>
        string TrimText(string content)
            => TrimLines(content, StringSplitOptions.RemoveEmptyEntries, options.KeepNewLinesInText ? Environment.NewLine : " ");

        /// <summary>
        /// Trims the code by removing extra indent.
        /// </summary>
        string TrimCode(string content)
            => TrimLines(content, StringSplitOptions.None, Environment.NewLine);

        string TrimWhitespaceTo(string content, int index)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            for (var i = 0; i < index; i++)
            {
                // Check for end of whitespace
                if (!char.IsWhiteSpace(content[i]))
                    return content.Substring(i);
            }

            return content.Substring(index);
        }

        string TrimLines(string content, StringSplitOptions splitOptions, string joinWith)
        {
            var lines = content.Split(new[] { Environment.NewLine, "\n" }, splitOptions).ToList();

            if (lines.Count == 0)
                return string.Empty;

            // Remove leading and trailing empty lines which are used for wrapping in the doc XML.
            if (lines[0].Trim().Length == 0)
                lines.RemoveAt(0);

            if (lines.Count == 0)
                return string.Empty;

            if (lines[lines.Count - 1].Trim().Length == 0)
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return string.Empty;

            // The indent of the first line of content determines the base 
            // indent for all the lines, which   we should remove since it's just 
            // a doc gen artifact.
            var indent = lines[0].TakeWhile(c => char.IsWhiteSpace(c)).Count();
            // Indent in generated XML doc files is greater than 4 always. 
            // This allows us to optimize the case where the author actually placed 
            // whitespace inline in between tags.
            if (indent <= 4 && !string.IsNullOrEmpty(lines[0]) && lines[0][0] != '\t')
                indent = 0;

            return string.Join(joinWith, lines
                .Select(line =>
                    {
                        if (string.IsNullOrEmpty(line))
                            return line;
                        else if (line.Length < indent)
                            return string.Empty;
                        else
                            return TrimWhitespaceTo(line, indent);
                    })
                .ToArray());
        }
    }
}
