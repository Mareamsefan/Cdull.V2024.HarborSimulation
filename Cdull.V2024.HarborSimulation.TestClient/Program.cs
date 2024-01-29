using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.Runtime.CompilerServices;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Harbor harbor = new Harbor("My Harbor", new List<Dock>(), new List<Ship>(), new Queue<Ship>(), new List<Crane>(), new List<CargoStorage>());

            harbor.InitializeCranes(10);
            List<Crane> cranes = harbor.GetCraneList();  
            harbor.InitializeDocks(10, "normal", "small", cranes);
            Console.WriteLine(harbor);


            CargoStorage cargoStorage = new("cargoS");
            Cargo cargo = new("cargo", 24);
            Ship ship = new("ship", "jhsajdh", "small");
            harbor.AddCargoToStorage(ship, ship.);

        }
    }
}