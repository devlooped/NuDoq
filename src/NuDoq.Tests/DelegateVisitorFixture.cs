using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Demo;
using Xunit;

namespace NuDoq
{
    public class DelegateVisitorFixture
    {
        ConcurrentDictionary<PropertyInfo, int> calls = new ConcurrentDictionary<PropertyInfo, int>();

        [Fact]
        public void when_visiting_root_then_visits_all_delegates()
        {
            var calledMethod = GetType().GetMethod("Called", BindingFlags.Instance | BindingFlags.NonPublic);
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

        void Called(PropertyInfo property)
        {
            calls[property] = calls.GetOrAdd(property, 0) + 1;
        }
    }
}