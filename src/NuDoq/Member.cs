#region Apache Licensed
/*
 Copyright 2013 Daniel Cazzulino

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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Base class for all documentation members: types, 
    /// fields, properties, methods and events.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    public abstract class Member : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Member"/> class.
        /// </summary>
        /// <param name="memberId">The member id as specified in the documentation XML.</param>
        /// <param name="elements">The contained documentation elements.</param>
        public Member(string memberId, IEnumerable<Element> elements)
            : base(elements)
        {
            this.Id = memberId;
        }

        /// <summary>
        /// Gets the member id as specified in the documentation XML.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the kind of member.
        /// </summary>
        public abstract MemberKinds Kind { get; }

        /// <summary>
        /// Gets the reflection information for this member, 
        /// if the reading process used an assembly.
        /// </summary>
        public MemberInfo Info { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return this.Id + " " + base.ToString();
        }
    }
}