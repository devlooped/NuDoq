namespace NuDoq
{
    /// <summary>
    /// Represents the <c>typeparamref</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/ms173192(v=vs.80).aspx.
    /// </remarks>
    public class TypeParamRef : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeParamRef"/> class.
        /// </summary>
        /// <param name="name">The name of the referenced type parameter.</param>
        public TypeParamRef(string name) => Name = name;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitTypeParamRef(this);
            return visitor;
        }

        /// <summary>
        /// Gets the name of the referenced type parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<typeparamref>" + base.ToString();
    }
}