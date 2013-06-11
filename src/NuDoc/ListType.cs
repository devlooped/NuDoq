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
    using System;
    using System.Linq;

    /// <summary>
    /// Specifies the type of <see cref="List" /> in use.
    /// </summary>
    public enum ListType
    {
        /// <summary>
        /// The bullet list type.
        /// </summary>
        Bullet,
        
        /// <summary>
        /// The number list type.
        /// </summary>
        Number,
        
        /// <summary>
        /// The table list type.
        /// </summary>
        Table,

        /// <summary>
        /// The list type could not be determined.
        /// </summary>
        Unknown,
    }
}