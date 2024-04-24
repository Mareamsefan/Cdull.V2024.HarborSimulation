
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when transportation fails during the simulation.
    /// </summary>
    public class TransportationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransportationFailedException"/> class.
        /// </summary>
        public TransportationFailedException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportationFailedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TransportationFailedException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportationFailedException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TransportationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }

}
