using Cdull.V2024.HarborSimulation.SimulationFramework;
using System;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Program
    {

        static void Main(string[] args)
        {


            IHarborSimulation driver = new Simulation();
            DateTime startTime = new DateTime(2024, 1, 1, 13, 09, 21);
            DateTime endTime = new DateTime(2024, 1, 5);
            CargoStorage cargoStorage = new CargoStorage("CargoStorage", 100);
            Harbor harbor_2 = new("MARI HARBOR", cargoStorage);
            driver.Run(harbor_2, startTime, endTime, 10, Enums.Size.Large, 100, Enums.Model.ContainerShip, 
                5, 10, Enums.Size.Large, Enums.Model.ContainerShip);


            Console.WriteLine(harbor_2.name);

            
            DateTime date1 = new DateTime(2024, 1, 2);
            Console.WriteLine($"Harbor history for date: {date1}" + harbor_2.GetHarborHistory(date1));
            
            DateTime date2 = new DateTime(2024, 1, 3);
            Console.WriteLine($"Harbor history for date:{date2}"+harbor_2.GetHarborHistory(date2));
            DateTime date3 = new DateTime(2024, 1, 4);
            Console.WriteLine($"Harbor history for date:{date3}"+harbor_2.GetHarborHistory(date3));
            
         
           

        }

        static void TestingShipHandler() {
           

        }



    }
}