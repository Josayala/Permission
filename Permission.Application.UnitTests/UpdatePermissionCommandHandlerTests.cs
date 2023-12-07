using AutoMapper;
using Moq;
using Permission.Application.Abstractions;
using Permission.Application.CommandService.CommandHandlers;
using Permission.Application.CommandService.Commands;
using Permission.Infrastructure.Dtos;
using Permission.Infrastructure.EF.Repositories;
using Xunit;

namespace Permission.Application.UnitTests
{
    [Trait("Category", "Command Handler")]
    public class UpdatePermissionCommandHandlerTests : IClassFixture<InMemoryPermissionDataBaseSeedData>
    {
        private readonly Mock<IElasticSearchService> _mockElasticSearchService;
        private readonly Mock<IPermissionOperationService> _mockPermissionOperationService;
        private readonly UpdatePermissionCommandHandler _handler;
        InMemoryPermissionDataBaseSeedData _fixture;
        public UpdatePermissionCommandHandlerTests(InMemoryPermissionDataBaseSeedData fixture)
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Models.Permission, PermissionDto>();
            });
            var mapper = mockMapper.CreateMapper();
            _fixture = fixture;
            _mockElasticSearchService = new Mock<IElasticSearchService>();
            _mockPermissionOperationService = new Mock<IPermissionOperationService>();
            _handler = new UpdatePermissionCommandHandler(new PermissionRepository(_fixture.DbContext), mapper, _mockElasticSearchService.Object, _mockPermissionOperationService.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdatePermission_Successfully()
        {
            // Arrange    
            var permissionUpdated = UpdatePermission();

            _mockElasticSearchService.Setup(x => x.IndexPermissionInElasticsearch("permissions", permissionUpdated));

            _mockPermissionOperationService.Setup(x => x.ProducePermissionOperationMessage(new OperationMessageDto() { Id = Guid.NewGuid(), NameOperation = "Modify" })).Returns(Task.CompletedTask);

            var request = new UpdatePermissionCommand()
            {
                Id= permissionUpdated.Id,
                EmployeeForename = permissionUpdated.EmployeeForename,
                EmployeeSurname = permissionUpdated.EmployeeSurname,
                PermissionTypeId = permissionUpdated.PermissionTypeId
            };
            // Act
            var result = await _handler.Handle(request, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(permissionUpdated.EmployeeForename, result.EmployeeForename);
            Assert.Equal(permissionUpdated.EmployeeSurname, result.EmployeeSurname);
            Assert.Equal(permissionUpdated.PermissionTypeId, result.PermissionTypeId);
            _mockElasticSearchService.Verify(x => x.IndexPermissionInElasticsearch("permissions", It.IsAny<Domain.Models.Permission>()), Times.Once);
            _mockPermissionOperationService.Verify(x => x.ProducePermissionOperationMessage(It.IsAny<OperationMessageDto>()), Times.Once);

        }

        #region Private Methods       
        public Domain.Models.Permission UpdatePermission()
        {
            return new Domain.Models.Permission(1, "Felix", "Ayala", 1, DateTime.Now);
        }
        #endregion
    }
}
