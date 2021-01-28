using System.Collections.Generic;
using System.Xml.Linq;

namespace NuDoq
{
    /// <summary>
    /// An unsupported or unknown XML documentation element.
    /// </summary>
    public class UnknownElement : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownElement"/> class.
        /// </summary>
        /// <param name="xml">The <see cref="XElement"/> containing the entire element markup.</param>
        /// <param name="content">The child content.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public UnknownElement(XElement xml, IEnumerable<Element> content, IDictionary<string, string> attributes)
            : base(content, attributes)
            => Xml = xml;

        /// <summary>
        /// Gets the <see cref="XElement"/> containing the entire element markup.
        /// </summary>
        public XElement Xml { get; }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitUnknownElement(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "unknown:<" + Xml.Name.LocalName + ">" + base.ToString();
    }
}