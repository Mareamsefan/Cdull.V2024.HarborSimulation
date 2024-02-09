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
            CargoStorage cargoStorage = new CargoStorage("CargoStorage", 100);
            Harbor harbor_2 = new("MARI HARBOR", cargoStorage);
            driver.Run(harbor_2, startTime, endTime, 10, Enums.Size.Large, 100, Enums.Model.ContainerShip, 
                5, 10, Enums.Size.Large, Enums.Model.ContainerShip);


            //Console.WriteLine(harbor_2.HarborHistory);

            Console.WriteLine(harbor_2.GetHarborHistory(new DateTime(2024, 1, 2)));

            Console.WriteLine(harbor_2.GetHarborHistory(new DateTime(2024, 1, 3)));



        }




    }
}