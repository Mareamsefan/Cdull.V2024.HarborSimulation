using System;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations
{
    /// <summary>
    /// Represents an exception that is thrown when there is insufficient cargo in the cargo storage.
    /// </summary>
    internal class InsufficientContainerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientContainerException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InsufficientContainerException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientContainerException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public InsufficientContainerException(string message, Exception innerException) : base(message, innerException)
        {


        }
    }
}
