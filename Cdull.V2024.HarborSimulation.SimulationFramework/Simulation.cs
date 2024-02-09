using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.ComponentModel.Design;



namespace Cdull.V2024.HarborSimulation.TestClient
{

    public class Simulation : IHarborSimulation
    {
      

        void IHarborSimulation.Run(Harbor harbor, DateTime starttime, DateTime endTime, int numberOfShips, 
            Enums.Size shipSize, int numberOfCargoOnShip, Enums.Model shipModel, int numberOfDocks, int numberOfCranes, Enums.Size dockSize, Enums.Model dockModel)
        {
            var currentTime = starttime;
            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.DockedShips.Clear();
            harbor.SailingShips.Clear();
            harbor.InitializeDocks(numberOfDocks, dockModel, dockSize, numberOfCranes);
            harbor.InitializeShips(harbor, numberOfShips, shipSize, shipModel, numberOfCargoOnShip);
            Console.WriteLine($"Currentime: {currentTime}");
            Console.WriteLine($"HARBOR SIMULATION STARTED: {harbor.name}");
            harbor.QueueShipsToDock();

            while (currentTime < endTime)
            {

           

                if (currentTime.Hour == 0 && currentTime.Minute == 0)
                {

                    harbor.SaveHarborHistroy(currentTime);
                 
                }

                harbor.DockShips(currentTime);

                harbor.AddCargoToStorage();
                harbor.AddCargoToShips(10, currentTime);




                // Geting the ship I want to sail:
                foreach (Ship ship in harbor.Ships)
                {
                    if (ship.HasDocked && !ship.IsSailing)
                    {
                        for (int i = 0; i > (harbor.Ships.Count / 2); i++)
                        {
                            harbor.Sailing(ship, currentTime, new DateTime(2024, 1, 2), 1);
                        }

                    }

                }

                currentTime = currentTime.AddMinutes(1);
            
                if (currentTime.Date >= endTime.Date)
                {
             
                    break;
                }
           
            }

     
            


        }
    }


}
