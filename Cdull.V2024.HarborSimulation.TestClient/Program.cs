using Cdull.V2024.HarborSimulation.SimulationFramework;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Program
    {

        static void Main(string[] args)
        {


            IHarborSimulation driver = new Simulation();
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 5);
            Harbor harbor_2 = new("MARI HARBOR");
            driver.Run(harbor_2, startTime, endTime, 3, Enums.Size.Large, 10, Enums.Model.ContainerShip, 3, 10, Enums.Size.Large, Enums.Model.ContainerShip);



        }




    }
}