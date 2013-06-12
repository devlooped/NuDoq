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

    /// <summary>
    /// Delegate configuration to use with the <see cref="DelegateVisitor"/>, 
    /// which allows anonymous visitors to be used without having to create 
    /// <see cref="Visitor"/>-derived classes.
    /// </summary>
    public class VisitorDelegates
    {
        /// <summary>
        /// Gets or sets the action to invoke when visiting all documented members of an XML document file.
        /// </summary>
        /// <seealso cref="Visitor.VisitDocument"/>
        public Action<DocumentMembers> VisitDocument { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting all documented members of an assembly.
        /// </summary>
        /// <seealso cref="Visitor.VisitAssembly"/>
        public Action<AssemblyMembers> VisitAssembly { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting a type member.
        /// </summary>
        /// <seealso cref="Visitor.VisitType"/>
        public Action<TypeDeclaration> VisitType { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting an interface.
        /// </summary>
        /// <seealso cref="Visitor.VisitInterface"/>
        public Action<Interface> VisitInterface { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting a class.
        /// </summary>
        /// <seealso cref="Visitor.VisitClass"/>
        public Action<Class> VisitClass { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting a struct.
        /// </summary>
        /// <seealso cref="Visitor.VisitStruct"/>
        public Action<Struct> VisitStruct { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting an enumeration.
        /// </summary>
        /// <seealso cref="Visitor.VisitEnum"/>
        public Action<Enum> VisitEnum { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting a field.
        /// </summary>
        /// <seealso cref="Visitor.VisitField"/>
        public Action<Field> VisitField { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting a property.
        /// </summary>
        /// <seealso cref="Visitor.VisitProperty"/>
        public Action<Property> VisitProperty { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting an event.
        /// </summary>
        /// <seealso cref="Visitor.VisitEvent"/>
        public Action<Event> VisitEvent { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting a method.
        /// </summary>
        /// <seealso cref="Visitor.VisitMethod"/>
        public Action<Method> VisitMethod { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting an extension method.
        /// </summary>
        /// <seealso cref="Visitor.VisitExtensionMethod"/>
        public Action<ExtensionMethod> VisitExtensionMethod { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting any member.
        /// </summary>
        /// <seealso cref="Visitor.VisitMember"/>
        public Action<Member> VisitMember { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>summary</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitSummary"/>
        public Action<Summary> VisitSummary { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>remarks</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitRemarks"/>
        public Action<Remarks> VisitRemarks { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>para</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitPara"/>
        public Action<Para> VisitPara { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>code</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitCode"/>
        public Action<Code> VisitCode { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>c</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitC"/>
        public Action<C> VisitC { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the literal text inside other documentation elements.
        /// </summary>
        /// <seealso cref="Visitor.VisitText"/>
        public Action<Text> VisitText { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>example</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitExample"/>
        public Action<Example> VisitExample { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>see</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitSee"/>
        public Action<See> VisitSee { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>seealso</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitSeeAlso"/>
        public Action<SeeAlso> VisitSeeAlso { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>param</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitParam"/>
        public Action<Param> VisitParam { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>paramref</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitParamRef"/>
        public Action<ParamRef> VisitParamRef { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>typeparam</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitTypeParam"/>
        public Action<TypeParam> VisitTypeParam { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>typeparamref</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitTypeParamRef"/>
        public Action<TypeParamRef> VisitTypeParamRef { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>value</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitValue"/>
        public Action<Value> VisitValue { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>list</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitList"/>
        public Action<List> VisitList { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>listheader</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitListHeader"/>
        public Action<ListHeader> VisitListHeader { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>term</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitTerm"/>
        public Action<Term> VisitTerm { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>description</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitDescription"/>
        public Action<Description> VisitDescription { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>item</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitItem"/>
        public Action<Item> VisitItem { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting the <c>exception</c> documentation element.
        /// </summary>
        /// <seealso cref="Visitor.VisitException"/>
        public Action<Exception> VisitException { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting an unknown member.
        /// </summary>
        /// <seealso cref="Visitor.VisitUnknownMember"/>
        public Action<UnknownMember> VisitUnknownMember { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting an unknown element.
        /// </summary>
        /// <seealso cref="Visitor.VisitUnknownElement"/>
        public Action<UnknownElement> VisitUnknownElement { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting any container element.
        /// </summary>
        /// <seealso cref="Visitor.VisitContainer"/>
        public Action<Container> VisitContainer { get; set; }

        /// <summary>
        /// Gets or sets the action to invoke when visiting any element.
        /// </summary>
        /// <seealso cref="Visitor.VisitElement"/>
        public Action<Element> VisitElement { get; set; }
    }

    /// <summary>
    /// A visitor that can receive delegates for each visit operation 
    /// in the visitable model, via the <see cref="VisitorDelegates"/> 
    /// type.
    /// </summary>
    public class DelegateVisitor : Visitor
    {
        private VisitorDelegates delegates;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateVisitor"/> class.
        /// </summary>
        /// <param name="delegates">The delegates to use when visiting the model.</param>
        public DelegateVisitor(VisitorDelegates delegates)
        {
            this.delegates = delegates;
        }

        /// <summary>
        /// See <see cref="Visitor.VisitDocument"/>.
        /// </summary>
        public override void VisitDocument(DocumentMembers document)
        {
            if (delegates.VisitDocument != null)
                delegates.VisitDocument(document);

            base.VisitDocument(document);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitAssembly"/>.
        /// </summary>
        public override void VisitAssembly(AssemblyMembers assembly)
        {
            if (delegates.VisitAssembly != null)
                delegates.VisitAssembly(assembly);

            base.VisitAssembly(assembly);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitC"/>.
        /// </summary>
        public override void VisitC(C code)
        {
            if (delegates.VisitC != null)
                delegates.VisitC(code);

            base.VisitC(code);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitClass"/>.
        /// </summary>
        public override void VisitClass(Class type)
        {
            if (delegates.VisitClass != null)
                delegates.VisitClass(type);

            base.VisitClass(type);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitCode"/>.
        /// </summary>
        public override void VisitCode(Code code)
        {
            if (delegates.VisitCode != null)
                delegates.VisitCode(code);

            base.VisitCode(code);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitContainer"/>.
        /// </summary>
        protected override void VisitContainer(Container container)
        {
            if (delegates.VisitContainer != null)
                delegates.VisitContainer(container);

            base.VisitContainer(container);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitDescription"/>.
        /// </summary>
        public override void VisitDescription(Description description)
        {
            if (delegates.VisitDescription != null)
                delegates.VisitDescription(description);

            base.VisitDescription(description);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitElement"/>.
        /// </summary>
        protected override void VisitElement(Element element)
        {
            if (delegates.VisitElement != null)
                delegates.VisitElement(element);

            base.VisitElement(element);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitEnum"/>.
        /// </summary>
        public override void VisitEnum(Enum type)
        {
            if (delegates.VisitEnum != null)
                delegates.VisitEnum(type);

            base.VisitEnum(type);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitEvent"/>.
        /// </summary>
        public override void VisitEvent(Event @event)
        {
            if (delegates.VisitEvent != null)
                delegates.VisitEvent(@event);

            base.VisitEvent(@event);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitExample"/>.
        /// </summary>
        public override void VisitExample(Example example)
        {
            if (delegates.VisitExample != null)
                delegates.VisitExample(example);

            base.VisitExample(example);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitException"/>.
        /// </summary>
        public override void VisitException(Exception exception)
        {
            if (delegates.VisitException != null)
                delegates.VisitException(exception);

            base.VisitException(exception);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitExtensionMethod"/>.
        /// </summary>
        public override void VisitExtensionMethod(ExtensionMethod method)
        {
            if (delegates.VisitExtensionMethod != null)
                delegates.VisitExtensionMethod(method);

            base.VisitExtensionMethod(method);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitField"/>.
        /// </summary>
        public override void VisitField(Field field)
        {
            if (delegates.VisitField != null)
                delegates.VisitField(field);

            base.VisitField(field);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitInterface"/>.
        /// </summary>
        public override void VisitInterface(Interface type)
        {
            if (delegates.VisitInterface != null)
                delegates.VisitInterface(type);

            base.VisitInterface(type);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitItem"/>.
        /// </summary>
        public override void VisitItem(Item item)
        {
            if (delegates.VisitItem != null)
                delegates.VisitItem(item);

            base.VisitItem(item);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitList"/>.
        /// </summary>
        public override void VisitList(List list)
        {
            if (delegates.VisitList != null)
                delegates.VisitList(list);

            base.VisitList(list);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitListHeader"/>.
        /// </summary>
        public override void VisitListHeader(ListHeader header)
        {
            if (delegates.VisitListHeader != null)
                delegates.VisitListHeader(header);

            base.VisitListHeader(header);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitMember"/>.
        /// </summary>
        public override void VisitMember(Member member)
        {
            if (delegates.VisitMember != null)
                delegates.VisitMember(member);

            base.VisitMember(member);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitMethod"/>.
        /// </summary>
        public override void VisitMethod(Method method)
        {
            if (delegates.VisitMethod != null)
                delegates.VisitMethod(method);

            base.VisitMethod(method);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitPara"/>.
        /// </summary>
        public override void VisitPara(Para para)
        {
            if (delegates.VisitPara != null)
                delegates.VisitPara(para);

            base.VisitPara(para);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitParam"/>.
        /// </summary>
        public override void VisitParam(Param param)
        {
            if (delegates.VisitParam != null)
                delegates.VisitParam(param);

            base.VisitParam(param);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitParamRef"/>.
        /// </summary>
        public override void VisitParamRef(ParamRef paramRef)
        {
            if (delegates.VisitParamRef != null)
                delegates.VisitParamRef(paramRef);

            base.VisitParamRef(paramRef);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitProperty"/>.
        /// </summary>
        public override void VisitProperty(Property property)
        {
            if (delegates.VisitProperty != null)
                delegates.VisitProperty(property);

            base.VisitProperty(property);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitRemarks"/>.
        /// </summary>
        public override void VisitRemarks(Remarks remarks)
        {
            if (delegates.VisitRemarks != null)
                delegates.VisitRemarks(remarks);

            base.VisitRemarks(remarks);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitSee"/>.
        /// </summary>
        public override void VisitSee(See see)
        {
            if (delegates.VisitSee != null)
                delegates.VisitSee(see);

            base.VisitSee(see);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitSeeAlso"/>.
        /// </summary>
        public override void VisitSeeAlso(SeeAlso seeAlso)
        {
            if (delegates.VisitSeeAlso != null)
                delegates.VisitSeeAlso(seeAlso);

            base.VisitSeeAlso(seeAlso);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitStruct"/>.
        /// </summary>
        public override void VisitStruct(Struct type)
        {
            if (delegates.VisitStruct != null)
                delegates.VisitStruct(type);

            base.VisitStruct(type);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitSummary"/>.
        /// </summary>
        public override void VisitSummary(Summary summary)
        {
            if (delegates.VisitSummary != null)
                delegates.VisitSummary(summary);

            base.VisitSummary(summary);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitTerm"/>.
        /// </summary>
        public override void VisitTerm(Term term)
        {
            if (delegates.VisitTerm != null)
                delegates.VisitTerm(term);

            base.VisitTerm(term);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitText"/>.
        /// </summary>
        public override void VisitText(Text text)
        {
            if (delegates.VisitText != null)
                delegates.VisitText(text);

            base.VisitText(text);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitType"/>.
        /// </summary>
        public override void VisitType(TypeDeclaration type)
        {
            if (delegates.VisitType != null)
                delegates.VisitType(type);

            base.VisitType(type);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitTypeParam"/>.
        /// </summary>
        public override void VisitTypeParam(TypeParam typeParam)
        {
            if (delegates.VisitTypeParam != null)
                delegates.VisitTypeParam(typeParam);

            base.VisitTypeParam(typeParam);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitTypeParamRef"/>.
        /// </summary>
        public override void VisitTypeParamRef(TypeParamRef typeParamRef)
        {
            if (delegates.VisitTypeParamRef != null)
                delegates.VisitTypeParamRef(typeParamRef);

            base.VisitTypeParamRef(typeParamRef);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitUnknownMember"/>.
        /// </summary>
        public override void VisitUnknownMember(UnknownMember member)
        {
            if (delegates.VisitUnknownMember != null)
                delegates.VisitUnknownMember(member);

            base.VisitUnknownMember(member);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitUnknownElement"/>.
        /// </summary>
        public override void VisitUnknownElement(UnknownElement element)
        {
            if (delegates.VisitUnknownElement != null)
                delegates.VisitUnknownElement(element);

            base.VisitUnknownElement(element);
        }

        /// <summary>
        /// See <see cref="Visitor.VisitValue"/>.
        /// </summary>
        public override void VisitValue(Value value)
        {
            if (delegates.VisitValue != null)
                delegates.VisitValue(value);

            base.VisitValue(value);
        }
    }
}