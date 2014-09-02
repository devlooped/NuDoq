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

namespace Demo
{
    using System;

    /// <summary>
    /// Sample with generic type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The T</typeparam>
    /// <typeparam name="S"></typeparam>
    public class SampleGeneric<T, S>
    {
        /// <summary>
        /// Does the specified <paramref name="func" />.
        /// </summary>
        /// <typeparam name="R">R</typeparam>
        /// <param name="func">The func T.</param>
        /// <returns>
        /// Returns
        /// </returns>
        public R Do<R>(Func<T, S, R> func)
        {
            return default(R);
        }
    }
}