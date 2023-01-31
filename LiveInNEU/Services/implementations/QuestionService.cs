using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using LiveInNEU.Models;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 刷题服务实现
    /// </summary>
    /// <author></author>
    public class QuestionService : IQuestionService
    {
        /******** 构造函数 ********/
        public QuestionService(IScheduleStorage scheduleStorage,
            IQuestionStorage questionService, IStoreStorage storeStorage, IRemoteFavoriteStorage remoteFavoriteStorage)
        {
            _scheduleStorage = scheduleStorage;
            _questionStorage = questionService;
            _storeStorage = storeStorage;
            _remoteFavoriteStorage = remoteFavoriteStorage;
        }

        /******** 共有变量 ********/

        /******** 私有变量 ********/
        /// <summary>
        /// 导航信息存储
        /// </summary>
        private IScheduleStorage _scheduleStorage;

        /// <summary>
        /// 题目信息存储
        /// </summary>
        private IQuestionStorage _questionStorage;

        private readonly IStoreStorage _storeStorage;

        private IRemoteFavoriteStorage _remoteFavoriteStorage;


        /******** 继承方法 ********/


        /******** 扩展方法 ********/

        public Store IsExist(int questionId, IList<Store> list)
        {
            foreach (var i in list)
            {
                if (i.QuestionId == questionId)
                {
                    return i;
                }
            }

            return null;
        }

        public async Task<IList<Question>> GetQuestinsAsync(string lessonName, string character, int num)
        {
            IList<Question> list = new List<Question>();
            Question question = new Question()
            {
                Subject = "null"
            };
            int pNum = num - 1;
            if (num == 1)
            {
                list.Add(question);
            }
            else
            {
                list.Add(await _questionStorage.GetQuestionAsync(p =>
                    p.LessonName == lessonName && p.Character == character &&
                    p.Number == pNum));
            }

            list.Add(await _questionStorage.GetQuestionAsync(p =>
                p.LessonName == lessonName && p.Character == character &&
                p.Number == num));

            pNum = num + 1;
            Question next = await _questionStorage.GetQuestionAsync(p =>
                p.LessonName == lessonName && p.Character == character &&
                p.Number == pNum) ?? question;
            list.Add(next);
            return list;
        }

        public async Task FinishQuestionAsync(string lessonName, string character, int num)
        {
            Schedule schedule = await _scheduleStorage.GetScheduleAsync(p =>
                p.LessonName == lessonName && p.Character == character);
            Question question = await _questionStorage.GetQuestionAsync(p =>
                p.LessonName == lessonName && p.Character == character &&
                p.Number == num);
            schedule.Finished = question.IsTested == 1
                ? schedule.Finished
                : schedule.Finished + 1;
            schedule.Now = schedule.Now > num ? schedule.Now : num;
            await _scheduleStorage.UpdateScheduleAsync(schedule);

            question.IsTested = 1;
            await _questionStorage.UpdateQuestionAsync(question);
        }

        public async Task<IList<Question>> GetStoreUpsAsync(string lessonName, string character, int num)
        {
            IList<Question> list = await _questionStorage.GetQuestionsAsync(p =>
                p.StoreUp == 1 && p.LessonName == lessonName && p.Character == character);
            return list;
        }

        public async Task SetStoreUpAsync(string lessonName, string character,
            int num)
        {
            Question question = await _questionStorage.GetQuestionAsync(p =>
                p.LessonName == lessonName && p.Character == character &&
                p.Number == num);
            question.StoreUp = question.StoreUp == 1 ? 0 : 1;
            var schedule = await _scheduleStorage.GetScheduleAsync(p =>
                p.LessonName == lessonName && p.Character == character);

            var scheduleFirst = await _scheduleStorage.GetScheduleAsync(p =>
                p.LessonName == lessonName && p.Character == "First");

            if (question.StoreUp == 1)
            {
                schedule.StoreNum++;
                scheduleFirst.StoreNum++;
            }
            else
            {
                schedule.StoreNum--;
                scheduleFirst.StoreNum--;
            }
            await _scheduleStorage.UpdateScheduleAsync(schedule);
            await _scheduleStorage.UpdateScheduleAsync(scheduleFirst);
            
            var now = DateTime.Now;
            question.StoreUpTime = now.ToString();
            await _questionStorage.UpdateQuestionAsync(question);

            //TODO 同步信息修改
            var local =
                await _storeStorage.GetStoresAsync(p => p.QuestionId != -1);
            var temp = IsExist(question.Id, local);
            if (temp != null)
            {
                var l = Convert.ToDateTime(temp.UpdateTime);
                if (l < now)
                {
                    temp.UpdateTime = question.StoreUpTime;
                    temp.IsStore = question.StoreUp;
                    await _storeStorage.UpdateStoreAsync(temp);
                }
            }
            else
            {
                await _storeStorage.AddStoreAsync(new Store
                {
                    IsStore = question.StoreUp,
                    QuestionId = question.Id,
                    UpdateTime = question.StoreUpTime
                });
            }
        }

        public async Task<IList<Question>> GetQuestinsAsync(string lessonName, string character)
        {
            return await _questionStorage.GetQuestionsAsync(p =>
                p.LessonName == lessonName && p.Character == character);
        }
        
        public async Task<bool> Synchronization()
        {
            string FirebaseClient =
                "https://inneu-eebfd-default-rtdb.asia-southeast1.firebasedatabase.app/";

            string FrebaseSecret = "RtlbP3Sh41eYQJ3kXiG8qskYv0WfkvjOHBhnytP1";

            FirebaseClient firebaseClient = new FirebaseClient(FirebaseClient,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(FrebaseSecret)
                });
            var firebase = new List<Store>();
            var local =
                await _storeStorage.GetStoresAsync(p => p.QuestionId != -1);
            try
            {
                var coll = await firebaseClient
                    .Child(await _storeStorage.GetUserName())
                    .OnceAsync<List<Store>>();
                if (coll.Count == 0)
                {
                    await firebaseClient
                        .Child(await _storeStorage.GetUserName())
                        .PostAsync(local);
                    return true;
                }

                firebase = coll.First().Object;
            }
            catch (Exception e)
            {
                return false;
            }

            var remote = new List<Store>();
            foreach (var remoteStoraFirebaseObject in firebase)
            {
                var temp = IsExist(remoteStoraFirebaseObject.QuestionId, local);
                if (temp != null)
                {
                    var l = Convert.ToDateTime(temp.UpdateTime);
                    var r = Convert.ToDateTime(remoteStoraFirebaseObject
                        .UpdateTime);
                    if (l < r)
                    {
                        temp.UpdateTime = remoteStoraFirebaseObject.UpdateTime;
                        temp.IsStore = remoteStoraFirebaseObject.IsStore;
                    }
                }
                else
                {
                    remote.Add(remoteStoraFirebaseObject);
                }
            }

            foreach (var remoteStore in remote)
            {
                local.Add(remoteStore);
            }

            try
            {
                await firebaseClient.Child(await _storeStorage.GetUserName())
                    .DeleteAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            try
            {
                await firebaseClient.Child(await _storeStorage.GetUserName())
                    .PostAsync(local);
            }
            catch (Exception e)
            {
                return false;
            }

            foreach (var VARIABLE in local) {
                Question question = await  _questionStorage.GetQuestionAsync(p =>
                    p.Id == VARIABLE.QuestionId);
                if (question.StoreUp==VARIABLE.IsStore) {
                    question.StoreUpTime = VARIABLE.UpdateTime.ToString();
                    await _questionStorage.UpdateQuestionStoreAsync(question);
                } else {
                    await SetStoreUpAsync(question.LessonName, question.Character, question.Number);
                }
            }

            await _storeStorage.UpStoresAsync(local);
            return true;
        }

        public async Task<bool> SynchronizationByOneDrive()
        {
            var isRightaAsync = await _remoteFavoriteStorage.SignInAsync();

            if (isRightaAsync)
            {
                IList<Store> firebase = new List<Store>();
                var local =
                    await _storeStorage.GetStoresAsync(p => p.QuestionId != -1);
                try
                {
                    firebase = await _remoteFavoriteStorage.GetFavoriteItemsAsync();
                }
                catch (Exception e)
                {
                    return false;
                }

                var remote = new List<Store>();
                foreach (var remoteStoraFirebaseObject in firebase)
                {
                    var temp = IsExist(remoteStoraFirebaseObject.QuestionId, local);
                    if (temp != null)
                    {
                        var l = Convert.ToDateTime(temp.UpdateTime);
                        var r = Convert.ToDateTime(remoteStoraFirebaseObject
                            .UpdateTime);
                        if (l < r)
                        {
                            temp.UpdateTime = remoteStoraFirebaseObject.UpdateTime;
                            temp.IsStore = remoteStoraFirebaseObject.IsStore;
                        }
                    }
                    else
                    {
                        remote.Add(remoteStoraFirebaseObject);
                    }
                }

                foreach (var remoteStore in remote)
                {
                    local.Add(remoteStore);
                }


                try
                {
                    await _remoteFavoriteStorage.SaveFavoriteItemsAsync(local);
                }
                catch (Exception e)
                {
                    return false;
                }

                foreach (var VARIABLE in local)
                {
                    Question question = await _questionStorage.GetQuestionAsync(p =>
                        p.Id == VARIABLE.QuestionId);
                    if (question.StoreUp == VARIABLE.IsStore)
                    {
                        question.StoreUpTime = VARIABLE.UpdateTime.ToString();
                        await _questionStorage.UpdateQuestionStoreAsync(question);
                    }
                    else
                    {
                        await SetStoreUpAsync(question.LessonName,
                            question.Character, question.Number);
                    }
                }
                await _storeStorage.UpStoresAsync(local);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}