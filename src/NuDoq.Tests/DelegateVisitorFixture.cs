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
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Demo;
    using Xunit;

    public class DelegateVisitorFixture
    {
        private ConcurrentDictionary<PropertyInfo, int> calls = new ConcurrentDictionary<PropertyInfo, int>();

        [Fact]
        public void when_visiting_root_then_visits_all_delegates()
        {
            var calledMethod = this.GetType().GetMethod("Called", BindingFlags.Instance | BindingFlags.NonPublic);
            var delegates = new VisitorDelegates();
            foreach (var prop in typeof(VisitorDelegates).GetProperties())
            {
                var actionTypeParam = prop.PropertyType.GetGenericArguments()[0];
                var expression = Expression.Lambda(
                    prop.PropertyType,
                    Expression.Call(Expression.Constant(this), calledMethod, Expression.Constant(prop)),
                    Expression.Parameter(actionTypeParam));

                prop.SetValue(delegates, expression.Compile());
            }

            var members = DocReader.Read(typeof(Sample).Assembly);
            members.Accept(new DelegateVisitor(delegates));

            foreach (var prop in typeof(VisitorDelegates).GetProperties())
            {
                if (prop == Reflect<VisitorDelegates>.GetProperty(x => x.VisitUnknownMember))
                    continue;

                Assert.True(calls.GetOrAdd(prop, 0) > 0, string.Format("Delegate on {0} was not called.", prop.Name));
            }
        }

        private void Called(PropertyInfo property)
        {
            calls[property] = calls.GetOrAdd(property, 0) + 1;
        }
    }
}