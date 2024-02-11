using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.ComponentModel.Design;



namespace Cdull.V2024.HarborSimulation.TestClient
{

    public class Simulation : IHarborSimulation
    {
      

        void IHarborSimulation.Run(Harbor harbor, DateTime starttime, DateTime endTime, List<Ship> ships, List<Dock> docks)
        {
            var currentTime = starttime;

            // resetting all lists in harbor: 
            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.Docks.Clear();
            harbor.DockedShips.Clear();
            harbor.SailingShips.Clear();

            // Adding ships and docks to harbor: 
          
            harbor.Docks.AddRange(docks);
            harbor.Ships.AddRange(ships);


            while (currentTime < endTime)
            {

                harbor.DockShips(currentTime);
                harbor.AddCargoToStorage();
                harbor.AddCargoToShips(10, currentTime);

                harbor.QueueShipsToDock();


                foreach (Ship ship in harbor.Ships)
                {
                    DateTime date = new DateTime(2024, 1, 3);
                    if (ship.HasDocked && !ship.IsSailing)
                    {
                        harbor.Sailing(ship, currentTime, date, 1);

                    }

                }

                // Geting the ship I want to sail:


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
