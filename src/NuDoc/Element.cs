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
    using System.Collections.Generic;

    /// <summary>
    /// Base class for all elements in a documentation file, including 
    /// types, members, and content like summaries, remarks, etc. 
    /// </summary>
    /// <remarks>
    /// This type is the root of the visitor model hierarchy.
    /// </remarks>
    public abstract class Element : ClariusLabs.NuDoc.IVisitable
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : Visitor;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            var visitor = new TextVisitor();
            this.Accept(visitor);
            return visitor.Text;
        }

        /// <summary>
        /// Enumerates this instance and all its descendents recursively.
        /// </summary>
        public IEnumerable<Element> Traverse()
        {
            return this.Accept(new TraverseVisitor()).Elements;
        }

        private class TraverseVisitor : Visitor
        {
            public TraverseVisitor()
            {
                this.Elements = new List<Element>();
            }

            protected override void VisitElement(Element element)
            {
                base.VisitElement(element);
                this.Elements.Add(element);
            }

            public List<Element> Elements { get; set; }
        }
    }
}