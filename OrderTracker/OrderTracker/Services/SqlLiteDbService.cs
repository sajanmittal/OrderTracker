using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        async Task InitializeAsync()
        {
            try
            {
                if (!initialized)
                {

                    if (!Db.TableMappings.Any(m => m.MappedType.Name == nameof(Setting)))
                    {
                        await Db.CreateTablesAsync(CreateFlags.None, TablesList).ConfigureAwait(true);
                        if(Db.TableMappings.Any(x => x.MappedType.Equals(typeof(Setting))) && await SettingService.GetSetting(Constants.RESTORE_SETTING_KEY) == null)
                        {
                            await InsertAsync(new Setting { SettingName= Constants.RESTORE_SETTING_KEY, SettingValue ="false" });
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

        private Type[] TablesList => new Type[] { typeof(Setting), typeof(Order) };


        public async Task<List<T>> SelectAsync<T>(Expression<Func<T, bool>> predicate = null) where T : BaseModel, new()
        {
            if (predicate == null)
                return await Db.Table<T>().ToListAsync();
            else
                return await Db.Table<T>().Where(predicate).ToListAsync();
        }

        public async Task<int> InsertAsync<T>(T model) where T : BaseModel
        {
            return await Db.InsertAsync(model, typeof(T));
        }

        public async Task<int> UpdateAsync<T>(T model) where T : BaseModel
        {
            return await Db.UpdateAsync(model, typeof(T));
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate = null) where T : BaseModel, new()
        {
            var data = await FirstOrDefault(predicate);
            return data != null;
        }

        public async Task<int> BulkInsertAsync<T>(ICollection<T> collection) where T : BaseModel
        {
            int result = 0;
            if (collection.Count > 0)
            {
                result = await Db.InsertAllAsync(collection, typeof(T));
            }

            return result;
        }

        public async Task<bool> IsTableEmptyAsync<T>() where T : BaseModel, new()
        {
            var records = await SelectAsync<T>();
            return records.Count > 0;
        }

        public async Task<T> SelectById<T>(int id) where T: BaseModelWithId, new()
        {
            return await FirstOrDefault<T>(x => x.Id == id);
        }

        public async Task<T> FirstOrDefault<T>(Expression<Func<T, bool>> predicate = null) where T : BaseModel, new()
        {
            return await Db.FindAsync(predicate);
        }
    }
}
