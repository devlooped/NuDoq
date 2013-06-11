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

#pragma warning disable 0436
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

/// <summary>
/// Provides strong-typed static reflection for arbitrary 
/// expressions, typically static members where no 
/// instance parameter is needed.
/// </summary>
internal static partial class Reflect
{
	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod(Expression<Action> method)
	{
		return GetMethodInfo(method);
	}

	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod<TResult>(Expression<Func<TResult>> method)
	{
		return GetMethodInfo(method);
	}

	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod(Expression<Func<Action>> method)
	{
		return GetDelegateMethodInfo(method);
	}

	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod<TResult>(Expression<Func<Func<TResult>>> method)
	{
		return GetDelegateMethodInfo(method);
	}

	/// <summary>
	/// Gets the name of the property represented by the lambda expression.
	/// </summary>
	/// <param name="property">An expression that accesses a property.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="property"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="property"/> is not a lambda expression or it does not represent a property access.</exception>
	/// <returns>The property info.</returns>
	public static string GetPropertyName<TResult>(Expression<Func<TResult>> property)
	{
		return GetProperty(property).Name;
	}

	/// <summary>
	/// Gets the property represented by the lambda expression.
	/// </summary>
	/// <param name="property">An expression that accesses a property.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="property"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="property"/> is not a lambda expression or it does not represent a property access.</exception>
	/// <returns>The property info.</returns>
	public static PropertyInfo GetProperty<TResult>(Expression<Func<TResult>> property)
	{
		var info = GetMemberInfo(property) as PropertyInfo;
		if (info == null)
			throw new ArgumentException("Member is not a property");

		return info;
	}

	/// <summary>
	/// Gets the field represented by the lambda expression.
	/// </summary>
	/// <param name="field">An expression that accesses a field.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="field"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="field"/> is not a lambda expression or it does not represent a field access.</exception>
	/// <returns>The field info.</returns>
	public static FieldInfo GetField<TResult>(Expression<Func<TResult>> field)
	{
		var info = GetMemberInfo(field) as FieldInfo;
		if (info == null)
			throw new ArgumentException("Member is not a field");

		return info;
	}

	private static MethodInfo GetMethodInfo(LambdaExpression lambda)
	{
		Guard.NotNull(() => lambda, lambda);

		if (lambda.Body.NodeType != ExpressionType.Call)
			throw new ArgumentException("Not a method call", "lambda");

		return ((MethodCallExpression)lambda.Body).Method;
	}

	private static MethodInfo GetDelegateMethodInfo(LambdaExpression lambda)
	{
		Guard.NotNull(() => lambda, lambda);

		if (lambda.Body.NodeType != ExpressionType.Convert)
			throw new ArgumentException("Not a method reference", "lambda");

		// Do we need all these checks here? The compiler always generates 
		// this same chain of calls for the given call pattern...

		var convertOperand = ((UnaryExpression)lambda.Body).Operand;

		if (convertOperand.NodeType != ExpressionType.Call)
			throw new ArgumentException("Not a method reference", "lambda");

		var createDelegate = (MethodCallExpression)convertOperand;

		if (createDelegate.Arguments.Last().NodeType != ExpressionType.Constant)
			throw new ArgumentException("Not a method reference", "lambda");

		var methodRef = (ConstantExpression)createDelegate.Arguments.Last();

		if (!(methodRef.Value is MethodInfo))
			throw new ArgumentException("Not a method reference", "lambda");

		return (MethodInfo)methodRef.Value;
	}

	private static MemberInfo GetMemberInfo(LambdaExpression lambda)
	{
		Guard.NotNull(() => lambda, lambda);

		if (lambda.Body.NodeType == ExpressionType.MemberAccess)
			return ((MemberExpression)lambda.Body).Member;
		else
			throw new ArgumentException("Not a member access", "lambda");
	}
}

/// <summary>
/// Provides strong-typed static reflection of the <typeparamref name="TTarget"/> 
/// type.
/// </summary>
/// <typeparam name="TTarget">Type to reflect.</typeparam>
internal static partial class Reflect<TTarget>
{
	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod(Expression<Action<TTarget>> method)
	{
		return GetMethodInfo(method);
	}

	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod<TResult>(Expression<Func<TTarget, TResult>> method)
	{
		return GetMethodInfo(method);
	}

	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod<TResult>(Expression<Func<TTarget, Func<TResult>>> method)
	{
		return GetDelegateMethodInfo(method);
	}

	/// <summary>
	/// Gets the method represented by the lambda expression.
	/// </summary>
	/// <param name="method">An expression that invokes a method.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="method"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="method"/> is not a lambda expression or it does not represent a method invocation.</exception>
	/// <returns>The method info.</returns>
	public static MethodInfo GetMethod(Expression<Func<TTarget, Action>> method)
	{
		return GetDelegateMethodInfo(method);
	}

	/// <summary>
	/// Gets the name of the property represented by the lambda expression.
	/// </summary>
	/// <param name="property">An expression that accesses a property.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="property"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="property"/> is not a lambda expression or it does not represent a property access.</exception>
	/// <returns>The property info.</returns>
	public static string GetPropertyName<TResult>(Expression<Func<TTarget, TResult>> property)
	{
		return GetProperty(property).Name;
	}

	/// <summary>
	/// Gets the property represented by the lambda expression.
	/// </summary>
	/// <param name="property">An expression that accesses a property.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="property"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="property"/> is not a lambda expression or it does not represent a property access.</exception>
	/// <returns>The property info.</returns>
	public static PropertyInfo GetProperty<TResult>(Expression<Func<TTarget, TResult>> property)
	{
		var info = GetMemberInfo(property) as PropertyInfo;
		if (info == null)
			throw new ArgumentException("Member is not a property");

		return info;
	}

	/// <summary>
	/// Gets the field represented by the lambda expression.
	/// </summary>
	/// <param name="field">An expression that accesses a field.</param>
	/// <exception cref="ArgumentNullException">The <paramref name="field"/> is null.</exception>
	/// <exception cref="ArgumentException">The <paramref name="field"/> is not a lambda expression or it does not represent a field access.</exception>
	/// <returns>The field info.</returns>
	public static FieldInfo GetField<TResult>(Expression<Func<TTarget, TResult>> field)
	{
		var info = GetMemberInfo(field) as FieldInfo;
		if (info == null)
			throw new ArgumentException("Member is not a field");

		return info;
	}

	private static MethodInfo GetDelegateMethodInfo(LambdaExpression lambda)
	{
		Guard.NotNull(() => lambda, lambda);

		if (lambda.Body.NodeType != ExpressionType.Convert)
			throw new ArgumentException("Not a method reference", "lambda");

		// Do we need all these checks here? The compiler always generates 
		// this same chain of calls for the given call pattern...

		var convertOperand = ((UnaryExpression)lambda.Body).Operand;

		if (convertOperand.NodeType != ExpressionType.Call)
			throw new ArgumentException("Not a method reference", "lambda");

		var createDelegate = (MethodCallExpression)convertOperand;

		if (createDelegate.Arguments.Last().NodeType != ExpressionType.Constant)
			throw new ArgumentException("Not a method reference", "lambda");

		var methodRef = (ConstantExpression)createDelegate.Arguments.Last();

		if (!(methodRef.Value is MethodInfo))
			throw new ArgumentException("Not a method reference", "lambda");

		return (MethodInfo)methodRef.Value;
	}

	private static MethodInfo GetMethodInfo(LambdaExpression lambda)
	{
		Guard.NotNull(() => lambda, lambda);

		if (lambda.Body.NodeType != ExpressionType.Call)
			throw new ArgumentException("Not a method call", "lambda");

		return ((MethodCallExpression)lambda.Body).Method;
	}

	private static MemberInfo GetMemberInfo(LambdaExpression lambda)
	{
		Guard.NotNull(() => lambda, lambda);

		if (lambda.Body.NodeType == ExpressionType.MemberAccess)
			return ((MemberExpression)lambda.Body).Member;
		else
			throw new ArgumentException("Not a member access", "lambda");
	}
}
#pragma warning restore 0436