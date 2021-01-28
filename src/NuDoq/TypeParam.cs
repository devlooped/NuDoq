using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>typeparam</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/ms173191(v=vs.80).aspx.
    /// </remarks>
    public class TypeParam : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeParam"/> class.
        /// </summary>
        /// <param name="name">The name of the type parameter.</param>
        /// <param name="elements">The elements that make up the documentation.</param>
        public TypeParam(string name, IEnumerable<Element> elements)
            : base(elements)
            => Name = name;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitTypeParam(this);
            return visitor;
        }

        /// <summary>
        /// Gets the name of the type parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<typeparam>" + base.ToString();
    }
}