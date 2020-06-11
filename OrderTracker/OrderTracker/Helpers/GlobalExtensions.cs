using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace OrderTracker
{
	public static class GlobalExtensions
	{
		#region Expression Extensions

		public static MemberInfo GetMemberInfo(this Expression expression)
		{
			MemberExpression operand;
			LambdaExpression lambdaExpression = (LambdaExpression)expression;
			if (lambdaExpression.Body as UnaryExpression != null)
			{
				UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
				operand = (MemberExpression)body.Operand;
			}
			else
			{
				operand = (MemberExpression)lambdaExpression.Body;
			}
			return operand.Member;
		}

		#endregion Expression Extensions

		#region IEnunbrable Extensions

		public async static Task<DataTable> ToDataTable<T>(this IEnumerable<T> items, string tableName = null)
		{
			return await Task.Run(() =>
			{
				DataTable dataTable = new DataTable(tableName ?? $"{typeof(T).Name} Records");
				PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<IgnoreAttribute>() == null).ToArray();
				foreach (PropertyInfo prop in Props)
				{
					dataTable.Columns.Add(new DataColumn(prop.Name, prop.PropertyType.IsEnum ? typeof(string) : prop.PropertyType.GetNullableType()));
				}
				foreach (T item in items)
				{
					DataRow row = dataTable.NewRow();
					foreach (var prop in Props)
						row[prop.Name] = IsNullableType(prop.PropertyType) ? (prop.GetValue(item) ?? DBNull.Value) : prop.GetValue(item);

					dataTable.Rows.Add(row);
				}
				return dataTable;
			}
			);
		}


		public async static Task ForEachAsync<T>(this IEnumerable<T> collection, Func<T, Task> action)
		{
			if (collection.Any())
			{
				foreach (T data in collection)
				{
					await action(data);
				}
			}
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			if (collection.Any())
			{
				foreach (T data in collection)
				{
					action(data);
				}
			}
		}


		#endregion IEnunbrable Extensions

		#region System.Type Extensions

		public static Type GetNullableType(this Type t)
		{
			Type returnType = t;
			if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				returnType = Nullable.GetUnderlyingType(t);
			}
			return returnType;
		}

		public static bool IsNullableType(this Type type) => type == typeof(string) || type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));

		#endregion System.Type Extensions

		#region Collection Extensions

		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> data)
		{
			foreach (T item in data)
			{
				collection.Add(item);
			}
		}


		#endregion Collection Extensions

		#region SQLiteConnection Extensions

		public static List<T> QueryTable<T, U>(this SQLiteConnection conn, Expression<Func<T, bool>> predExpr = null, Expression<Func<T, U>> orderExpr = null, bool isDesc = false) where T : IBaseModel, new()
		{
			TableQuery<T> table = conn.Table<T>();
			if (predExpr != null && orderExpr != null)
			{
				table = isDesc ? table.Where(predExpr).OrderByDescending(orderExpr) : table.Where(predExpr).OrderBy(orderExpr);
			}
			else if (predExpr != null)
			{
				table = table.Where(predExpr);
			}
			else if (orderExpr != null)
			{
				table = isDesc ? table.OrderByDescending(orderExpr) : table.OrderBy(orderExpr);
			}

			return table.ToList();
		}

		#endregion SQLiteConnection Extensions
	}
}