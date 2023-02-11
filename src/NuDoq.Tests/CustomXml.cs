using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuDoq
{
    /// <summary>
    /// This element contains 
    /// <custom id="value">
    /// element with <nested class="help">nested</nested> elements.
    /// </custom>
    /// </summary>
    /// <remarks>
    /// <code source="foo.cs" region="example" />
    /// </remarks>
    public class CustomXml
    {
        ///     <summary>
        ///   Begin
        ///  End
        ///     </summary>
        public void WeirdIndenting() { }

        /// <preliminary />
        public void Preliminary() { }

        /// <summary>
        /// <![CDATA[<><> character data]]>
        /// </summary>
        public void HasCData() { }

        /// <summary>example method</summary>
        /// <param name="p1">byref</param>
        public void ByRef(ref int p1) { }


        /// <summary>example method</summary>
        /// <param name="p1">out</param>
        public void Out(out int p1)
        {
            p1 = default;
        }

        /// <summary>
        /// With
        /// 
        /// new
        /// 
        /// 
        /// lines
        /// </summary>
        public string NewLines { get; set; }

        /// <summary>indexed</summary>
        public int this[int index]
        {
            get => 0;
            set => Console.WriteLine(value);
        }
    }
}
