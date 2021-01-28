namespace NuDoq
{
    /// <summary>
    /// Represents the <c>code</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/f8hahtxf(v=vs.80).aspx.
    /// </remarks>
    public class Code : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Code"/> class 
        /// with the given content.
        /// </summary>
        public Code(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitCode(this);
            return visitor;
        }

        /// <summary>
        /// Gets the code content.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<code>" + base.ToString();
        }
    }
}