using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Permission.Application.Abstractions;
using Permission.Application.QueryService.Queries;
using Permission.Domain.Interfaces;
using Permission.Infrastructure.Dtos;
using Serilog;

namespace Permission.Application.QueryService.QueryHandlers
{
    public class GetPermissionsQueryHandler : IQueryHandler<GetPermissionsQuery, List<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IPermissionOperationService _permissionOperationService;
        private readonly IMapper _mapper;

        public GetPermissionsQueryHandler(IPermissionRepository permissionRepository, IMapper mapper, IElasticSearchService elasticSearchService, IPermissionOperationService permissionOperationService)
        {
            _ = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _ = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ = elasticSearchService ?? throw new ArgumentNullException(nameof(elasticSearchService));
            _ = permissionOperationService ?? throw new ArgumentNullException(nameof(permissionOperationService));
            this._permissionRepository = permissionRepository;
            this._mapper = mapper;
            _elasticSearchService = elasticSearchService;
            _permissionOperationService = permissionOperationService;
        }

        public async Task<List<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {     
            try
            {
                var permissionList = await _permissionRepository.Query<Domain.Models.Permission>(cancellationToken).ToListAsync();
                await _permissionOperationService.ProducePermissionOperationMessage(new OperationMessageDto { Id = Guid.NewGuid(), NameOperation = "Get" });
                var permissionDtoList = _mapper.Map<List<PermissionDto>>(permissionList);
                await _elasticSearchService.GetDocuments("permissions");
                return permissionDtoList;

            }
            catch (Exception ex)
            {
                Log.Error("Error Calling the Get Operation", nameof(GetPermissionsQuery));
                return new List<PermissionDto>();                
            }         
        }
      
    }
}
