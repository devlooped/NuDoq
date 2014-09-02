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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml;

    /// <summary>
    /// Base class for all elements in a documentation file, including 
    /// types, members, and content like summaries, remarks, etc. 
    /// </summary>
    /// <remarks>
    /// This type is the root of the visitor model hierarchy.
    /// </remarks>
    [DebuggerDisplay("{ToText()}")]
    public abstract class Element : IVisitable, IXmlLineInfo
    {
        private IXmlLineInfo lineInfo;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public abstract TVisitor Accept<TVisitor>(TVisitor visitor) where TVisitor : Visitor;

        /// <summary>
        /// Returns the text content of this element and all its 
        /// children if any.
        /// </summary>
        public string ToText()
        {
            var visitor = new TextVisitor();
            this.Accept(visitor);
            return visitor.Text;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            if (this.lineInfo != null && this.lineInfo.HasLineInfo())
                return "(" + this.lineInfo.LineNumber + "," + this.lineInfo.LinePosition + ")";

            return "";
        }

        /// <summary>
        /// Enumerates this instance and all its descendents recursively.
        /// </summary>
        public IEnumerable<Element> Traverse()
        {
            return this.Accept(new TraverseVisitor()).Elements;
        }

        internal void SetLineInfo(IXmlLineInfo lineInfo)
        {
            this.lineInfo = lineInfo;
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

        bool IXmlLineInfo.HasLineInfo()
        {
            return this.lineInfo != null && this.lineInfo.HasLineInfo();
        }

        int IXmlLineInfo.LineNumber
        {
            get { return this.lineInfo == null ? 0 : this.lineInfo.LineNumber; }
        }

        int IXmlLineInfo.LinePosition
        {
            get { return this.lineInfo == null ? 0 : this.lineInfo.LinePosition; }
        }
    }
}