namespace NuDoq
{
    /// <summary>
    /// Represents the <c>c</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/te6h7cxs(v=vs.80).aspx.
    /// </remarks>
    public class C : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="C"/> class 
        /// with the given content.
        /// </summary>
        public C(string content) => Content = content;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitC(this);
            return visitor;
        }

        /// <summary>
        /// Gets the code content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<c>" + base.ToString();
    }
}