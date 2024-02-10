using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.ComponentModel.Design;



namespace Cdull.V2024.HarborSimulation.TestClient
{

    public class Simulation : IHarborSimulation
    {
      

        void IHarborSimulation.Run(Harbor harbor, DateTime starttime, DateTime endTime, int numberOfShips, 
            Enums.Size shipSize, int numberOfCargoOnShip, Enums.Model shipModel, int numberOfDocks, int numberOfCranes, 
            Enums.Size dockSize, Enums.Model dockModel)
        {
            var currentTime = starttime;
            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.DockedShips.Clear();
            harbor.SailingShips.Clear();
            harbor.InitializeDocks(numberOfDocks, dockModel, dockSize, numberOfCranes);
            harbor.InitializeShips(numberOfShips, shipSize, shipModel, numberOfCargoOnShip);
            harbor.QueueShipsToDock();
           

            while (currentTime < endTime)
            {
                harbor.DockShips(currentTime);
                harbor.AddCargoToStorage();
                harbor.AddCargoToShips(10, currentTime);
                

                // Geting the ship I want to sail:
              /* foreach (Ship ship in harbor.Ships)
               {
                    DateTime date = new DateTime(2024, 1, 3);
                    if (ship.HasDocked && !ship.IsSailing) 
                    {              
                       harbor.Sailing(ship, currentTime, new DateTime(2024, 1, 2), 2);                       

                    }

                }*/

                if (currentTime.Hour == 0 && currentTime.Minute == 0)
                {

                    harbor.SaveHarborHistroy(currentTime.Date);

                }



                /* foreach (Ship ship in harbor.Ships)
                 {
                     if (ship.HasDocked && !ship.IsSailing)
                     {
                         harbor.RecurringSailing(new DateTime(2024, 1, 2), 1, currentTime, ship, Enums.RecurringType.Daily);

                     }
                 }*/

                currentTime = currentTime.AddMinutes(1);
            
                if (currentTime.Date >= endTime.Date)
                {
             
                    break;
                }
           
            }

     
            


        }
    }


}
