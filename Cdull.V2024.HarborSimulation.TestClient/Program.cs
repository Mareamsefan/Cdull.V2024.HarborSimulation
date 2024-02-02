using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.ComponentModel.DataAnnotations;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //CargoStorage harborCargoStorage = new("cargoStorage");  
            // Eksempel på starttidspunkt (1. januar 2024 kl. 00:00:00)
            // År , måned, dag 
            DateTime startTime = new DateTime(2023, 1, 1);
            DateTime EndTime = new DateTime(2023, 1, 1);
            // Eksempel på slutttidspunkt (5. januar 2024 kl. 23:59:59)

            Harbor harbor = new Harbor("My Harbor", new CargoStorage("cargoStoage"));
            harbor.InitializeShips(10, Enums.Size.Small, Enums.ShipType.ContainerShip, 10);
            harbor.InitializeCranes(10);
            harbor.InitializeDocks(10, Enums.DockType.ContainerDock, Enums.Size.Medium);
            Console.WriteLine(harbor);
            harbor.SetUpWatch(new DateTime(2023, 12, 03), new DateTime(2024, 01, 02));
            Console.WriteLine(harbor.Watch.StartTime);
            Console.WriteLine(harbor.Watch.EndTime);
            harbor.QueueShipsToDock(); 
            harbor.DockShips();

            Console.WriteLine("\n");

            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.Docks.Clear();

            DateTime mintid = new DateTime(2024, 2, 2, 10, 0, 0);
            harbor.Watch.StartCountingTime(mintid);
            harbor.Watch.StartCountingTime(mintid);
            Console.WriteLine(mintid);

            //Ny (se på etterpå)
            TimeSpan leggetiltid = TimeSpan.FromMinutes(10);
            harbor.Watch.AddTime(leggetiltid);

            //se på dette iforhold til tid
            harbor.Watch.StopCountingTime();
            Console.WriteLine(harbor.Watch.StopCountingTime());
            Console.WriteLine(harbor.Watch.MeasureTimeElapsed());

            Crane nycrane = new Crane("BigCrane");
            Dock nydock = new Dock("Anders", Enums.Size.Large, Enums.DockType.ContainerDock, nycrane);
            Ship badBoiShip = new Ship("FarligShip", Enums.ShipType.ContainerShip, Enums.Size.Large);
            harbor.Ships.Add(badBoiShip);
            harbor.Docks.Add(nydock);
            harbor.QueueShipsToDock();

            



            Console.WriteLine("\n");
            Console.WriteLine(harbor.WaitingShips.Count);
            Console.WriteLine(harbor.Docks.Count);
            Console.WriteLine(nydock.IsAvailable);
            Console.WriteLine(harbor.AvailableDockOfSize(badBoiShip.Size));
            Console.WriteLine(harbor.DockShips());
            Console.WriteLine(badBoiShip.HasDocked);


            //NY
            Console.WriteLine("\n");
            Console.WriteLine("HER");
            //Gir ikke ut noe?
            Console.WriteLine(nydock.OccupiedBy);
            Console.WriteLine("Docken Anders er tilgjengelig? " + nydock.IsAvailable);
            harbor.RemoveShipFromDock(nydock);
            Console.WriteLine("\n");














        }
    }
}