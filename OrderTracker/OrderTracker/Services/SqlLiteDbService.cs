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
            if (!initialized && !Db.TableMappings.Any(m => m.MappedType.Name == typeof(Order).Name))
            {
                await Db.CreateTablesAsync(CreateFlags.AutoIncPK, typeof(Order)).ConfigureAwait(false);
                initialized = true;
            }
        }

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

        public async Task BACKUP_DB(string backupDestinationPath, string backupDbName)
        {
            await Db.BackupAsync(backupDestinationPath, backupDbName);
        }
    }
}
