using System.Collections.Generic;
using System.Linq;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>listheader</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/y3ww3c7e(v=vs.80).aspx.
    /// </remarks>
    public class ListHeader : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListHeader"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public ListHeader(IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(elements, attributes)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitListHeader(this);
            return visitor;
        }

        /// <summary>
        /// Gets the term from the contained elements, if any.
        /// </summary>
        public Term Term => Elements.OfType<Term>().FirstOrDefault();

        /// <summary>
        /// Gets the description from the contained elements, if any.
        /// </summary>
        public Description Description => Elements.OfType<Description>().FirstOrDefault();

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<listheader>" + base.ToString();
    }
}