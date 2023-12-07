using Permission.Application.Abstractions;
using Permission.Infrastructure.Dtos;

namespace Permission.Application.PermissionService
{
    public class PermissionOperationService : IPermissionOperationService
    {
        private readonly IKafkaProducer _kafkaProducer;

        public PermissionOperationService(IKafkaProducer kafkaProducer)
        {
            this._kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
          
        }

        /// <inheritdoc/>
        public async Task ProducePermissionOperationMessage(OperationMessageDto operationMessage)
        {
            await _kafkaProducer.ProduceAsync(operationMessage);
        }
    }
}
