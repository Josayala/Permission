using AutoMapper;
using Moq;
using Permission.Application.Abstractions;
using Permission.Application.QueryService.Queries;
using Permission.Application.QueryService.QueryHandlers;
using Permission.Infrastructure.Dtos;
using Permission.Infrastructure.EF.Repositories;
using Xunit;

namespace Permission.Application.UnitTests
{
    [Trait("Category", "Query Handler")]
    public class GetPermissionsQueryHandlerTests : IClassFixture<InMemoryPermissionDataBaseSeedData>
    {
        private readonly Mock<IElasticSearchService> _mockElasticSearchService;
        private readonly Mock<IPermissionOperationService> _mockPermissionOperationService;
        private readonly GetPermissionsQueryHandler _handler;
        InMemoryPermissionDataBaseSeedData _fixture;
        public GetPermissionsQueryHandlerTests(InMemoryPermissionDataBaseSeedData fixture)
        {            
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Models.Permission, PermissionDto>();
            });
            var mapper = mockMapper.CreateMapper();
            _fixture = fixture;
            _mockElasticSearchService = new Mock<IElasticSearchService>();
            _mockPermissionOperationService = new Mock<IPermissionOperationService>();
            _handler = new GetPermissionsQueryHandler(new PermissionRepository(_fixture.DbContext), mapper, _mockElasticSearchService.Object, _mockPermissionOperationService.Object);         
        }

        [Fact]
        public async Task Handle_ShouldReturnGetPermissions_Successfully()
        {
            // Arrange    
            var permissions = GetAllPermissionList();
            var permissionDtos = GetAllPermissionDtoList();
            _mockElasticSearchService.Setup(x => x.GetDocuments("permissions")).ReturnsAsync(permissions);
            _mockPermissionOperationService.Setup(x => x.ProducePermissionOperationMessage(new OperationMessageDto() { Id = Guid.NewGuid(),NameOperation="Get" })).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(new GetPermissionsQuery(), new CancellationToken());

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(permissionDtos.First().EmployeeSurname, result.First().EmployeeSurname);
            Assert.Equal(permissionDtos.First().EmployeeForename, result.First().EmployeeForename);
            Assert.Equal(permissionDtos.First().Id, result.First().Id);
            Assert.Equal(permissionDtos.First().PermissionTypeId, result.First().PermissionTypeId);

            _mockElasticSearchService.Verify(x => x.GetDocuments("permissions"), Times.Once);
            _mockPermissionOperationService.Verify(x => x.ProducePermissionOperationMessage(It.IsAny<OperationMessageDto>()), Times.Once);

        }

        #region Private Methods       
        public List<Domain.Models.Permission> GetAllPermissionList()
        {
            var dtoList = new List<Domain.Models.Permission>
            {
                new Domain.Models.Permission(1,"Juan","Daniel",1,DateTime.Now),
                new Domain.Models.Permission(2,"Esteban","Ramon",1,DateTime.Now),
            };
            return dtoList;
        }

        public List<PermissionDto> GetAllPermissionDtoList()
        {
            var dtoList = new List<PermissionDto>
            {
                new PermissionDto(){Id= 1,EmployeeForename="Juan",EmployeeSurname="Daniel",PermissionTypeId= 1,PermissionDate= DateTime.Now },
            };
            return dtoList;
        }



        #endregion
    }
}
