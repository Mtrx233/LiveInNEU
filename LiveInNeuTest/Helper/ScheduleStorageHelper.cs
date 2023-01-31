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
    public class ScheduleStorageHelper
    {
        public static void RemoveDatabaseFile() =>
            File.Delete(ScheduleStorage.ScheduleDbPath);

        public static async Task<ScheduleStorage>
            GetInitializeDataStorageAsync()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorageStorage = infoStorageMock.Object;


            ScheduleStorage dataStorage = new ScheduleStorage(mockInfoStorageStorage, mockPreferenceStorage);
            await dataStorage.InitializeAsync();
            return dataStorage;
        }
    }
}
