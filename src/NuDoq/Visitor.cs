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
    using System.Reflection;

    /// <summary>
    /// Base class for visitors of the visitable documentation 
    /// model.
    /// </summary>
    public abstract class Visitor
    {
        /// <summary>
        /// Visits the entire set of members read by the <see cref="DocReader.Read(string)"/>.
        /// </summary>
        public virtual void VisitDocument(DocumentMembers document)
        {
            VisitContainer(document);
        }

        /// <summary>
        /// Visits the entire set of members read by the <see cref="DocReader.Read(Assembly)"/>.
        /// </summary>
        public virtual void VisitAssembly(AssemblyMembers assembly)
        {
            VisitDocument(assembly);
        }

        /// <summary>
        /// Visits a type member.
        /// </summary>
        /// <remarks>
        /// If reflection metadata augmentation exists, 
        /// this method will also be called for the 
        /// <see cref="TypeDeclaration"/>-derived semantic 
        /// classes <see cref="Class"/>, <see cref="Enum"/>, 
        /// <see cref="Interface"/> and <see cref="Struct"/>.
        /// </remarks>
        public virtual void VisitType(TypeDeclaration type)
        {
            VisitMember(type);
        }

        /// <summary>
        /// Visits the semantically augmented interface type member.
        /// </summary>
        /// <remarks>
        /// This member will only be called when using an 
        /// <see cref="System.Reflection.Assembly"/> as input for 
        /// the <see cref="DocReader"/> so that this augmentation is 
        /// available.
        /// </remarks>
        public virtual void VisitInterface(Interface type)
        {
            VisitType(type);
        }

        /// <summary>
        /// Visits the semantically augmented class type member.
        /// </summary>
        /// <remarks>
        /// This member will only be called when using an 
        /// <see cref="System.Reflection.Assembly"/> as input for 
        /// the <see cref="DocReader"/> so that this augmentation is 
        /// available.
        /// </remarks>
        public virtual void VisitClass(Class type)
        {
            VisitType(type);
        }

        /// <summary>
        /// Visits the semantically augmented struct type member.
        /// </summary>
        /// <remarks>
        /// This member will only be called when using an 
        /// <see cref="System.Reflection.Assembly"/> as input for 
        /// the <see cref="DocReader"/> so that this augmentation is 
        /// available.
        /// </remarks>
        public virtual void VisitStruct(Struct type)
        {
            VisitType(type);
        }

        /// <summary>
        /// Visits the semantically augmented enum type member.
        /// </summary>
        /// <remarks>
        /// This member will only be called when using an 
        /// <see cref="System.Reflection.Assembly"/> as input for 
        /// the <see cref="DocReader"/> so that this augmentation is 
        /// available.
        /// </remarks>
        public virtual void VisitEnum(Enum type)
        {
            VisitType(type);
        }

        /// <summary>
        /// Visits a documented field member.
        /// </summary>
        public virtual void VisitField(Field field)
        {
            VisitMember(field);
        }

        /// <summary>
        /// Visits a documented property member.
        /// </summary>
        public virtual void VisitProperty(Property property)
        {
            VisitMember(property);
        }

        /// <summary>
        /// Visits a documented event member.
        /// </summary>
        public virtual void VisitEvent(Event @event)
        {
            VisitMember(@event);
        }

        /// <summary>
        /// Visits a documented method member.
        /// </summary>
        /// <remarks>
        /// This method will also be called for the semantically augmented 
        /// <see cref="ExtensionMethod"/> if an <see cref="System.Reflection.Assembly"/> 
        /// is used with the <see cref="DocReader"/>.
        /// </remarks>
        public virtual void VisitMethod(Method method)
        {
            VisitMember(method);
        }

        /// <summary>
        /// Visits the semantically augmented extension method member.
        /// </summary>
        /// <remarks>
        /// This member will only be called when using an 
        /// <see cref="System.Reflection.Assembly"/> as input for 
        /// the <see cref="DocReader"/> so that this augmentation is 
        /// available.
        /// </remarks>
        public virtual void VisitExtensionMethod(ExtensionMethod method)
        {
            VisitMethod(method);
        }

        /// <summary>
        /// Visit the generic base class <see cref="Member"/>.
        /// </summary>
        /// <remarks>
        /// This method is called for all <see cref="Member"/>-derived 
        /// types.
        /// </remarks>
        public virtual void VisitMember(Member member)
        {
            VisitContainer(member);
        }

        /// <summary>
        /// Visits the <c>summary</c> documentation element.
        /// </summary>
        public virtual void VisitSummary(Summary summary)
        {
            VisitContainer(summary);
        }

        /// <summary>
        /// Visits the <c>remarks</c> documentation element.
        /// </summary>
        public virtual void VisitRemarks(Remarks remarks)
        {
            VisitContainer(remarks);
        }

        /// <summary>
        /// Visits the <c>para</c> documentation element.
        /// </summary>
        public virtual void VisitPara(Para para)
        {
            VisitContainer(para);
        }

        /// <summary>
        /// Visits the <c>code</c> documentation element.
        /// </summary>
        public virtual void VisitCode(Code code)
        {
            VisitElement(code);
        }

        /// <summary>
        /// Visits the <c>c</c> documentation element.
        /// </summary>
        public virtual void VisitC(C code)
        {
            VisitElement(code);
        }

        /// <summary>
        /// Visits the literal text inside other documentation elements.
        /// </summary>
        public virtual void VisitText(Text text)
        {
            VisitElement(text);
        }

        /// <summary>
        /// Visits the <c>example</c> documentation element.
        /// </summary>
        public virtual void VisitExample(Example example)
        {
            VisitContainer(example);
        }

        /// <summary>
        /// Visits the <c>see</c> documentation element.
        /// </summary>
        public virtual void VisitSee(See see)
        {
            VisitElement(see);
        }

        /// <summary>
        /// Visits the <c>seealso</c> documentation element.
        /// </summary>
        public virtual void VisitSeeAlso(SeeAlso seeAlso)
        {
            VisitElement(seeAlso);
        }

        /// <summary>
        /// Visits the <c>param</c> documentation element.
        /// </summary>
        public virtual void VisitParam(Param param)
        {
            VisitContainer(param);
        }

        /// <summary>
        /// Visits the <c>paramref</c> documentation elemnet.
        /// </summary>
        public virtual void VisitParamRef(ParamRef paramRef)
        {
            VisitElement(paramRef);
        }

        /// <summary>
        /// Visits the <c>typeparam</c> documentation element.
        /// </summary>
        public virtual void VisitTypeParam(TypeParam typeParam)
        {
            VisitContainer(typeParam);
        }

        /// <summary>
        /// Visits the <c>typeparamref</c> documentation element.
        /// </summary>
        public virtual void VisitTypeParamRef(TypeParamRef typeParamRef)
        {
            VisitElement(typeParamRef);
        }

        /// <summary>
        /// Visits the <c>value</c> documentation element.
        /// </summary>
        public virtual void VisitValue(Value value)
        {
            VisitContainer(value);
        }

        /// <summary>
        /// Visits the <c>list</c> documentation element.
        /// </summary>
        public virtual void VisitList(List list)
        {
            VisitContainer(list);
        }

        /// <summary>
        /// Visits the <c>listheader</c> documentation element.
        /// </summary>
        public virtual void VisitListHeader(ListHeader header)
        {
            VisitContainer(header);
        }

        /// <summary>
        /// Visits the <c>term</c> documentation element.
        /// </summary>
        public virtual void VisitTerm(Term term)
        {
            VisitContainer(term);
        }

        /// <summary>
        /// Visits the <c>description</c> documentation element.
        /// </summary>
        public virtual void VisitDescription(Description description)
        {
            VisitContainer(description);
        }

        /// <summary>
        /// Visits the <c>item</c> documentation element.
        /// </summary>
        public virtual void VisitItem(Item item)
        {
            VisitContainer(item);
        }

        /// <summary>
        /// Visits the <c>exception</c> documentation element.
        /// </summary>
        public virtual void VisitException(Exception exception)
        {
            VisitContainer(exception);
        }

        /// <summary>
        /// Visits the <c>returns</c> documentation element.
        /// </summary>
        public virtual void VisitReturns(Returns returns)
        {
            VisitContainer(returns);
        }

        /// <summary>
        /// Visits an unknown member element.
        /// </summary>
        public virtual void VisitUnknownMember(UnknownMember member)
        {
            VisitContainer(member);
        }

        /// <summary>
        /// Visits an unknown documentation element.
        /// </summary>
        public virtual void VisitUnknownElement(UnknownElement element)
        {
            VisitContainer(element);
        }

        /// <summary>
        /// Visits any container element.
        /// </summary>
        /// <remarks>
        /// This method is called for all of the <see cref="Container"/>-derived 
        /// types.
        /// </remarks>
        protected virtual void VisitContainer(Container container)
        {
            VisitElement(container);
            foreach (var element in container.Elements)
            {
                element.Accept(this);
            }
        }

        /// <summary>
        /// Visits any element.
        /// </summary>
        /// <remarks>
        /// This method is called for all of the <see cref="Element"/>-derived 
        /// types.
        /// </remarks>
        protected virtual void VisitElement(Element element)
        {
        }
    }
}