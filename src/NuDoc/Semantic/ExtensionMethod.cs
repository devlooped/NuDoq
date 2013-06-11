#region Apache Licensed
/*
 Copyright 2013 Clarius Consulting, Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

namespace ClariusLabs.NuDoc
{
    using System.Collections.Generic;

    /// <summary>
    /// Semantically augmented <see cref="Method" /> for extension methods,
    /// available when using an <see cref="System.Reflection.Assembly" />
    /// with the <see cref="Reader" />.
    /// </summary>
    public class ExtensionMethod : Method
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionMethod"/> class.
        /// </summary>
        /// <param name="memberId">The extension method member id.</param>
        /// <param name="extendedTypeId">The extended type id (the <c>this</c> parameter).</param>
        /// <param name="elements">The contained documentation elements.</param>
        public ExtensionMethod(string memberId, string extendedTypeId, IEnumerable<Element> elements)
            : base(memberId, elements)
        {
            this.ExtendedTypeId = extendedTypeId;
        }

        /// <summary>
        /// Gets the extended type id (the <c>this</c> parameter).
        /// </summary>
        public string ExtendedTypeId { get; private set; }

        /// <summary>
        /// Gets the kind of member, which contains both the <see cref="MemberKinds.Method" /> and 
        /// <see cref="MemberKinds.ExtensionMethod"/> flags.
        /// </summary>
        public override MemberKinds Kind { get { return MemberKinds.Method | MemberKinds.ExtensionMethod; } }

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