using System.Collections.Generic;
using System.Linq;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>item</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/y3ww3c7e(v=vs.80).aspx.
    /// </remarks>
    public class Item : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        public Item(IEnumerable<Element> elements)
            : base(elements)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitItem(this);
            return visitor;
        }

        /// <summary>
        /// Gets the term from the contained elements, if any.
        /// </summary>
        public Term Term { get { return Elements.OfType<Term>().FirstOrDefault(); } }

        /// <summary>
        /// Gets the description from the contained elements, if any.
        /// </summary>
        public Description Description { get { return Elements.OfType<Description>().FirstOrDefault(); } }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<item>" + base.ToString();
        }
    }
}