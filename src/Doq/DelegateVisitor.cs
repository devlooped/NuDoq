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
    using System.Linq;

    public class VisitorDelegates
    {
        public Action<Members> VisitMembers { get; set; }
        public Action<TypeDeclaration> VisitType { get; set; }
        public Action<NestedType> VisitNestedType { get; set; }
        public Action<Interface> VisitInterface { get; set; }
        public Action<Class> VisitClass { get; set; }
        public Action<Struct> VisitStruct { get; set; }
        public Action<Enum> VisitEnum { get; set; }
        public Action<Field> VisitField { get; set; }
        public Action<Property> VisitProperty { get; set; }
        public Action<Event> VisitEvent { get; set; }
        public Action<Method> VisitMethod { get; set; }
        public Action<ExtensionMethod> VisitExtensionMethod { get; set; }
        public Action<Member> VisitMember { get; set; }
        public Action<Summary> VisitSummary { get; set; }
        public Action<Remarks> VisitRemarks { get; set; }
        public Action<Para> VisitPara { get; set; }
        public Action<Code> VisitCode { get; set; }
        public Action<C> VisitC { get; set; }
        public Action<Text> VisitText { get; set; }
        public Action<Example> VisitExample { get; set; }
        public Action<See> VisitSee { get; set; }
        public Action<SeeAlso> VisitSeeAlso { get; set; }
        public Action<Param> VisitParam { get; set; }
        public Action<ParamRef> VisitParamRef { get; set; }
        public Action<TypeParam> VisitTypeParam { get; set; }
        public Action<TypeParamRef> VisitTypeParamRef { get; set; }
        public Action<Value> VisitValue { get; set; }
        public Action<List> VisitList { get; set; }
        public Action<ListHeader> VisitListHeader { get; set; }
        public Action<Term> VisitTerm { get; set; }
        public Action<Description> VisitDescription { get; set; }
        public Action<Item> VisitItem { get; set; }
        public Action<Exception> VisitException { get; set; }
        public Action<UnknownMember> VisitUnknownMember { get; set; }
        public Action<Container> VisitContainer { get; set; }
        public Action<Element> VisitElement { get; set; }
    }

    public class DelegateVisitor : Visitor
    {
        private VisitorDelegates delegates;

        public DelegateVisitor(VisitorDelegates delegates)
        {
            this.delegates = delegates;
        }

        public override void VisitC(C code)
        {
            if (delegates.VisitC != null)
                delegates.VisitC(code);

            base.VisitC(code);
        }

        public override void VisitClass(Class type)
        {
            if (delegates.VisitClass != null)
                delegates.VisitClass(type);

            base.VisitClass(type);
        }

        public override void VisitCode(Code code)
        {
            if (delegates.VisitCode != null)
                delegates.VisitCode(code);

            base.VisitCode(code);
        }

        protected override void VisitContainer(Container container)
        {
            if (delegates.VisitContainer != null)
                delegates.VisitContainer(container);

            base.VisitContainer(container);
        }

        public override void VisitDescription(Description description)
        {
            if (delegates.VisitDescription != null)
                delegates.VisitDescription(description);

            base.VisitDescription(description);
        }

        protected override void VisitElement(Element element)
        {
            if (delegates.VisitElement != null)
                delegates.VisitElement(element);

            base.VisitElement(element);
        }

        public override void VisitEnum(Enum type)
        {
            if (delegates.VisitEnum != null)
                delegates.VisitEnum(type);

            base.VisitEnum(type);
        }

        public override void VisitEvent(Event @event)
        {
            if (delegates.VisitEvent != null)
                delegates.VisitEvent(@event);

            base.VisitEvent(@event);
        }

        public override void VisitExample(Example example)
        {
            if (delegates.VisitExample != null)
                delegates.VisitExample(example);

            base.VisitExample(example);
        }

        public override void VisitException(Exception exception)
        {
            if (delegates.VisitException != null)
                delegates.VisitException(exception);

            base.VisitException(exception);
        }

        public override void VisitExtensionMethod(ExtensionMethod method)
        {
            if (delegates.VisitExtensionMethod != null)
                delegates.VisitExtensionMethod(method);

            base.VisitExtensionMethod(method);
        }

        public override void VisitField(Field field)
        {
            if (delegates.VisitField != null)
                delegates.VisitField(field);

            base.VisitField(field);
        }

        public override void VisitInterface(Interface type)
        {
            if (delegates.VisitInterface != null)
                delegates.VisitInterface(type);

            base.VisitInterface(type);
        }

        public override void VisitItem(Item item)
        {
            if (delegates.VisitItem != null)
                delegates.VisitItem(item);

            base.VisitItem(item);
        }

        public override void VisitList(List list)
        {
            if (delegates.VisitList != null)
                delegates.VisitList(list);

            base.VisitList(list);
        }

        public override void VisitListHeader(ListHeader header)
        {
            if (delegates.VisitListHeader != null)
                delegates.VisitListHeader(header);

            base.VisitListHeader(header);
        }

        public override void VisitMember(Member member)
        {
            if (delegates.VisitMember != null)
                delegates.VisitMember(member);

            base.VisitMember(member);
        }

        public override void VisitMembers(Members members)
        {
            if (delegates.VisitMembers != null)
                delegates.VisitMembers(members);

            base.VisitMembers(members);
        }

        public override void VisitMethod(Method method)
        {
            if (delegates.VisitMethod != null)
                delegates.VisitMethod(method);

            base.VisitMethod(method);
        }

        public override void VisitNestedType(NestedType type)
        {
            if (delegates.VisitNestedType != null)
                delegates.VisitNestedType(type);

            base.VisitNestedType(type);
        }

        public override void VisitPara(Para para)
        {
            if (delegates.VisitPara != null)
                delegates.VisitPara(para);

            base.VisitPara(para);
        }

        public override void VisitParam(Param param)
        {
            if (delegates.VisitParam != null)
                delegates.VisitParam(param);

            base.VisitParam(param);
        }

        public override void VisitParamRef(ParamRef paramRef)
        {
            if (delegates.VisitParamRef != null)
                delegates.VisitParamRef(paramRef);

            base.VisitParamRef(paramRef);
        }

        public override void VisitProperty(Property property)
        {
            if (delegates.VisitProperty != null)
                delegates.VisitProperty(property);

            base.VisitProperty(property);
        }

        public override void VisitRemarks(Remarks remarks)
        {
            if (delegates.VisitRemarks != null)
                delegates.VisitRemarks(remarks);

            base.VisitRemarks(remarks);
        }

        public override void VisitSee(See see)
        {
            if (delegates.VisitSee != null)
                delegates.VisitSee(see);

            base.VisitSee(see);
        }

        public override void VisitSeeAlso(SeeAlso seeAlso)
        {
            if (delegates.VisitSeeAlso != null)
                delegates.VisitSeeAlso(seeAlso);

            base.VisitSeeAlso(seeAlso);
        }

        public override void VisitStruct(Struct type)
        {
            if (delegates.VisitStruct != null)
                delegates.VisitStruct(type);

            base.VisitStruct(type);
        }

        public override void VisitSummary(Summary summary)
        {
            if (delegates.VisitSummary != null)
                delegates.VisitSummary(summary);

            base.VisitSummary(summary);
        }

        public override void VisitTerm(Term term)
        {
            if (delegates.VisitTerm != null)
                delegates.VisitTerm(term);

            base.VisitTerm(term);
        }

        public override void VisitText(Text text)
        {
            if (delegates.VisitText != null)
                delegates.VisitText(text);

            base.VisitText(text);
        }

        public override void VisitType(TypeDeclaration type)
        {
            if (delegates.VisitType != null)
                delegates.VisitType(type);

            base.VisitType(type);
        }

        public override void VisitTypeParam(TypeParam typeParam)
        {
            if (delegates.VisitTypeParam != null)
                delegates.VisitTypeParam(typeParam);

            base.VisitTypeParam(typeParam);
        }

        public override void VisitTypeParamRef(TypeParamRef typeParamRef)
        {
            if (delegates.VisitTypeParamRef != null)
                delegates.VisitTypeParamRef(typeParamRef);

            base.VisitTypeParamRef(typeParamRef);
        }

        public override void VisitUnknownMember(UnknownMember member)
        {
            if (delegates.VisitUnknownMember != null)
                delegates.VisitUnknownMember(member);

            base.VisitUnknownMember(member);
        }

        public override void VisitValue(Value value)
        {
            if (delegates.VisitValue != null)
                delegates.VisitValue(value);

            base.VisitValue(value);
        }
    }
}