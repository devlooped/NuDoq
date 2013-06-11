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
    public class Visitor
    {
        public virtual void VisitMembers(Members members)
        {
            VisitContainer(members);
        }

        public virtual void VisitType(TypeDeclaration type)
        {
            VisitMember(type);
        }

        public virtual void VisitNestedType(NestedType type)
        {
            VisitType(type);
        }

        public virtual void VisitInterface(Interface type)
        {
            VisitType(type);
        }
        
        public virtual void VisitClass(Class type)
        {
            VisitType(type);
        }

        public virtual void VisitStruct(Struct type)
        {
            VisitType(type);
        }

        public virtual void VisitEnum(Enum type)
        {
            VisitType(type);
        }

        public virtual void VisitField(Field field)
        {
            VisitMember(field);
        }

        public virtual void VisitProperty(Property property)
        {
            VisitMember(property);
        }

        public virtual void VisitEvent(Event @event)
        {
            VisitMember(@event);
        }

        public virtual void VisitMethod(Method method)
        {
            VisitMember(method);
        }

        public virtual void VisitExtensionMethod(ExtensionMethod method)
        {
            VisitMember(method);
        }

        public virtual void VisitMember(Member member)
        {
            VisitContainer(member);
        }

        public virtual void VisitSummary(Summary summary)
        {
            VisitContainer(summary);
        }

        public virtual void VisitRemarks(Remarks remarks)
        {
            VisitContainer(remarks);
        }

        public virtual void VisitPara(Para para)
        {
            VisitContainer(para);
        }

        public virtual void VisitCode(Code code)
        {
            VisitElement(code);
        }

        public virtual void VisitC(C code)
        {
            VisitElement(code);
        }

        public virtual void VisitText(Text text)
        {
            VisitElement(text);
        }

        public virtual void VisitExample(Example example)
        {
            VisitContainer(example);
        }

        public virtual void VisitSee(See see)
        {
            VisitElement(see);
        }

        public virtual void VisitSeeAlso(SeeAlso seeAlso)
        {
            VisitElement(seeAlso);
        }

        public virtual void VisitParam(Param param)
        {
            VisitContainer(param);
        }

        public virtual void VisitParamRef(ParamRef paramRef)
        {
            VisitElement(paramRef);
        }

        public virtual void VisitTypeParam(TypeParam typeParam)
        {
            VisitContainer(typeParam);
        }

        public virtual void VisitTypeParamRef(TypeParamRef typeParamRef)
        {
            VisitElement(typeParamRef);
        }

        public virtual void VisitValue(Value value)
        {
            VisitContainer(value);
        }

        public virtual void VisitList(List list)
        {
            VisitContainer(list);
        }

        public virtual void VisitListHeader(ListHeader header)
        {
            VisitContainer(header);
        }

        public virtual void VisitTerm(Term term)
        {
            VisitContainer(term);
        }

        public virtual void VisitDescription(Description description)
        {
            VisitContainer(description);
        }
        
        public virtual void VisitItem(Item item)
        {
            VisitContainer(item);
        }

        public virtual void VisitException(Exception exception)
        {
            VisitContainer(exception);
        }

        public virtual void VisitUnknownMember(UnknownMember member)
        {
            VisitContainer(member);
        }

        protected virtual void VisitContainer(Container container)
        {
            VisitElement(container);
            foreach (var element in container.Elements)
            {
                element.Accept(this);
            }
        }

        protected virtual void VisitElement(Element element)
        {
        }
    }
}