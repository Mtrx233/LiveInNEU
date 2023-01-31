using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Views;
using Xamarin.Essentials;

namespace LiveInNEU.ViewModels
{
    public class QuestionPageViewModel : ViewModelBase, INotifyPropertyChanged, IQueryAttributable
    {
        public string Now
        {
            get { return now;}
            set
            {
                if (now != value)
                {
                    now = value;
                    OnPropertyChanged("Now");
                }
            }
        }

        private string now;
        private Question memoQuestion;

        public RelayCommand<Question> MemoCommand =>
            _memoCommand ?? (_memoCommand =
                new RelayCommand<Question>(question => memoQuestion = question));

        public string SubjectNow
        {
            get { return _subjectNow; }
            set
            {
                if (_subjectNow != value)
                {
                    _subjectNow = value;
                    OnPropertyChanged("SubjectNow");
                }
            }
        }

        private string characterNow;

        public RelayCommand PreCommand =>
            _preCommand ?? (_preCommand = new RelayCommand(() =>
                PreCommandFunction()));

        private async void PreCommandFunction()
        {
            if (questionOrderNow > 0)
            {
                _question = Questions[--questionOrderNow];
                SetPreQuestion();
            }
        }

        private void SetPreQuestion()
        {
            SetDefault();
            SetQuestionNew();
        }

        public RelayCommand NextCommand =>
            _nextCommand ?? (_nextCommand = new RelayCommand(() =>
                NextCommandFunction()));

        private async void NextCommandFunction()
        {
            if (questionOrderNow + 1 < Questions.Count)
            {
                await _questionService.FinishQuestionAsync(_question.LessonName, _question.Character, _question.Number);
                _question = Questions[++questionOrderNow];
                SetNextQuestion();
            }
        }

        private void SetNextQuestion()
        {
            SetDefault();
            SetQuestionNew();
        }

        private void SetQuestionNew()
        {
            OptionA = imageGet(_question.OptionA);
            OptionB = imageGet(_question.OptionB);
            OptionC = imageGet(_question.OptionC);
            OptionD = imageGet(_question.OptionD);
            QuestionImg = imageGet(_question.Subject);
            Analysis = imageGet(_question.Analysis);
            SubjectNow = "第" + (questionOrderNow + 1) + "题";
            IsCollected = _question.StoreUp == 1 ? Color.Gold : Color.Default;
        }

        private void SetDefault()
        {
            ColorA = ColorB = ColorC = ColorD = Color.Default;
            VisibleProper = false;
        }

        public Color IsCollected
        {
            get { return _isCollected; }
            set
            {
                if (_isCollected != value)
                {
                    _isCollected = value;
                    OnPropertyChanged("IsCollected");
                }
            }
        }

        public RelayCommand CollectCommand =>
            _collectCommand ?? (_collectCommand = new RelayCommand(() =>
                SetCollect()));

        private async void SetCollect()
        {
            IsCollected = IsCollected == Color.Default ? Color.Gold : Color.Default;
            _question.StoreUp = _question.StoreUp == 1 ? 0 : 1;
            await _questionService.SetStoreUpAsync(_question.LessonName, _question.Character, _question.Number);
        }

        public Question _question;
        private bool isStored;
        private Color _colorA;
        private Color _colorB;
        private Color _colorC;
        private Color _colorD;
        private bool _visibleProper;
        private RelayCommand _answerACommand;
        private RelayCommand _answerBCommand;
        private RelayCommand _answerCCommand;
        private RelayCommand _answerDCommand;
        private RelayCommand _collectCommand;
        private Color _isCollected;
        private RelayCommand _nextCommand;
        private byte[] _optionA;
        private byte[] _optionB;
        private byte[] _optionC;
        private byte[] _optionD;
        private byte[] _questionImg;
        private byte[] _analysis;
        private RelayCommand _preCommand;
        private IQuestionService _questionService;

        private IList<Question> Questions;
        private RelayCommand _pageAppearingCommand;

        public QuestionPageViewModel(IQuestionService questionService)
        {
            _questionService = questionService;
            _visibleProper = false;
        }

        private int questionOrderNow;
        private string _subjectNow;
        private RelayCommand<Question> _memoCommand;

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand =
                new RelayCommand(async () => await PageAppearingCommandFunction()));

        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        private async Task PageAppearingCommandFunction()
        {
            if (isStored)
            {
                Questions = await _questionService.GetStoreUpsAsync(lessonNameNow, characterNow, 0);
            }
            else
            {
                Questions = await _questionService.GetQuestinsAsync(lessonNameNow, characterNow);
            }

            _question = Questions[questionOrderNow];
            SetDefault();
            SetQuestionNew();
            Now = _question.LessonName + "-" + _question.Character + "-" +
                  _question.Number.ToString();
        }

        public string lessonNameNow { get; set; }

        public Color ColorA
        {
            get { return _colorA; }
            set
            {
                if (_colorA != value)
                {
                    _colorA = value;
                    OnPropertyChanged("ColorA");
                    SetVisibleProper();
                }
            }
        }

        public Color ColorB
        {
            get { return _colorB; }
            set
            {
                if (_colorB != value)
                {
                    _colorB = value;
                    OnPropertyChanged("ColorB");
                    SetVisibleProper();
                }
            }
        }

        public Color ColorC
        {
            get { return _colorC; }
            set
            {
                if (_colorC != value)
                {
                    _colorC = value;
                    OnPropertyChanged("ColorC");
                    SetVisibleProper();
                }
            }
        }

        public Color ColorD
        {
            get { return _colorD; }
            set
            {
                if (_colorD != value)
                {
                    _colorD = value;
                    OnPropertyChanged("ColorD");
                    SetVisibleProper();
                }
            }
        }

        private void SetVisibleProper()
        {
            VisibleProper = true;
        }

        public bool VisibleProper
        {
            get { return _visibleProper; }
            set
            {
                if (_visibleProper != value)
                {
                    _visibleProper = value;
                    OnPropertyChanged("VisibleProper");
                }
            }
        }

        public byte[] OptionA
        {
            get { return _optionA; }
            set
            {
                if (_optionA != value)
                {
                    _optionA = value;
                    OnPropertyChanged("OptionA");
                }
            }
        }

        public byte[] OptionB
        {
            get { return _optionB; }
            set
            {
                if (_optionB != value)
                {
                    _optionB = value;
                    OnPropertyChanged("OptionB");
                }
            }
        }

        public byte[] OptionC
        {
            get { return _optionC; }
            set
            {
                if (_optionC != value)
                {
                    _optionC = value;
                    OnPropertyChanged("OptionC");
                }
            }
        }

        public byte[] OptionD
        {
            get { return _optionD; }
            set
            {
                if (_optionD != value)
                {
                    _optionD = value;
                    OnPropertyChanged("OptionD");
                }
            }
        }


        public byte[] Analysis
        {
            get { return _analysis; }
            set
            {
                if (_analysis != value)
                {
                    _analysis = value;
                    OnPropertyChanged("Analysis");
                }
            }
        }

        public byte[] QuestionImg
        {
            get { return _questionImg; }
            set
            {
                if (_questionImg != value)
                {
                    _questionImg = value;
                    OnPropertyChanged("QuestionImg");
                }
            }
        }

        public RelayCommand AnswerACommand =>
            _answerACommand ?? (_answerACommand = new RelayCommand(() =>
                chooseA()));

        private void chooseA()
        {
            if (_question.Answer == 1)
            {
                ColorA = Color.Green;
            }
            else
            {
                ColorA = Color.Red;
            }
        }

        public RelayCommand AnswerBCommand =>
            _answerBCommand ?? (_answerBCommand = new RelayCommand(() =>
                chooseB()));

        private void chooseB()
        {
            if (_question.Answer == 2)
            {
                ColorB = Color.Green;
            }
            else
            {
                ColorB = Color.Red;
            }
        }

        public RelayCommand AnswerCCommand =>
            _answerCCommand ?? (_answerCCommand = new RelayCommand(() =>
                chooseC()));

        private void chooseC()
        {
            if (_question.Answer == 3)
            {
                ColorC = Color.Green;
            }
            else
            {
                ColorC = Color.Red;
            }
        }

        public RelayCommand AnswerDCommand =>
            _answerDCommand ?? (_answerDCommand = new RelayCommand(() =>
                chooseD()));

        private void chooseD()
        {
            if (_question.Answer == 4)
            {
                ColorD = Color.Green;
            }
            else
            {
                ColorD = Color.Red;
            }
        }

        public byte[] imageGet(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            return bytes;
        }

        public ObservableCollection<Question> ImagesCollection { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            lessonNameNow = HttpUtility.UrlDecode(query["LessonNameChoose"]);
            characterNow = HttpUtility.UrlDecode(query["CharacterChoose"]);
            if (query.ContainsKey("IsStored"))
            {
                isStored = true;
                questionOrderNow = 0;
            }
            else
            {
                isStored = false;
                questionOrderNow = int.Parse(HttpUtility.UrlDecode(query["SubjectNow"]));
            }
        }
    }
}