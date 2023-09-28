using FluentValidation;
using MoviesAPI.Application.Managers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Application.Managers.QueryValidators
{
    public class GetIntelligentBillboardQueryValidator : AbstractValidator<GetIntelligentBillboardQuery>
    {
        public GetIntelligentBillboardQueryValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("The start date is required.");

            RuleFor(x => x.EndDate).NotEmpty().WithMessage("The end date is required.")
                                   .GreaterThan(x => x.StartDate).WithMessage("The end date must be after the start date.");

            RuleFor(x => x.NumberOfBigRoomScreens).GreaterThanOrEqualTo(0).
                WithMessage("The number of large screens must be greater than or equal to 0.")
                .LessThan(25).WithMessage("The number of large screens can't be greater than 25.");

            RuleFor(x => x.NumberOfSmallRoomScreens).GreaterThanOrEqualTo(0)
                .WithMessage("The number of small screens must be greater than or equal to 0.")
                .LessThan(25).WithMessage("The number of small screens can't be greater than 25."); ;

            RuleFor(x => x.CityId).GreaterThanOrEqualTo(0).When(x => x.CityId.HasValue)
                                .WithMessage("The value of CityId must be greater than or equal to 0 if provided.");

        }
    }
}
