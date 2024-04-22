using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

/// <summary>
/// Represents the scheduling and execution of ship sailings.
/// </summary>
namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    public class Sailing
    {
      internal DateTime DateTime {  get; set; }
      internal int DestinationLocation { get; set; }    
      internal RecurringType RecurringType { get; set; }    

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