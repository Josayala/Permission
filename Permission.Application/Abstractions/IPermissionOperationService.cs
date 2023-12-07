using Permission.Infrastructure.Dtos;

namespace Permission.Application.Abstractions
{
    public interface IPermissionOperationService
    {
        /// <summary>
        /// Publish message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ProducePermissionOperationMessage(OperationMessageDto message);
    }
}
