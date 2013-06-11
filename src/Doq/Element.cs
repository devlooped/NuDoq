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
    using System.Collections.Generic;

    public abstract class Element
    {
        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : Visitor;

        public override string ToString()
        {
            var visitor = new TextVisitor();
            this.Accept(visitor);
            return visitor.Text;
        }

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