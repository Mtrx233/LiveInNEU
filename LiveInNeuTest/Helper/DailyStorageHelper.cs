using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;

namespace LiveInNeuTest.Helper
{
    public class DailyStorageHelper
    {
        public static void RemoveDatabaseFile() =>
            File.Delete(DailyStorage.DailyDbPath);

        public static async Task<DailyStorage>
            GetInitializeDataStorageAsync()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorageStorage = infoStorageMock.Object;


            DailyStorage dataStorage = new DailyStorage(mockInfoStorageStorage, mockPreferenceStorage);
            await dataStorage.InitializeAsync();
            return dataStorage;
        }
    }
}
