#region Apache Licensed
/*
 Copyright 2013 Daniel Cazzulino

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
    using System.Linq;
    using System.Xml.Linq;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public class XmlVisitor : Visitor
    {
        XElement currentElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlVisitor"/> class.
        /// </summary>
        public XmlVisitor()
        {
            this.Xml = new XDocument();
            this.Xml.Declaration = null;
        }

        /// <summary>
        /// Visits the entire set of members read by the <see cref="DocReader.Read(Assembly)" />.
        /// </summary>
        public override void VisitAssembly(AssemblyMembers assembly)
        {
            this.currentElement = new XElement("members");
            this.Xml.Add(new XElement("doc",
                new XElement("assembly",
                    new XElement("name", assembly.Assembly.GetName().Name)),
                    this.currentElement));

            base.VisitAssembly(assembly);
        }

        /// <summary>
        /// Visits the entire set of members read by the <see cref="DocReader.Read(string)" />.
        /// </summary>
        public override void VisitDocument(DocumentMembers document)
        {
            if (this.currentElement == null)
            {
                this.currentElement = new XElement("members");
                this.Xml.Add(new XElement("doc", this.currentElement));
            }

            base.VisitDocument(document);
        }

        /// <summary>
        /// Visit the generic base class <see cref="Member" />.
        /// </summary>
        /// <param name="member"></param>
        /// <remarks>
        /// This method is called for all <see cref="Member" />-derived
        /// types.
        /// </remarks>
        public override void VisitMember(Member member)
        {
            AddXml("member", "name", member.Id, member, base.VisitMember);
        }

        /// <summary>
        /// Visits the <c>c</c> documentation element.
        /// </summary>
        public override void VisitC(C code)
        {
            AddXml(new XElement("c", new XText(code.Content)), code, base.VisitC);
        }

        /// <summary>
        /// Visits the <c>code</c> documentation element.
        /// </summary>
        public override void VisitCode(Code code)
        {
            AddXml(new XElement("code", new XText(code.Content)), code, base.VisitCode);
        }

        /// <summary>
        /// Visits the <c>description</c> documentation element.
        /// </summary>
        public override void VisitDescription(Description description)
        {
            AddXml("description", description, base.VisitDescription);
        }

        /// <summary>
        /// Visits the <c>example</c> documentation element.
        /// </summary>
        public override void VisitExample(Example example)
        {
            AddXml("example", example, base.VisitExample);
        }

        /// <summary>
        /// Visits the <c>exception</c> documentation element.
        /// </summary>
        public override void VisitException(Exception exception)
        {
            AddXml("exception", "cref", exception.Cref, exception, base.VisitException);
        }

        /// <summary>
        /// Visits the <c>item</c> documentation element.
        /// </summary>
        public override void VisitItem(Item item)
        {
            AddXml("item", item, base.VisitItem);
        }

        /// <summary>
        /// Visits the <c>list</c> documentation element.
        /// </summary>
        public override void VisitList(List list)
        {
            AddXml("list", "type", list.Type.ToString().ToLowerInvariant(), list, base.VisitList);
        }

        /// <summary>
        /// Visits the <c>listheader</c> documentation element.
        /// </summary>
        public override void VisitListHeader(ListHeader header)
        {
            AddXml("listheader", header, base.VisitListHeader);
        }

        /// <summary>
        /// Visits the <c>para</c> documentation element.
        /// </summary>
        public override void VisitPara(Para para)
        {
            AddXml("para", para, base.VisitPara);
        }

        /// <summary>
        /// Visits the <c>param</c> documentation element.
        /// </summary>
        public override void VisitParam(Param param)
        {
            AddXml("param", "name", param.Name, param, base.VisitParam);
        }

        /// <summary>
        /// Visits the <c>paramref</c> documentation elemnet.
        /// </summary>
        public override void VisitParamRef(ParamRef paramRef)
        {
            AddXml("paramref", "name", paramRef.Name, paramRef, base.VisitParamRef);
        }

        /// <summary>
        /// Visits the <c>remarks</c> documentation element.
        /// </summary>
        public override void VisitRemarks(Remarks remarks)
        {
            AddXml("remarks", remarks, base.VisitRemarks);
        }

        /// <summary>
        /// Visits the <c>see</c> documentation element.
        /// </summary>
        public override void VisitSee(See see)
        {
            var element = new XElement("see");
            if (see.Cref != null)
                element.Add(new XAttribute("cref", see.Cref));
            if (see.Langword != null)
                element.Add(new XAttribute("langword", see.Langword));

            AddXml(element, see, base.VisitSee);
        }

        /// <summary>
        /// Visits the <c>seealso</c> documentation element.
        /// </summary>
        public override void VisitSeeAlso(SeeAlso seeAlso)
        {
            AddXml("seealso", "cref", seeAlso.Cref, seeAlso, base.VisitSeeAlso);
        }

        /// <summary>
        /// Visits the <c>summary</c> documentation element.
        /// </summary>
        public override void VisitSummary(Summary summary)
        {
            AddXml("summary", summary, base.VisitSummary);
        }

        /// <summary>
        /// Visits the <c>term</c> documentation element.
        /// </summary>
        public override void VisitTerm(Term term)
        {
            AddXml("term", term, base.VisitTerm);
        }

        /// <summary>
        /// Visits the literal text inside other documentation elements.
        /// </summary>
        public override void VisitText(Text text)
        {
            this.currentElement.Add(new XText(text.Content));
            base.VisitText(text);
        }

        /// <summary>
        /// Visits the <c>typeparam</c> documentation element.
        /// </summary>
        public override void VisitTypeParam(TypeParam typeParam)
        {
            AddXml("typeparam", "name", typeParam.Name, typeParam, base.VisitTypeParam);
        }

        /// <summary>
        /// Visits the <c>typeparamref</c> documentation element.
        /// </summary>
        public override void VisitTypeParamRef(TypeParamRef typeParamRef)
        {
            AddXml("typeparamref", "name", typeParamRef.Name, typeParamRef, base.VisitTypeParamRef);
        }

        /// <summary>
        /// Visits an unknown documentation element.
        /// </summary>
        public override void VisitUnknownElement(UnknownElement element)
        {
            AddXml(new XElement(element.Xml.Name, element.Xml.Attributes()), element, base.VisitUnknownElement);
        }

        /// <summary>
        /// Visits the <c>value</c> documentation element.
        /// </summary>
        public override void VisitValue(Value value)
        {
            AddXml("value", value, base.VisitValue);
        }

        /// <summary>
        /// Gets the XML.
        /// </summary>
        public XDocument Xml { get; private set; }

        void AddXml<TVisitable>(string elementName, TVisitable element, Action<TVisitable> visit)
        {
            AddXml(new XElement(elementName), element, visit);
        }

        void AddXml<TVisitable>(string elementName, string attributeName, object attributeValueOrNull, TVisitable element, Action<TVisitable> visit)
        {
            var xml = new XElement(elementName);
            if (attributeValueOrNull != null)
                xml.Add(new XAttribute(attributeName, attributeValueOrNull));

            AddXml(xml, element, visit);
        }

        void AddXml<TVisitable>(XElement xml, TVisitable element, Action<TVisitable> visit)
        {
            this.currentElement.Add(xml);
            this.currentElement = xml;

            visit(element);

            this.currentElement = this.currentElement.Parent;
        }
    }
}