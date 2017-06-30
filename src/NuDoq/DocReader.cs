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

namespace NuDoq
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Reads .NET XML API documentation files, optionally augmenting 
    /// them with reflection information if reading from an assembly.
    /// </summary>
    public static class DocReader
    {
        /// <summary>
        /// Reads the specified documentation file and returns a lazily-constructed 
        /// set of members that can be visited.
        /// </summary>
        /// <param name="fileName">Path to the documentation file.</param>
        /// <returns>All documented members found in the given file.</returns>
        /// <exception cref="System.IO.FileNotFoundException">Could not find documentation file to load.</exception>
        public static DocumentMembers Read(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            var doc = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            return new DocumentMembers(doc, doc.Root.Element("members").Elements("member")
                .Where(element => element.Attribute("name") != null)
                //.OrderBy(element => element.Attribute("name").Value)
                .Select(element => CreateMember(element.Attribute("name").Value, element, ReadContent(element))));
        }

        /// <summary>
        /// Uses the specified assembly to locate a documentation file alongside the assembly by 
        /// changing the extension to ".xml". If the file is found, it will be read and all 
        /// found members will contain extended reflection information in the <see cref="Member.Info"/> 
        /// property.
        /// </summary>
        /// <param name="assembly">The assembly to read the documentation from.</param>
        /// <returns>All documented members found in the given file, together with the reflection metadata 
        /// association from the assembly.</returns>
        /// <exception cref="System.IO.FileNotFoundException">Could not find documentation file to load.</exception>
        public static AssemblyMembers Read(Assembly assembly)
        {
            return Read(assembly, null);
        }

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
        /// <exception cref="System.IO.FileNotFoundException">Could not find documentation file to load.</exception>
        public static AssemblyMembers Read(Assembly assembly, string documentationFilename)
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
                //.OrderBy(e => e.Attribute("name").Value)
                .Select(element => CreateMember(element.Attribute("name").Value, element, ReadContent(element)))
                .Select(member => ReplaceExtensionMethods(member, map))
                .Select(member => ReplaceTypes(member, map))
                .Select(member => SetInfo(member, map)));
        }

        /// <summary>
        /// Sets the extended reflection info if found in the map.
        /// </summary>
        private static Member SetInfo(Member member, MemberIdMap map)
        {
            member.Info = map.FindMember(member.Id);

            return member;
        }

        /// <summary>
        /// Replaces the generic <see cref="TypeDeclaration"/> with 
        /// concrete types according to the reflection information.
        /// </summary>
        private static Member ReplaceTypes(Member member, MemberIdMap map)
        {
            if (member.Kind != MemberKinds.Type)
                return member;

            var type = (Type)map.FindMember(member.Id);
            if (type == null)
                return member;

            if (type.IsInterface)
                return new Interface(member.Id, member.Elements);
            if (type.IsClass)
                return new Class(member.Id, member.Elements);
            if (type.IsEnum)
                return new Enum(member.Id, member.Elements);
            if (type.IsValueType)
                return new Struct(member.Id, member.Elements);

            return member;
        }

        /// <summary>
        /// Replaces the generic method element with a more specific extension method 
        /// element as needed.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        private static Member ReplaceExtensionMethods(Member member, MemberIdMap map)
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
                    return new ExtensionMethod(member.Id, extendedTypeId, member.Elements);
            }

            return member;
        }

        /// <summary>
        /// Creates the appropriate type of member according to the member id prefix.
        /// </summary>
        private static Member CreateMember(string memberId, XElement element, IEnumerable<Element> children)
        {
            var member = default(Member);
            switch (memberId[0])
            {
                case 'T':
                    member = new TypeDeclaration(memberId, children);
                    break;
                case 'F':
                    member = new Field(memberId, children);
                    break;
                case 'P':
                    member = new Property(memberId, children);
                    break;
                case 'M':
                    member = new Method(memberId, children);
                    break;
                case 'E':
                    member = new Event(memberId, children);
                    break;
                default:
                    member = new UnknownMember(memberId);
                    break;
            }

            member.SetLineInfo(element as IXmlLineInfo);
            return member;
        }

        /// <summary>
        /// Reads all supported documentation elements.
        /// </summary>
        private static IEnumerable<Element> ReadContent(XElement xml)
        {
            foreach (var node in xml.Nodes())
            {
                var element = default(Element);
                switch (node.NodeType)
                {
                    case System.Xml.XmlNodeType.Element:
                        var elementNode = (XElement)node;
                        switch (elementNode.Name.LocalName)
                        {
                            case "summary":
                                element = new Summary(ReadContent(elementNode));
                                break;
                            case "remarks":
                                element = new Remarks(ReadContent(elementNode));
                                break;
                            case "example":
                                element = new Example(ReadContent(elementNode));
                                break;
                            case "para":
                                element = new Para(ReadContent(elementNode));
                                break;
                            case "param":
                                element = new Param(FindAttribute(elementNode, "name"), ReadContent(elementNode));
                                break;
                            case "paramref":
                                element = new ParamRef(FindAttribute(elementNode, "name"));
                                break;
                            case "typeparam":
                                element = new TypeParam(FindAttribute(elementNode, "name"), ReadContent(elementNode));
                                break;
                            case "typeparamref":
                                element = new TypeParamRef(FindAttribute(elementNode, "name"));
                                break;
                            case "code":
                                element = new Code(TrimCode(elementNode.Value), FindAttribute(elementNode, "source"), FindAttribute(elementNode, "region"));
                                break;
                            case "c":
                                element = new C(elementNode.Value);
                                break;
                            case "a":
                                element = new Anchor(FindAttribute(elementNode, "href"), elementNode.Value, ReadContent(elementNode));
                                break;
                            case "see":
                                element = new See(FindAttribute(elementNode, "cref"), FindAttribute(elementNode, "langword"), elementNode.Value, ReadContent(elementNode));
                                break;
                            case "seealso":
                                element = new SeeAlso(FindAttribute(elementNode, "cref"), elementNode.Value, ReadContent(elementNode));
                                break;
                            case "list":
                                element = new List(FindAttribute(elementNode, "type"), ReadContent(elementNode));
                                break;
                            case "listheader":
                                element = new ListHeader(ReadContent(elementNode));
                                break;
                            case "term":
                                element = new Term(ReadContent(elementNode));
                                break;
                            case "description":
                                element = new Description(ReadContent(elementNode));
                                break;
                            case "item":
                                element = new Item(ReadContent(elementNode));
                                break;
                            case "exception":
                                element = new Exception(FindAttribute(elementNode, "cref"), ReadContent(elementNode));
                                break;
                            case "value":
                                element = new Value(ReadContent(elementNode));
                                break;
                            case "returns":
                                element = new Returns(ReadContent(elementNode));
                                break;
                            default:
                                element = new UnknownElement(elementNode, ReadContent(elementNode));
                                break;
                        }
                        break;
                    case System.Xml.XmlNodeType.Text:
                        element = new Text(TrimText(((XText)node).Value));
                        break;
                    default:
                        break;
                }

                if (element != null)
                {
                    element.SetLineInfo(xml as IXmlLineInfo);
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Retrieves an attribute value if found, otherwise, returns a null string.
        /// </summary>
        private static string FindAttribute(XElement elementNode, string attributeName)
        {
            return elementNode.Attributes().Where(x => x.Name == attributeName).Select(x => x.Value).FirstOrDefault();
        }

        /// <summary>
        /// Trims the text by removing new lines and trimming the indent.
        /// </summary>
        private static string TrimText(string content)
        {
            return TrimLines(content, StringSplitOptions.RemoveEmptyEntries, " ");
        }

        /// <summary>
        /// Trims the code by removing extra indent.
        /// </summary>
        private static string TrimCode(string content)
        {
            return TrimLines(content, StringSplitOptions.None, Environment.NewLine);
        }

        private static string TrimLines(string content, StringSplitOptions splitOptions, string joinWith)
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
                            return line.Substring(indent);
                    })
                .ToArray());
        }
    }
}