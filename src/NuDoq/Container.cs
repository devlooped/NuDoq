using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Base class for elements that can contain other elements.
    /// </summary>
    /// <remarks>
    /// Implements the composite pattern for the visitable model.
    /// </remarks>
    public abstract class Container : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public Container(IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(attributes)
            => Elements = elements.Cached();

        /// <summary>
        /// Gets the elements contained in this instance.
        /// </summary>
        public IEnumerable<Element> Elements { get; }
    }
}