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

namespace ClariusLabs.Demo
{
    using System;

    /// <summary>
    /// Sample API documentation. This is the summary.
    /// With every line we will have the same 
    /// issue. This should all go to a single 
    /// non-breaking line.
    /// </summary>
    /// <example>
    /// What follows is an example:
    /// <code>
    /// var code = new ThisIsCode();
    /// </code>
    /// And this is an inline <c>c tag</c> within an example.
    /// </example>
    /// <remarks>
    /// This is the remarks section, which can also have <c>c tag</c> code.
    /// <code>
    /// var code = new SomeCodeTagWithinRemarks();
    /// </code>
    /// You can use <see cref="Provider"/> see tag within sections.
    /// <para>
    /// We can have paragraphs anywhere.
    /// </para>
    /// </remarks>
    /// <seealso cref="Provider"/>
    public class Sample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sample"/> class.
        /// </summary>
        public Sample()
        {
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id of this sample.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets the value for the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id to get the value for.</param>
        /// <returns><see langword="true"/> if the value <c>true</c> (with c tag); <see langword="false"/> otherwise.</returns>
        public bool GetValue(int id)
        {
            return false;
        }

        /// <summary>
        /// Generic method on non-generic type.
        /// </summary>
        public T Do<T>(Func<T> func)
        {
            return default(T);
        }

        /// <summary>
        /// Generic method on non-generic type, accepting one-dimensional array.
        /// </summary>
        public T DoWithArray1<T>(T[] array) {
            return default(T);
        }

        /// <summary>
        /// Generic method on non-generic type, accepting two-dimensional array.
        /// </summary>
        public T DoWithArray2<T>(T[,] array) {
            return default(T);
        }

        /// <summary>
        /// A nested type
        /// </summary>
        public class NestedType
        {
            /// <summary>
            /// Gets or sets the nested type property.
            /// </summary>
            public int NestedTypeProperty { get; set; }
        }

        /// <summary>
        /// This is not added to the type map.
        /// </summary>
        private class PrivateNestedType
        {
        }
    }
}