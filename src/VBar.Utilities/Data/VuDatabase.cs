namespace VBarUtilities.Data
{
    using SQLite;
    using System;
    using System.Threading.Tasks;

    public class VuDatabase
    {
        readonly SQLiteAsyncConnection database;

        public VuDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            CreateTables();
        }

        public AsyncTableQuery<VuFlight> Flights()
        {
            return database.Table<VuFlight>();
        }

        public async Task<VuFlight> GetFlight(DateTime dateAndTime)
        {
            return await database.GetAsync<VuFlight>(dateAndTime);
        }

        internal async Task ResetDatabase()
        {
            await database.DropTableAsync<VuSetup>();
            await database.DropTableAsync<VuController>();
            await database.DropTableAsync<VuDevice>();
            await database.DropTableAsync<VuBattery>();
            await database.DropTableAsync<VuModel>();
            await database.DropTableAsync<VuFlight>();
            await database.DropTableAsync<VuFlightDetail>();

            await CreateTables();
        }

        private async Task CreateTables()
        {
            database.CreateTableAsync<VuSetup>().Wait();
            database.CreateTableAsync<VuController>().Wait();
            database.CreateTableAsync<VuDevice>().Wait();
            database.CreateTableAsync<VuBattery>().Wait();
            database.CreateTableAsync<VuModel>().Wait();
            database.CreateTableAsync<VuFlight>().Wait();
            database.CreateTableAsync<VuPart>().Wait();
            database.CreateTableAsync<VuFlightDetail>().Wait();
        }

        public async Task<int> InsertFlightAsync(VuFlight flight)
        {
            return await database.InsertOrReplaceAsync(flight);
        }

        public async Task<int> InsertPartAsync(VuPart part)
        {
            return await database.InsertAsync(part);
        }

        public async Task<int> UpdatePartAsync(VuPart part)
        {
            return await database.UpdateAsync(part);
        }

        public AsyncTableQuery<VuPart> Parts()
        {
            return database.Table<VuPart>();
        }

        public AsyncTableQuery<VuController> Controllers()
        {
            return database.Table<VuController>();
        }

        public async Task ClearControllers()
        {
            await database.DropTableAsync<VuController>();
            await database.CreateTableAsync<VuController>();
        }

        public async Task<int> InsertControllerAsync(VuController controller)
        {
            return await database.InsertAsync(controller);
        }

        internal async Task<int> InsertDeviceAsync(VuDevice device)
        {
            return await database.InsertAsync(device);
        }
        public async Task ClearDevices()
        {
            await database.DropTableAsync<VuDevice>();
            await database.CreateTableAsync<VuDevice>();
        }

        public AsyncTableQuery<VuDevice> Devices()
        {
            return database.Table<VuDevice>();
        }

        internal async Task<int> InsertSetupAsync(VuSetup setup)
        {
            return await database.InsertAsync(setup);
        }

        public async Task ClearSetups()
        {
            await database.DropTableAsync<VuSetup>();
            await database.CreateTableAsync<VuSetup>();
        }

        public AsyncTableQuery<VuSetup> Setups()
        {
            return database.Table<VuSetup>();
        }

        public async Task<VuSetup> GetSetupById(int id)
        {
            return await database.GetAsync<VuSetup>(id);
        }

        internal async Task ClearBatteries()
        {
            await database.DropTableAsync<VuBattery>();
            await database.CreateTableAsync<VuBattery>();
        }

        internal async Task<int> InsertBatteryAsync(VuBattery battery)
        {
            return await database.InsertAsync(battery);
        }

        public AsyncTableQuery<VuBattery> Batteries()
        {
            return database.Table<VuBattery>();
        }

        internal async Task ClearModels()
        {
            await database.DropTableAsync<VuModel>();
            await database.CreateTableAsync<VuModel>();
        }

        internal async Task<int> InsertModelAsync(VuModel model)
        {
            return await database.InsertAsync(model);
        }

        public AsyncTableQuery<VuModel> Models()
        {
            return database.Table<VuModel>();
        }

        public async Task<VuPart> GetPart(Guid id)
        {
            return await database.GetAsync<VuPart>(id);
        }

        public async Task DeletePart(Guid id)
        {
            await database.DeleteAsync<VuPart>(id);
        }

        public async Task<VuFlightDetail> GetFlightDetail(int flightNo)
        {
            return await database.FindAsync<VuFlightDetail>(flightNo);
        }

        public async Task<int> AddFlightDetail(VuFlightDetail flightDetail)
        {
            return await database.InsertAsync(flightDetail);
        }

        public async Task<int> UpdateFlightDetail(VuFlightDetail flightDetail)
        {
            return await database.UpdateAsync(flightDetail);
        }

        public async Task<VuController> GetController(string controllerId)
        {
            return await database.GetAsync<VuController>(controllerId);
        }

        public async Task<VuDevice> GetDevice(string deviceId)
        {
            return await database.FindAsync<VuDevice>(deviceId);
        }

        public async Task<VuModel> GetModel(string deviceId)
        {
            return await database.GetAsync<VuModel>(deviceId);
        }

        public async Task<VuBattery> GetBattery(string name)
        {
            return await database.FindAsync<VuBattery>(name);
        }

        public AsyncTableQuery<VuFlightDetail> FlightDetails()
        {
            return database.Table<VuFlightDetail>();
        }
    }
}
