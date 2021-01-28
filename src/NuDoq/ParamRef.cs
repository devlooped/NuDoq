namespace NuDoq
{
    /// <summary>
    /// Represents the <c>paramref</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/wb7x2fhw(v=vs.80).aspx.
    /// </remarks>
    public class ParamRef : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParamRef"/> class.
        /// </summary>
        /// <param name="name">The name of the referenced parameter.</param>
        public ParamRef(string name) => Name = name;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitParamRef(this);
            return visitor;
        }

        /// <summary>
        /// Gets the name of the referenced parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<paramref>" + base.ToString();
    }
}