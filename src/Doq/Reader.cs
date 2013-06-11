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

namespace ClariusLabs.Doq
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public static class Reader
    {
        public static Members Read(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            var doc = XDocument.Load(fileName);

            return new Members(doc.Root.Element("members").Elements("member")
                .Select(e => CreateMember(e.Attribute("name").Value, ReadContent(e))));
        }

        // TODO: support multiple assemblies.
        public static Members Read(Assembly assembly)
        {
            var fileName = Path.ChangeExtension(assembly.Location, ".xml");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Could not find documentation file to load.", fileName);

            var doc = XDocument.Load(fileName);
            var map = new MemberIdMap();
            map.Add(assembly);

            return new Members(doc.Root.Element("members").Elements("member")
                .Where(element => element.Attribute("name") != null)
                .Select(element => CreateMember(element.Attribute("name").Value, ReadContent(element)))
                .Select(member => ReplaceExtensionMethods(member, map))
                .Select(member => ReplaceTypes(member, map))
                .Select(member => SetInfo(member, map)));
        }

        private static Member SetInfo(Member member, MemberIdMap map)
        {
            member.Info = map.FindMember(member.ToString());

            return member;
        }

        private static Member ReplaceTypes(Member member, MemberIdMap map)
        {
            if (member.Kind != MemberKinds.Type)
                return member;

            var type = (Type)map.FindMember(member.ToString());
            if (type == null)
                return member;

            var nestingTypeId = "";
            if (type.DeclaringType != null &&
                !string.IsNullOrEmpty((nestingTypeId = map.FindId(type.DeclaringType))))
                return new NestedType(member.ToString(), nestingTypeId, member.Elements);

            if (type.IsInterface)
                return new Interface(member.ToString(), member.Elements);
            if (type.IsClass)
                return new Class(member.ToString(), member.Elements);
            if (type.IsEnum)
                return new Enum(member.ToString(), member.Elements);
            if (type.IsValueType)
                return new Struct(member.ToString(), member.Elements);

            return member;
        }

        private static Member ReplaceExtensionMethods(Member member, MemberIdMap map)
        {
            if (member.Kind != MemberKinds.Method)
                return member;

            var method = (MethodBase)map.FindMember(member.ToString());
            if (method == null)
                return member;

            if (method.GetCustomAttributes(true).Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.ExtensionAttribute"))
            {
                var extendedTypeId = map.FindId(method.GetParameters()[0].ParameterType);
                if (!string.IsNullOrEmpty(extendedTypeId))
                    return new ExtensionMethod(member.ToString(), extendedTypeId, member.Elements);
            }

            return member;
        }

        private static Member CreateMember(string memberId, IEnumerable<Element> elements)
        {
            switch (memberId[0])
            {
                case 'T':
                    return new TypeDeclaration(memberId, elements);
                case 'F':
                    return new Field(memberId, elements);
                case 'P':
                    return new Property(memberId, elements);
                case 'M':
                    return new Method(memberId, elements);
                case 'E':
                    return new Event(memberId, elements);
                default:
                    return new UnknownMember(memberId);
            }
        }

        private static IEnumerable<Element> ReadContent(XElement element)
        {
            foreach (var node in element.Nodes())
            {
                switch (node.NodeType)
                {
                    case System.Xml.XmlNodeType.Element:
                        var elementNode = (XElement)node;
                        switch (elementNode.Name.LocalName)
                        {
                            case "summary":
                                yield return new Summary(ReadContent(elementNode));
                                break;
                            case "remarks":
                                yield return new Remarks(ReadContent(elementNode));
                                break;
                            case "example":
                                yield return new Example(ReadContent(elementNode));
                                break;
                            case "para":
                                yield return new Para(ReadContent(elementNode));
                                break;
                            case "param":
                                yield return new Param(FindAttribute(elementNode, "name"), ReadContent(elementNode));
                                break;
                            case "paramref":
                                yield return new ParamRef(FindAttribute(elementNode, "name"));
                                break;
                            case "typeparam":
                                yield return new TypeParam(FindAttribute(elementNode, "name"), ReadContent(elementNode));
                                break;
                            case "typeparamref":
                                yield return new TypeParamRef(FindAttribute(elementNode, "name"));
                                break;
                            case "code":
                                yield return new Code(TrimCode(elementNode.Value));
                                break;
                            case "c":
                                yield return new C(elementNode.Value);
                                break;
                            case "see":
                                yield return new See(FindAttribute(elementNode, "cref"));
                                break;
                            case "seealso":
                                yield return new SeeAlso(FindAttribute(elementNode, "cref"));
                                break;
                            case "list":
                                yield return new List(FindAttribute(elementNode, "type"), ReadContent(elementNode));
                                break;
                            case "listheader":
                                yield return new ListHeader(ReadContent(elementNode));
                                break;
                            case "term":
                                yield return new Term(ReadContent(elementNode));
                                break;
                            case "description":
                                yield return new Description(ReadContent(elementNode));
                                break;
                            case "item":
                                yield return new Item(ReadContent(elementNode));
                                break;
                            case "exception":
                                yield return new Exception(FindAttribute(elementNode, "cref"), ReadContent(elementNode));
                                break;
                            case "value":
                                yield return new Value(ReadContent(elementNode));
                                break;
                            default:
                                break;
                        }
                        break;
                    case System.Xml.XmlNodeType.Text:
                        yield return new Text(TrimText(((XText)node).Value));
                        break;
                    default:
                        break;
                }

            }
        }

        private static string FindAttribute(XElement elementNode, string attributeName)
        {
            return elementNode.Attributes().Where(x => x.Name == attributeName).Select(x => x.Value).FirstOrDefault();
        }

        private static string TrimText(string content)
        {
            return TrimLines(content, StringSplitOptions.RemoveEmptyEntries, " ");
        }

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
            if (indent <= 4)
                indent = 0;

            return string.Join(joinWith, lines
                .Select(line => line.Substring(indent))
                .ToArray());
        }
    }
}