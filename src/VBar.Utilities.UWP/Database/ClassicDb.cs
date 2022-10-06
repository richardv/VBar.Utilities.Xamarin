namespace VBarUtilities.UWP.Database
{
    using System;
    using SQLite;

    public class ClassicDb : IDisposable
    {
        readonly SQLiteAsyncConnection database;

        public ClassicDb(string path)
        {
            database = new SQLiteAsyncConnection(path);
        }

        public AsyncTableQuery<batterylog> BatteryLogs()
        {
            return database.Table<batterylog>();
        }

        public async void Dispose()
        {
            await database.CloseAsync();
        }

        public AsyncTableQuery<model> Models()
        {
            return database.Table<model>();
        }
    }
}
