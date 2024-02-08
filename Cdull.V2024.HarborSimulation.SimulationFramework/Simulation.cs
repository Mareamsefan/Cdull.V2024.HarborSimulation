using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.ComponentModel.Design;



namespace Cdull.V2024.HarborSimulation.TestClient
{

    public class Simulation : IHarborSimulation
    {


        void IHarborSimulation.Run(Harbor harbor, DateTime starttime, DateTime endTime, int numberOfShips, Enums.Size shipSize, int numberOfCargoOnShip, Enums.Model shipModel, int numberOfDocks, int numberOfCranes, Enums.Size dockSize, Enums.Model dockModel)
        {
            var currentTime = starttime;
            CargoStorage cargoStorage = new CargoStorage("CargoStorage");
            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.DockedShips.Clear();
            harbor.SailingShips.Clear();
            harbor.InitializeDocks(numberOfDocks, dockModel, dockSize, numberOfCranes);
            harbor.InitializeShips(harbor, numberOfShips, shipSize, shipModel, numberOfCargoOnShip);
            Console.WriteLine($"Currentime: {currentTime}");
            Console.WriteLine($"HARBOR SIMULATION STARTED: {harbor.name}");


            while (currentTime < endTime)
            {

                harbor.QueueShipsToDock();

                harbor.DockShips(currentTime);

                harbor.AddCargoToStorage();
                harbor.AddCargoToShips(10, currentTime);

                // Geting the ship I want to sail:
                foreach(Ship ship in harbor.Ships)
                {
                    harbor.Sailing(ship, currentTime, new DateTime(2024, 1, 2), 1);
                }
                
                if (currentTime.Hour == 0 && currentTime.Minute == 0)
                {
                    Console.WriteLine($"\n");
                    String currentTimex = currentTime.ToString();   
                    Console.WriteLine($"Currentime: {currentTime}\n");

                    Console.WriteLine($"ships: " + harbor.Ships.Count());

                    Console.WriteLine($"Docks: " + harbor.Docks.Count());


                    Console.WriteLine($"ships waiting to dock to harbor: " + harbor.WaitingShips.Count());


                    Console.WriteLine($"ships docked in harbor: " + harbor.DockedShips.Count());

                    Console.WriteLine($"ships sailing: " + harbor.SailingShips.Count());

                    foreach (Ship ship in harbor.DockedShips)
                    {
                        Console.WriteLine($"\n{ship.Name}: Docked at {ship.DockedAtTime} \n");
                        Console.WriteLine($"\n{ship.Name}: Has {ship.Cargo.Count()} cargo\n");
                        Console.WriteLine($"\n{ship.Name}: Is ready to sail --> {ship.IsReadyToSail}\n");
                    }

                    foreach (Ship ship in harbor.SailingShips)
                    {
                        Console.WriteLine($"\n{ship.Name}: Sailed at {ship.SailedAtTime} \n");

                    }



                }

                currentTime = currentTime.AddMinutes(1);

                if (currentTime >= endTime)
                {
                    break;
                }
            }



        }
    }


}
