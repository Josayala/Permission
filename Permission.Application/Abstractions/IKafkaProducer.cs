using Permission.Infrastructure.Dtos;

namespace Permission.Application.Abstractions
{
    /// <summary>
    /// Represents a Kafka producer.
    /// </summary>
    public interface IKafkaProducer
    {
        /// <summary>
        /// Asynchronously produces a message to the Kafka topic.
        /// </summary>
        /// <param name="operationMessage">The operation message to be produced.</param>
        Task ProduceAsync(OperationMessageDto operationMessage);
    }
}
