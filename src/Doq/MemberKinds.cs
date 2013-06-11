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
    using System.Linq;

    /// <summary>
    /// Kind (or kinds for semantically-augmented readings) 
    /// of member node.
    /// </summary>
    [Flags]
    public enum MemberKinds
    {
        Unknown = 0,
        Type = 1,
        Field = 2,
        Property = 4,
        Method = 8,
        Event = 16,
        // Here start the extended member types 
        // available only when you pass in an 
        // assembly for the reading.
        ExtensionMethod = 32,
        NestedType = 64,
        Enum = 128,
        Interface = 256,
        Class = 512,
        Struct = 1024,
        /*
T
type: class, interface, struct, enum, delegate
D
typedef
F
field
P
property (including indexers or other indexed properties)
M
method (including such special methods as constructors, operators, and so forth)
E
event         
        */
    }
}