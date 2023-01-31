using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;

namespace TestProject1.Helper
{
    public class DataStorageHelper
    {
        public static void RemoveDatabaseFile() =>
            File.Delete(DataStorage.DataDbPath);

        public static async Task<DataStorage>
            GetInitializeDataStorageAsync()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorageStorage = infoStorageMock.Object;


            DataStorage dataStorage = new DataStorage(mockInfoStorageStorage,mockPreferenceStorage);
            await dataStorage.InitializeAsync();
            return dataStorage;
        }
    }
}
