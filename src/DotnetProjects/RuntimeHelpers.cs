// ReSharper disable UseCollectionExpression
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable UnusedType.Global
#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130 // Namespace does not match folder structure
#pragma warning disable IDE0301 // Simplify collection initialization

/// <summary>
/// Copied here because GetSubArray isn't in netstandard but RuntimeHelpers is.
/// </summary>
[Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal static class RuntimeHelpers
{
	/// <summary>
	/// Slices the specified array using the specified range.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	public static T[] GetSubArray<T>(T[] array, Range range)
	{
		if (array == null)
		{
			throw new ArgumentNullException(nameof(array));
		}

		(int offset, int length) = range.GetOffsetAndLength(array.Length);

		if (default(T) != null || typeof(T[]) == array.GetType())
		{
			// We know the type of the array to be exactly T[].

			if (length == 0)
			{
				return Array.Empty<T>();
			}

			var dest = new T[length];
			Array.Copy(array, offset, dest, 0, length);
			return dest;
		}
		else
		{
			// The array is actually a U[] where U:T.
			var dest = (T[])Array.CreateInstance(array.GetType().GetElementType(), length);
			Array.Copy(array, offset, dest, 0, length);
			return dest;
		}
	}
}