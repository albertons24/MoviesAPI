using MoviesAPI.Application.Managers.QueryValidators;
using FluentValidation;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesAPI.Application.Managers.Queries;

namespace MoviesAPI.Tests.UnitTests.Validators
{
    public class GetIntelligentBillboardQueryValidatorTests
    {
        private readonly GetIntelligentBillboardQueryValidator _validator = new GetIntelligentBillboardQueryValidator();

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Empty()
        {
            var query = new GetIntelligentBillboardQuery
            {
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void Should_Have_Error_When_EndDate_Is_Empty()
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }

        [Fact]
        public void Should_Have_Error_When_EndDate_Is_Before_StartDate()
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = new DateTime(2023, 10, 10),
                EndDate = new DateTime(2023, 10, 5),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }

        [Fact]
        public void Should_Not_Have_Error_When_EndDate_Is_After_StartDate()
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = new DateTime(2023, 10, 10),
                EndDate = new DateTime(2023, 10, 15),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.EndDate);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void Should_Have_Error_When_NumberOfBigRoomScreens_Is_OutOfRange(int value)
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = value,
                NumberOfSmallRoomScreens = 1,
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.NumberOfBigRoomScreens);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(15)]
        [InlineData(24)]
        public void Should_Not_Have_Error_When_NumberOfBigRoomScreens_Is_WithinRange(int value)
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = value,
                NumberOfSmallRoomScreens = 1,
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.NumberOfBigRoomScreens);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void Should_Have_Error_When_NumberOfSmallRoomScreens_Is_OutOfRange(int value)
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = value,
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.NumberOfSmallRoomScreens);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(15)]
        [InlineData(24)]
        public void Should_Not_Have_Error_When_NumberOfSmallRoomScreens_Is_WithinRange(int value)
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = value,
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.NumberOfSmallRoomScreens);
        }

        [Fact]
        public void Should_Have_Error_When_CityId_Is_LessThan_Zero()
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
                CityId = -1,
            };

            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CityId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_CityId_Is_GreaterThanOrEqualTo_Zero()
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
                CityId = 0,
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.CityId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_CityId_Is_Null()
        {
            var query = new GetIntelligentBillboardQuery
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                NumberOfBigRoomScreens = 1,
                NumberOfSmallRoomScreens = 1,
                CityId = null,
            };

            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(x => x.CityId);
        }
    }
}
