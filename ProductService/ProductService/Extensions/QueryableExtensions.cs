using System.Linq.Expressions;

public static class QueryableExtensions
{
	public static IQueryable<T> WhereIfIn<T, TValue>(
	this IQueryable<T> query,
	List<TValue>? filterValues,
	Expression<Func<T, TValue>> propertySelector)
	{
		IEnumerable<TValue> cleanedValues;

		if (filterValues != null && filterValues.Count > 0)
		{
			// If TValue is string then clean null, empty, whitespace
			if (typeof(TValue) == typeof(string))
			{
				cleanedValues = filterValues
					.Where(v => v is string str && !string.IsNullOrWhiteSpace(str))
					.Distinct()
					.ToList();
			}
			else
			{
				// For other value types (int, enum, etc.) just distinct + skip null
				cleanedValues = filterValues
					.Where(v => v != null)
					.Distinct()
					.ToList();
			}

			if (cleanedValues.Any())
			{
				var parameter = propertySelector.Parameters[0];    // 'p' from p => p.Name
				var property = propertySelector.Body;             // p.Name expression

				// Generate the 'Contains' method call to be translated into SQL
				var containsMethod = typeof(List<TValue>).GetMethod(nameof(List<TValue>.Contains), new[] { typeof(TValue) })!;
				var valuesExpr = Expression.Constant(cleanedValues);  // list of values to check against

				var containsExpr = Expression.Call(valuesExpr, containsMethod, property);  // p.Name.Contains(cleanedValues)

				// Create a lambda expression: p => p.Name.Contains(cleanedValues)
				var lambda = Expression.Lambda<Func<T, bool>>(containsExpr, parameter);

				return query.Where(lambda);  // Applies the dynamic 'Where' condition
			}
		}
		return query;
	}
}
