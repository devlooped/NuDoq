using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Semantically augmented <see cref="Method" /> for extension methods,
    /// available when using an <see cref="System.Reflection.Assembly" />
    /// with the <see cref="DocReader" />.
    /// </summary>
    public class ExtensionMethod : Method
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionMethod"/> class.
        /// </summary>
        /// <param name="memberId">The extension method member id.</param>
        /// <param name="extendedTypeId">The extended type id (the <c>this</c> parameter).</param>
        /// <param name="elements">The contained documentation elements.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public ExtensionMethod(string memberId, string extendedTypeId, IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(memberId, elements, attributes)
            => ExtendedTypeId = extendedTypeId;

        /// <summary>
        /// Gets the extended type id (the <c>this</c> parameter).
        /// </summary>
        public string ExtendedTypeId { get; }

        /// <summary>
        /// Gets the kind of member, which contains both the <see cref="MemberKinds.Method" /> and 
        /// <see cref="MemberKinds.ExtensionMethod"/> flags.
        /// </summary>
        public override MemberKinds Kind => MemberKinds.Method | MemberKinds.ExtensionMethod;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitExtensionMethod(this);
            return visitor;
        }
    }
}