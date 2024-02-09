using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public interface IHarborSimulation
    {

        public void Run(Harbor harbor, DateTime starttime, DateTime endTime, int numberOfShips, Size shipSize, int numberOfCargoOnShip, Model shipModel, int numberOfDocks,
            int numberOfCranes, Size dockSize, Model dockModel);

   
    }
}

