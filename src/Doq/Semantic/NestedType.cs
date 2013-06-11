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

namespace ClariusLabs.Doq
{
    using System.Collections.Generic;

    /// <summary>
    /// Semantically augmented <see cref="TypeDeclaration" /> for
    /// nested types, available when using an <see cref="System.Reflection.Assembly" />
    /// with the <see cref="Reader" />.
    /// </summary>
    public class NestedType : TypeDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedType"/> class.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <param name="declaringTypeId">The declaring type id.</param>
        /// <param name="elements">The elements.</param>
        public NestedType(string memberId, string declaringTypeId, IEnumerable<Element> elements)
            : base(memberId, elements)
        {
            this.DeclaringTypeId = declaringTypeId;
        }

        /// <summary>
        /// Gets the declaring type identifier.
        /// </summary>
        public string DeclaringTypeId { get; private set; }

        /// <summary>
        /// Gets the kind of member, which contains both the <see cref="MemberKinds.Type" /> and 
        /// <see cref="MemberKinds.NestedType"/> flags.
        /// </summary>
        public override MemberKinds Kind { get { return MemberKinds.Type | MemberKinds.NestedType; } }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitNestedType(this);
            return visitor;
        }
    }
}