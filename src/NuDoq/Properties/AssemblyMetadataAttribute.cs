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

namespace System.Reflection
{
    /// <summary>Defines a key/value metadata pair for the decorated assembly.</summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class AssemblyMetadataAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyMetadataAttribute" /> class by using the specified metadata key and value.</summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public AssemblyMetadataAttribute(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>Gets the metadata key.</summary>
        /// <returns>The metadata key.</returns>
        public string Key { get; private set; }

        /// <summary>Gets the metadata value.</summary>
        /// <returns>The metadata value.</returns>
        public string Value { get; private set; }
    }
}