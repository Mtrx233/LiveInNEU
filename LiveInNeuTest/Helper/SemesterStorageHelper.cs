using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;

namespace TestProject1
{
    public class SemesterStorageHelper
    {
        public static void RemoveDatabaseFile() =>
            File.Delete(SemesterStorage.SemesterDbPath);

        public static async Task<SemesterStorage>
            GetInitializeLessonStorageAsync()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            SemesterStorage semesterStorage = new SemesterStorage(mockInfoStorage, mockPreferenceStorage);
            await semesterStorage.InitializeAsync();
            return semesterStorage;
        }
    }
}
