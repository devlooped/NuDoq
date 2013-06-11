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
    using System.Reflection;

    public abstract class Member : Container
    {
        public Member(string memberId, IEnumerable<Element> elements)
            : base(elements)
        {
            this.Id = memberId;
        }

        public string Id { get; private set; }
        public abstract MemberKinds Kind { get; }

        /// <summary>
        /// Gets the reflection information for this member, 
        /// if the reading process used an assembly.
        /// </summary>
        public MemberInfo Info { get; set; }

        public override string ToString()
        {
            return this.Id;
        }
    }
}