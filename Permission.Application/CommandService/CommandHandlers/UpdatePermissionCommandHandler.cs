using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Permission.Application.Abstractions;
using Permission.Application.CommandService.Commands;
using Permission.Domain.Interfaces;
using Permission.Infrastructure.Dtos;
using Permission.Infrastructure.Exceptions;
using Serilog;

namespace Permission.Application.CommandService.CommandHandlers
{
    public class UpdatePermissionCommandHandler : ICommandHandler<UpdatePermissionCommand, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IPermissionOperationService _permissionOperationService;

        private readonly IMapper _mapper;

        public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository, IMapper mapper, IElasticSearchService elasticSearchService, IPermissionOperationService permissionOperationService)
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

        public async Task<PermissionDto> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {

            _ = request ?? throw new ArgumentNullException(nameof(request));

            var permission = await _permissionRepository.Find<Domain.Models.Permission>(cancellationToken).SingleAsync(x => x.Id == request.Id);

            var permissionType = await _permissionRepository.Find<Domain.Models.PermissionType>(cancellationToken).SingleAsync(x => x.Id == request.PermissionTypeId);

            _ = permissionType ?? throw new EntityNotFoundException($"{nameof(Domain.Models.PermissionType)} is not found for {request.PermissionTypeId}");

            try {
                permission.Update(request.EmployeeForename, request.EmployeeSurname, request.PermissionTypeId);
                await _permissionOperationService.ProducePermissionOperationMessage(new OperationMessageDto { Id = Guid.NewGuid(), NameOperation = "Modify" });
                if (await _permissionRepository.SaveChangesAsync() > 0)
                {
                    await _elasticSearchService.IndexPermissionInElasticsearch("permissions", permission);
                    return _mapper.Map<PermissionDto>(permission);
                }
                else {
                    return new PermissionDto();
                }
                
            }
            catch (Exception ex)
            {
                Log.Error("Error Calling the Modify Operation", nameof(UpdatePermissionCommand));
                return new PermissionDto();
            }
          

    

        }
    }
}
