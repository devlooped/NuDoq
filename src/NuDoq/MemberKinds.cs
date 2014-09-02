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
    using System.Linq;

    /// <summary>
    /// Kind (or kinds for semantically-augmented readings)
    /// of member node.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    [Flags]
    public enum MemberKinds
    {
        /// <summary>
        /// The type of member could not be determined.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The member is a type.
        /// </summary>
        Type = 1,

        /// <summary>
        /// The member is a field.
        /// </summary>
        Field = 2,

        /// <summary>
        /// The member is a property.
        /// </summary>
        Property = 4,

        /// <summary>
        /// The member is a method.
        /// </summary>
        Method = 8,
        /// <summary>
        /// The member is an event.
        /// </summary>
        Event = 16,
        
        // Here start the extended member types 
        // available only when you pass in an 
        // assembly for the reading.
        /// <summary>
        /// The member is an extension method. This 
        /// kind is used in conjunction with <see cref="Method"/> 
        /// as a combined flag.
        /// </summary>
        ExtensionMethod = 32,

        /// <summary>
        /// The member is an enumeration. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Enum = 128,

        /// <summary>
        /// The member is an interface. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Interface = 256,

        /// <summary>
        /// The member is a class. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Class = 512,

        /// <summary>
        /// The member is a struct. This 
        /// kind is used in conjunction with <see cref="Type"/> 
        /// as a combined flag.
        /// </summary>
        Struct = 1024,
    }
}