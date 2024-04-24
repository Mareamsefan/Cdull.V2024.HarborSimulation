
namespace Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations
{
    /// <summary>
    /// Represents an exception that is thrown when there is not enough space in the cargo storage.
    /// </summary>
    public class StorageSpaceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSpaceException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public StorageSpaceException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSpaceException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public StorageSpaceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
