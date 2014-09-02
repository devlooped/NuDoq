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

using System;
using System.Diagnostics;
using System.Linq.Expressions;

/// <summary>
/// Common guard class for argument validation.
/// </summary>
[DebuggerStepThrough]
internal static class Guard
{
	/// <summary>
	/// Ensures the given <paramref name="value"/> is not null.
	/// Throws <see cref="ArgumentNullException"/> otherwise.
	/// </summary>
	public static void NotNull<T>(Expression<Func<T>> reference, T value)
	{
		if (value == null)
			throw new ArgumentNullException(GetParameterName(reference), "Parameter cannot be null.");
	}

	/// <summary>
	/// Ensures the given string <paramref name="value"/> is not null or empty.
	/// Throws <see cref="ArgumentNullException"/> in the first case, or 
	/// <see cref="ArgumentException"/> in the latter.
	/// </summary>
	public static void NotNullOrEmpty(Expression<Func<string>> reference, string value)
	{
		NotNull<string>(reference, value);
		if (value.Length == 0)
			throw new ArgumentException(GetParameterName(reference), "Parameter cannot be empty.");
	}

	private static string GetParameterName(Expression reference)
	{
		var lambda = reference as LambdaExpression;
		var member = lambda.Body as MemberExpression;

		return member.Member.Name;
	}
}