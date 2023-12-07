using System.Runtime.Serialization;

namespace Permission.Infrastructure.Exceptions
{
    /// <summary>
    /// Use when entity is not found
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        protected EntityNotFoundException(SerializationInfo oInfo, StreamingContext oContext) : base(oInfo, oContext)
        {
        }
        public EntityNotFoundException(string message) : base(message) { }
    }
}
