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

namespace NuDoq
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An unsupported or unknown member, such as those prefixed 
    /// with "N:" or "!:".
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    public class UnknownMember : Member
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownMember"/> class.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        public UnknownMember(string memberId)
            : base(memberId, Enumerable.Empty<Element>())
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitUnknownMember(this);
            return visitor;
        }

        /// <summary>
        /// Gets the kind of member, which equals <see cref="MemberKinds.Unknown"/>.
        /// </summary>
        public override MemberKinds Kind { get { return MemberKinds.Unknown; } }
    }
}