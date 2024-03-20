using System.ComponentModel;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a crane used in harbor to move containers from/to ships/harbor.
    /// </summary>
    public class Crane
    {
        private string Name { get; set; }
        internal int Speed { get; set; }
        private bool IsCraneAvalible {  get; set; }
        private bool IsCraneOutOfFuntion { get; set; }

        /// <summary>
        /// Initializes a new instance of the Crane class with the specified name.
        /// </summary>
        /// <param name="craneName">The name of the crane.</param>
        public Crane(string craneName)
        {
            Name = craneName;
            IsCraneAvalible = true;
            IsCraneOutOfFuntion = false;
            Speed = 100; 
        }

        public void MoveContainerFromShipToAGV(Ship ship, AGV agv)
        {
            if (ship == null)
            {
                throw new ArgumentNullException(nameof(ship), "Ship cannot be null.");
            }

            if (agv == null)
            {
                throw new ArgumentNullException(nameof(agv), "AGV cannot be null.");
            }

            // Sjekk om skipet har last
            if (ship.Container.Any())
            {
                Container container = ship.Container.First(); // Få tak i den første cargoen fra skipet

                // Fjern cargo fra skipet
                ship.Container.Remove(container);

                // Legg til cargo i AGV
                agv.LoadContainerToAGV(container);

                // Oppdater historien til cargo
                container.History.Add($"{container.Name} moved from Ship {ship.Name} to AGV {agv.Id}");
            }
            else
            {
                // Hvis ingen handling kan utføres, kaster vi en unntak eller utfører annen håndtering etter behov
                throw new InvalidOperationException("Unable to move container from Ship to AGV.");
            }
        }

        public void MoveContainerFromAGVToStorageColumn(StorageColumn column, AGV agv)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column), "StorageColumn cannot be null.");
            }

            if (agv == null)
            {
                throw new ArgumentNullException(nameof(agv), "AGV cannot be null.");
            }

            // Sjekk om AGV har last
            if (agv.container != null)
            {
                Container container = agv.container; // Få tak i containeren fra AGV-en

                // Fjern containeren fra AGV-en og legg til i lagringskolonnen
                agv.container = null;
                column.AddCargo(container);

                // Oppdater historien til containeren
                container.History.Add($"{container.Name} moved from AGV {agv.Id} to StorageColumn {column.ColumnId}");
            }
            else
            {
                // Hvis AGV-en er tom, kast unntak eller utfør annen håndtering etter behov
                throw new InvalidOperationException("AGV has no container available to move to StorageColumn.");
            }
        }




    }
}
