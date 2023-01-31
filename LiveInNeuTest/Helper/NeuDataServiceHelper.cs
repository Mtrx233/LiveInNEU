using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;

namespace TestProject1 {
    /// <summary>
    /// 课表数据获取帮助测试类
    /// </summary>
    public class NeuDataServiceHelper {
        /// <summary>
        /// 公用初始化参数
        /// </summary>
        /// <returns></returns>
        
        public static async Task<NeuDataService> GetLessonPageViewModelAsync() {
            var alertService = new Mock<IAlertService>();
            var lessonStorage = new Mock<ILessonStorage>();
            var loginStorage = new Mock<ILoginStorage>();
            var semesterStorage = new Mock<ISemesterStorage>();
            var dataStorage = new Mock<IDataStorage>();
            var neuDataService =
                new NeuDataService(lessonStorage.Object, loginStorage.Object,
                    semesterStorage.Object, alertService.Object,dataStorage.Object);
            return neuDataService;
        }
        
        
        public static async Task<NeuDataService> GetLessonPageViewModelAsync(Mock<ILoginStorage> LoginStorage) {
            var alertService = new Mock<IAlertService>();
            var lessonStorage = new Mock<ILessonStorage>();
            var loginStorage = LoginStorage;
            var semesterStorage = new Mock<ISemesterStorage>();
            var dataStorage = new Mock<IDataStorage>();
            var neuDataService =
                new NeuDataService(lessonStorage.Object, loginStorage.Object,
                    semesterStorage.Object, alertService.Object, dataStorage.Object);
            return neuDataService;
        }
        
        
    }
}