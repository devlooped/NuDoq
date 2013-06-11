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
    using System.Text;

    /// <summary>
    /// Visitor that is used to render ToString for elements.
    /// </summary>
    internal class TextVisitor : Visitor
    {
        private StringBuilder builder = new StringBuilder();

        public override void VisitC(C code)
        {
            base.VisitC(code);
            builder.Append(code.Content);
        }

        public override void VisitCode(Code code)
        {
            base.VisitCode(code);
            builder.AppendLine().Append(code.Content).AppendLine();
        }

        public override void VisitText(Text text)
        {
            base.VisitText(text);
            builder.Append(text.Content);
        }

        public override void VisitParamRef(ParamRef paramRef)
        {
            base.VisitParamRef(paramRef);
            if (!string.IsNullOrEmpty(paramRef.Name))
                builder.Append(paramRef.Name);
        }

        public override void VisitTypeParamRef(TypeParamRef typeParamRef)
        {
            base.VisitTypeParamRef(typeParamRef);
            if (!string.IsNullOrEmpty(typeParamRef.Name))
                builder.Append(typeParamRef.Name);
        }

        public override void VisitSee(See see)
        {
            base.VisitSee(see);
            if (!string.IsNullOrEmpty(see.Cref))
                builder.Append(see.Cref.Substring(2));
        }

        public override void VisitPara(Para para)
        {
            // Avoid double line breaks between adjacent <para> elements.
            if (builder.Length < 2 ||
                new string(new char[] { builder[builder.Length - 2], builder[builder.Length - 1] }) != Environment.NewLine)
            {
                builder.AppendLine();
            }

            base.VisitPara(para);
            builder.AppendLine();
        }

        public string Text { get { return builder.ToString(); } }
    }
}