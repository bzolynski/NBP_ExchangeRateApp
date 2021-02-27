using FluentValidation;
using NBP_ExchangeRateApp.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBP_ExchangeRateApp.Ui.Validators
{
    public class ApiQueryableValidator : AbstractValidator<IApiQueryableViewModel>
    {
        public ApiQueryableValidator()
        {
            RuleFor(x => x.SelectedStartDate)
                .Cascade(CascadeMode.Stop)
                .Must((vm, dt) => dt <= vm.SelectedEndDate).WithMessage("Start date cant be later than end date.").When(vm => vm.IsDatePeriod == true)
                .Must(BeEarlierThanToday).WithMessage("Start date cant be later than today.").When(vm => vm.IsDatePeriod == true);
                

            RuleFor(x => x.SelectedEndDate)
               .Cascade(CascadeMode.Stop)
               .Must((vm, dt) => dt >= vm.SelectedStartDate).WithMessage("End date cant be earlier than start date.").When(vm => vm.IsDatePeriod == true)
               .Must(BeEarlierThanToday).WithMessage("End date cant be later than today.").When(vm => vm.IsDatePeriod == true)
               .Must((vm, dt) => (vm.SelectedEndDate - vm.SelectedStartDate).Days < 93).WithMessage("Single enquiry cannot cover a period longer than 93 days.");

            RuleFor(x => x.SelectedDate)
                .Cascade(CascadeMode.Stop)
                .Must(dt => dt <= DateTime.Today).WithMessage("Selected date can't be later than today").When(vm => vm.IsDatePeriod == false && vm.IsShowLatest == false);

        }

        private bool BeEarlierThanToday(DateTime dt)
        {
            return dt <= DateTime.Today;
        }
    }
}
