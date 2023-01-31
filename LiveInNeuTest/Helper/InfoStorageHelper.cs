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
    public class InfoStorageHelper
    {
        public static void RemoveDatabaseFile() =>
            File.Delete(InfoStorage.InfoDbPath);

        public static async Task<InfoStorage>
            GetInitializeInfoStorageAsync()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;


            InfoStorage infoStorage = new InfoStorage(mockPreferenceStorage);
            await infoStorage.InitializeAsync();
            return infoStorage;
        }
    }
}
