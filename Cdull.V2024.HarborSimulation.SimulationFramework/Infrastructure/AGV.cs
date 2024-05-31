
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents an Automated Guided Vehicle (AGV) used in the harbor.
    /// </summary>

    public class AGV
    {
        /// <summary>
        /// Represents the _Id of the AGV. 
        /// </summary>
        private int _id;

        private static int s_counter; 

        private bool _isAvailable;

        private Container? _container;

        private int _location; 

        private int _speed; 

        /// <summary>
        /// Initializes a new instance of the <see cref="AGV"/> class with the specified location.
        /// </summary>
        /// <param name="agvLocation">The initial location of the AGV within the harbor.</param>
        public AGV(int agvLocation)
        {
            s_counter++;
            _id = s_counter;
            _location = agvLocation;
            _isAvailable = true;
            _speed = 7;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AGV"/> class with the specified location and speed.
        /// </summary>
        /// <param name="agvLocation">The initial location of the AGV within the harbor.</param>
        /// <param name="agvSpeed">The speed of the AGV.</param>
        public AGV(int agvLocation, int agvSpeed)
        {
            s_counter++;
            _id = s_counter;
            _location = agvLocation;
            _isAvailable = true;
            _speed = agvSpeed;
        }


        /// <summary>
        /// Loads the specified container onto the AGV.
        /// </summary>
        /// <param name="container">The container to be loaded onto the AGV.</param>
        /// <remarks>
        /// This method loads the specified container onto the AGV for transportation within the harbor.
        /// </remarks>
        internal void LoadContainerToAGV(Container container)
        {

            _container = container;

        }

        /// <summary>
        /// Gets the _Id of the AGV.
        /// </summary>
        public int GetId => _id;  


        /// <summary>
        /// Gets whether the AGV is available.
        /// </summary>
        public bool GetIsAvailable => _isAvailable;


        /// <summary>
        /// Sets whether the AGV is available.
        /// </summary>
        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable;


        /// <summary>
        /// Gets the container currently loaded on the AGV.
        /// </summary>
        public Container GetContainer => _container; 

        /// <summary>
        /// Sets the container to be loaded on the AGV.
        /// </summary>
        public void SetContainer(Container container) => _container = container;
       

        /// <summary>
        /// Gets the location of the AGV.
        /// </summary>
        public int GetLocation => _location;
        

        /// <summary>
        /// Sets the location of the AGV.
        /// </summary>
        public void SetLocation(int location) => _location = location;
        

        /// <summary>
        /// Gets the speed of the AGV.
        /// </summary>
        public int GetSpeed => _speed;
      

        /// <summary>
        /// Sets the speed of the AGV.
        /// </summary>
        public void SetSpeed(int speed) => _speed = speed;
        
    }
}
