using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    /// <summary>
    /// Represents a sailing event of a ship.
    /// </summary>
    public class Sailing
    {
        /// <summary>
        /// Gets or sets the date and time of the sailing.
        /// </summary>
        internal DateTime DateTime {  get; set; }

        /// <summary>
        /// Gets or sets the destination location of the sailing.
        /// </summary>
        internal int DestinationLocation { get; set; }

        /// <summary>
        /// Gets or sets the recurring type of the sailing.
        /// </summary>
        internal RecurringType RecurringType { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Sailing"/> class.
        /// </summary>
        /// <param name="sailingDateTime">The date and time of the sailing.</param>
        /// <param name="sailingDestinationLocation">The destination location of the sailing.</param>
        /// <param name="sailingRecurringType">The recurring type of the sailing.</param>
        internal Sailing(DateTime sailingDateTime, int sailingDestinationLocation, RecurringType sailingRecurringType) { 
        
            DateTime = sailingDateTime;
            DestinationLocation = sailingDestinationLocation;
            RecurringType = sailingRecurringType;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Sailing DateTime: {DateTime}, Destination Location: {DestinationLocation}, Recurring Type: {RecurringType}";
        }
    }
}