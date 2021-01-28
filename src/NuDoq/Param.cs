using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>param</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/8cw818w8(v=vs.80).aspx.
    /// </remarks>
    public class Param : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Param"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="elements">The elements that make up the parameter documentation.</param>
        public Param(string name, IEnumerable<Element> elements)
            : base(elements)
            => Name = name;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitParam(this);
            return visitor;
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<param>" + base.ToString();
    }
}