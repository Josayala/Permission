using Autofac.Extras.Moq;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Permission.Api.Controllers;
using Permission.Application.CommandService.Commands;
using Permission.Application.QueryService.Queries;
using Permission.Infrastructure.Dtos;
using Xunit;

namespace Permission.Api.UnitTests
{
    [Trait("Category", "Controller")]
    public class PermissionsControllerTests
    {
        [Fact]
        public async Task Get_AllPermissions_ReturnsOkResult()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                //Arrange
                var expectedResult = this.GetAllPermissionList();

                autoMock.Mock<IMediator>()
                    .Setup(x => x.Send(It.IsAny<GetPermissionsQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

                //Act
                var result = await autoMock.Create<PermissionsController>().GetPermissions();

                //Assert
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value.Should().Be(expectedResult);
                autoMock.Mock<IMediator>()
                    .Verify(x => x.Send(It.IsAny<GetPermissionsQuery>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }

        }

        [Fact]
        public async Task Post_CreatePermission_ReturnsOkResult()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                //Arrange
                var expectedResult = this.GetAllPermissionList().First();

                autoMock.Mock<IMediator>()
                    .Setup(x => x.Send(It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

                //Act
                var result = await autoMock.Create<PermissionsController>().CreatePermission(new CreatePermissionCommand() { EmployeeForename = "add", EmployeeSurname = "add", PermissionTypeId = 1 });

                //Assert
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value.Should().Be(expectedResult);
                autoMock.Mock<IMediator>()
                    .Verify(x => x.Send(It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()),
                                       Times.Once);
            }

        }

        [Fact]
        public async Task Put_UpdatePermission_ReturnsOkResult()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                //Arrange
                var idValid = 1;
                var expectedResult = this.GetAllPermissionList().First();

                autoMock.Mock<IMediator>()
                    .Setup(x => x.Send(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedResult);

                //Act
                var result = await autoMock.Create<PermissionsController>().UpdatePermission(idValid,new UpdatePermissionCommand() { Id=idValid,EmployeeForename="edit",EmployeeSurname="edit",PermissionTypeId=1});

                //Assert
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value.Should().Be(expectedResult);
                autoMock.Mock<IMediator>()
                    .Verify(x => x.Send(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()),
                                                          Times.Once);
            }

        }
        #region Private Methods
        public List<PermissionDto> GetAllPermissionList()
        {
            var dtoList = new List<PermissionDto>
            {
                new PermissionDto
                {
                    Id = 1,
                    EmployeeForename = "Juan",
                    EmployeeSurname = "Pepe",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Now,
                },
                new PermissionDto
                {
                    Id = 2,
                    EmployeeForename = "Jose",
                    EmployeeSurname = "Daniel",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Now,
                }
            };
            return dtoList;
        }
        #endregion
    }
}