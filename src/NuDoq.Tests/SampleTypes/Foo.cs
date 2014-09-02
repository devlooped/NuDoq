namespace NuDoq.SampleTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Foo<T, TMetadata>
    {
        /// <summary>
        /// Some method
        /// </summary>
        /// <param name="x">The x param</param>
        public void SomeMethod<T>(ref string x)
        {
        }
    }
}
