using System;
using ESchool.Admin.ViewModels.Examinations;
using FluentValidation;

namespace ESchool.Admin.Validators.Examinations
{
    public class QuestionValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionValidator()
        {
            RuleFor(p => p.Content)
                .NotEmpty();

            RuleFor(p => p.Type)
                .InclusiveBetween(1, 2);

            //RuleFor(p => p.DifficultLevel)
            //    .InclusiveBetween(1, 5);

            var fromDate = new DateTime(2015, 1, 1);
            var toDate = fromDate.AddYears(100);
            RuleFor(p => p.Month)
                .InclusiveBetween(fromDate, toDate);

            RuleFor(p => p.Type)
                .GreaterThan(0);
        }
    }
}
