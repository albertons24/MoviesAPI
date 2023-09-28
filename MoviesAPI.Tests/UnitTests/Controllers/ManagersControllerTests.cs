using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MoviesAPI.Application.Common.DTOs;
using MoviesAPI.Application.Managers.Queries;
using MoviesAPI.Host.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests.Controllers
{
    public class ManagersControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly ManagersController _controller;

        public ManagersControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new ManagersController(_mockMediator.Object);
        }

        [Fact]
        public async Task GetIntelligentBillboard_ReturnsOk_WithValidParams()
        {
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(7);
            var billboardDto = new IntelligentBillboardDto();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetIntelligentBillboardQuery>(), default(CancellationToken))).ReturnsAsync(billboardDto);

            var result = await _controller.GetIntelligentBillboard(startDate, endDate, 2, 2);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<IntelligentBillboardDto>(okResult.Value);
            Assert.Equal(billboardDto, returnValue);
        }
    }

}
