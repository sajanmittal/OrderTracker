using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace OrderTracker
{
	public class SqlLiteDbService
	{
		private static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
		{
			return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
		});

		private static SQLiteAsyncConnection Db => lazyInitializer.Value;
		private static bool initialized = false;

		public SqlLiteDbService()
		{
			InitializeAsync().ConfigureAwait(false);
		}

		private async Task InitializeAsync()
		{
			try
			{
				if (!initialized)
				{
					if (!Db.TableMappings.Any(m => m.MappedType.Name == nameof(Setting)))
					{
						await Db.CreateTablesAsync(CreateFlags.None, TablesList).ConfigureAwait(true);
						if (Db.TableMappings.Any(x => x.MappedType.Equals(typeof(Setting))) && await SettingService.GetSetting(Constants.RESTORE_SETTING_KEY) == null)
						{
							await InsertAsync(new Setting { SettingName = Constants.RESTORE_SETTING_KEY, SettingValue = "false" });
						}
					}
					initialized = true;
				}
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
			}
		}

		private Type[] TablesList => new Type[] { typeof(Setting), typeof(Order), typeof(AppName), typeof(PhoneInformation), typeof(PhoneAppLink) };

		public async Task<List<T>> SelectAsync<T>(Expression<Func<T, bool>> predicate = null) where T : IBaseModel, new()
		{
			if (predicate == null)
				return await Db.Table<T>().ToListAsync();
			else
				return await Db.Table<T>().Where(predicate).ToListAsync();
		}

		public async Task<int> InsertAsync<T>(T model) where T : IBaseModel
		{
			return await Db.InsertAsync(model, typeof(T));
		}

		public async Task<int> UpdateAsync<T>(T model) where T : IBaseModel
		{
			return await Db.UpdateAsync(model, typeof(T));
		}

		public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate = null) where T : IBaseModel, new()
		{
			var data = await FirstOrDefaultAsync(predicate);
			return data != null;
		}

		public async Task<int> BulkInsertAsync<T>(ICollection<T> collection) where T : IBaseModel
		{
			int result = 0;
			if (collection.Count > 0)
			{
				result = await Db.InsertAllAsync(collection, typeof(T), true);
			}

			return result;
		}

		public async Task<int> BulkInsertAsync(Type collectionType, ICollection collection)
		{
			int result = 0;
			if (collection.Count > 0)
			{
				result = await Db.InsertAllAsync(collection, collectionType, true);
			}

			return result;
		}

		public async Task<bool> IsTableEmptyAsync<T>() where T : IBaseModel, new()
		{
			var records = await SelectAsync<T>();
			return records.Count > 0;
		}

		public async Task<T> SelectById<T>(int id) where T : BaseModelWithId, new()
		{
			return await FirstOrDefaultAsync<T>(x => x.Id == id);
		}

		public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate = null) where T : IBaseModel, new()
		{
			return await Db.FindAsync(predicate);
		}

		public async Task Transaction(Func<SQLiteConnection, bool> action)
		{
			await Db.RunInTransactionAsync((conn) =>
			{
				try
				{
					bool isCompleted = action(conn);
					if (!isCompleted)
						throw new Exception("Cound not save data. Please contact support!!");
				}
				catch (Exception ex)
				{
					throw ex;
				}
			});
		}
	}
}