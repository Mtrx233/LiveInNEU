using FluentValidation;
using LiveInNEU.Models;

namespace LiveInNEU.Services.Validation {
    /// <author>钱子昂</author>
    public class LessonValidator: AbstractValidator<Lesson> {
        public LessonValidator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("请输入课程编号");
            RuleFor(x => x.Name).NotEmpty().WithMessage("请输入课程名称");
            RuleFor(x => x.ContinueTime).NotEqual(0).WithMessage("课程持续不能为0");
            RuleFor(x => x.StartTime+x.ContinueTime).LessThanOrEqualTo(13).WithMessage("时间超出正常范围");
        }
    }
}