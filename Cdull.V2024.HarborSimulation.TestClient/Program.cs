using System;
using System.Collections.Generic;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.TestClient;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace HarborSimulationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Opprett en instans av Harbor-klassen
            Harbor harbor = new Harbor("Test Harbor", new CargoStorage("CargoStorage", 100));

            // Opprett noen dokker
            List<Dock> docks = harbor.InitializeDocks(5, Model.ContainerShip, Size.Large, 2);
            docks.AddRange(harbor.InitializeDocks(5, Model.RoRo, Size.Large, 10));

            // Opprett noen skip
            List<Ship> ships = harbor.InitializeShips(5, Model.ContainerShip, Size.Large, 10);
            ships.AddRange(harbor.InitializeShips(5, Model.RoRo, Size.Large, 10));
           
            IHarborSimulation driver = new Simulation();

            // Sett start- og slutttidspunkter for simuleringen
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 15);

            

            DateTime startSailingTime = new DateTime(2024, 1, 1);



            // Kjør simuleringen
            driver.Run(harbor, startTime, endTime, ships, docks, startSailingTime, 1, true, Enums.RecurringType.Daily);


          
       


            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 1)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 2)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 3)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 4)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 5)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 6)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 7)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 8)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 9)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 10)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 11)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 12)));
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 14)));
        }

        public void Scenario_1()
        {
            //Scenario 1: Making docklist and shiplist of same type and size: 

            // Made an instance of harbor: 
            Harbor harbor = new Harbor("Test Harbor", new CargoStorage("CargoStorage", 100));

            // Made a list of 5 docks of the same Type and Size: 
            List<Dock> docks = harbor.InitializeDocks(5, Model.ContainerShip, Size.Large, 2);


            // Made a list of 5 ships of the same Type and Size: 
            List<Ship> ships = harbor.InitializeShips(5, Model.ContainerShip, Size.Large, 10);
           
            //Made a instance of our simulation - class that implements IHarborSimulation interface:  
            IHarborSimulation driver = new Simulation();

            // Made a startTime and a endTime for mye simulation: 
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 5);

            // Made a startTime for when sailing starts for all ships in harbor: 
            DateTime startSailingTime = new DateTime(2024, 1, 1);


            // Runing the simulation: 
            driver.Run(harbor, startTime, endTime, ships, docks, startSailingTime, 2, true, Enums.RecurringType.Daily);


            //Printing out harbor history to view harborstate: 
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 1)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 2)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 3)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 4)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 5)));
           
        }
    }
}
