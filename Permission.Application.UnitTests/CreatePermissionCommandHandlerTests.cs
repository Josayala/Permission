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
    public class CreatePermissionCommandHandlerTests : IClassFixture<InMemoryPermissionDataBaseSeedData>
    {
        private readonly Mock<IElasticSearchService> _mockElasticSearchService;
        private readonly Mock<IPermissionOperationService> _mockPermissionOperationService;
        private readonly CreatePermissionCommandHandler _handler;
        InMemoryPermissionDataBaseSeedData _fixture;
        public CreatePermissionCommandHandlerTests(InMemoryPermissionDataBaseSeedData fixture)
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Models.Permission, PermissionDto>();
                cfg.CreateMap<CreatePermissionCommand, Domain.Models.Permission>()
                     .ForMember(d => d.PermissionDate, s => s.MapFrom(x => DateTime.Now));
            });
            var mapper = mockMapper.CreateMapper();
            _fixture = fixture;
            _mockElasticSearchService = new Mock<IElasticSearchService>();
            _mockPermissionOperationService = new Mock<IPermissionOperationService>();
            _handler = new CreatePermissionCommandHandler(new PermissionRepository(_fixture.DbContext), mapper, _mockElasticSearchService.Object, _mockPermissionOperationService.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateNewPermission_Successfully()
        {
            // Arrange    
            var newPermission = CreateNewPermission();

            _mockElasticSearchService.Setup(x => x.IndexPermissionInElasticsearch("permissions", newPermission));

            _mockPermissionOperationService.Setup(x => x.ProducePermissionOperationMessage(new OperationMessageDto() { Id = Guid.NewGuid(), NameOperation = "Request" })).Returns(Task.CompletedTask);

            var request = new CreatePermissionCommand()
            {
                EmployeeForename = newPermission.EmployeeForename,
                EmployeeSurname = newPermission.EmployeeSurname,
                PermissionTypeId = newPermission.PermissionTypeId
            };
            // Act
            var result = await _handler.Handle(request, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newPermission.EmployeeForename, result.EmployeeForename);
            Assert.Equal(newPermission.EmployeeSurname, result.EmployeeSurname);
            Assert.Equal(newPermission.PermissionTypeId, result.PermissionTypeId);            
            _mockElasticSearchService.Verify(x => x.IndexPermissionInElasticsearch("permissions", It.IsAny<Domain.Models.Permission>()), Times.Once);
            _mockPermissionOperationService.Verify(x => x.ProducePermissionOperationMessage(It.IsAny<OperationMessageDto>()), Times.Once);

        }

        #region Private Methods       
        public Domain.Models.Permission CreateNewPermission()
        {   
            return new Domain.Models.Permission(3, "Jose", "Ayala", 1, DateTime.Now);
        }
        #endregion
    }
}
