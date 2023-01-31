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

    public class LessonStorageHelper
    {
        public static void RemoveDatabaseFile()=> 
            File.Delete(LessonStorage.LessonDbPath);

        public static async Task<LessonStorage>
            GetInitializeLessonStorageAsync() {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            LessonStorage lessonStorage = new LessonStorage(mockInfoStorage, mockPreferenceStorage);
            await lessonStorage.InitializeAsync();
            return lessonStorage;
        }

    }
}
