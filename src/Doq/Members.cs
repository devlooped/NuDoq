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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Composite of all lazy-read members in a documentation file 
    /// or assembly, returned from the <see cref="Reader"/>.
    /// </summary>
    public class Members : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Members"/> class.
        /// </summary>
        /// <param name="members">The lazily-read members of the set.</param>
        public Members(IEnumerable<Member> members)
            // In .NET 4.0 there's no covariance on IEnumerable.
            : base(members.OfType<Element>())
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitMembers(this);
            return visitor;
        }
    }
}