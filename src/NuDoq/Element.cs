﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace NuDoq
{
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
        IXmlLineInfo? lineInfo;

        /// <summary>
        /// Initializes the element with the given attributes.
        /// </summary>
        /// <param name="attributes">The attributes of the element, if any.</param>
        protected Element(IDictionary<string, string> attributes) => Attributes = attributes;

        /// <summary>
        /// Gets all the attributes of the element.
        /// </summary>
        public IDictionary<string, string> Attributes { get; }

        /// <summary>
        /// Enumerates this instance and all its descendents recursively.
        /// </summary>
        public IEnumerable<Element> Traverse() => Accept(new TraverseVisitor()).Elements;

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
            Accept(visitor);
            return visitor.Text;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            if (lineInfo != null && lineInfo.HasLineInfo())
                return "(" + lineInfo.LineNumber + "," + lineInfo.LinePosition + ")";

            return "";
        }

        /// <summary>
        /// Gets a value indicating whether the class can return line information.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="Element.LineNumber"/> and <see cref="Element.LinePosition"/> 
        /// can be provided; otherwise, <c>false</c>.
        /// </returns>
        protected bool HasLineInfo() => lineInfo != null && lineInfo.HasLineInfo();

        /// <summary>
        /// Gets the current line number.
        /// </summary>
        protected int LineNumber => lineInfo == null ? 0 : lineInfo.LineNumber;

        /// <summary>
        /// Gets the current line position.
        /// </summary>
        protected int LinePosition => lineInfo == null ? 0 : lineInfo.LinePosition;

        internal void SetLineInfo(IXmlLineInfo lineInfo) => this.lineInfo = lineInfo;

        bool IXmlLineInfo.HasLineInfo() => HasLineInfo();

        int IXmlLineInfo.LineNumber => LineNumber;

        int IXmlLineInfo.LinePosition => LinePosition;

        class TraverseVisitor : Visitor
        {
            public TraverseVisitor() => Elements = new List<Element>();

            protected override void VisitElement(Element element)
            {
                base.VisitElement(element);
                Elements.Add(element);
            }

            public List<Element> Elements { get; set; }
        }
    }
}