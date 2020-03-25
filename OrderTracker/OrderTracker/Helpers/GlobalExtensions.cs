using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace OrderTracker
{
	public static class GlobalExtensions
	{
		#region Expression Entensions

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

		#endregion

		#region IEnunbrable Extensions

		public async static Task<DataTable> ToDataTable<T>(this IEnumerable<T> items, string tableName = null)
		{
			return await Task.Run(() =>
			{
				DataTable dataTable = new DataTable(tableName ?? $"{typeof(T).Name} Records");
				PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<IgnoreAttribute>() == null).ToArray();
				foreach (PropertyInfo prop in Props)
				{
					dataTable.Columns.Add(new DataColumn(prop.Name, prop.PropertyType.GetNullableType()));
				}
				foreach (T item in items)
				{
					DataRow row = dataTable.NewRow();
					foreach (var prop in Props)
					{
						if (!IsNullableType(prop.PropertyType))
							row[prop.Name] = prop.GetValue(item, null);
						else
							row[prop.Name] = (prop.GetValue(item, null) ?? DBNull.Value);
					}

					dataTable.Rows.Add(row);
				}
				return dataTable;
			}
			);
		}

		#endregion

		#region System.Type Extenstions

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


		#endregion

		#region ObservableCollection Extensions

		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> data)
		{
			foreach (T item in data)
			{
				collection.Add(item);
			}
		}

		#endregion
	}
}
