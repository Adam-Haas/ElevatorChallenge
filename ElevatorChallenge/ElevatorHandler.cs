using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge
{
    public class ElevatorHandler
    {
        private readonly int _numberOfElevators;
        private readonly int _numberOfFloors;
        private readonly int _weightLimit;
        public IList<Elevator> Elevators;
        public event EventHandler ElevatorArrived;

        public ElevatorHandler(int numberOfElevators, int numberOfFloors, int weightLimit)
        {
            _numberOfElevators = numberOfElevators;
            _numberOfFloors = numberOfFloors;
            _weightLimit = weightLimit;
            initializeHandler();
        }

        private void initializeHandler()
        {
            //Initilize the _elevators variable and assign all available elevators
            Elevators = new List<Elevator>();

            for (int i = 0; i < _numberOfElevators; i++)
            {
                var newElevator = new Elevator(i, _weightLimit) { ElevatorState = ElevatorStates.Stationary };
                newElevator.ArrivedAtNewFloor += ElevatorArrivedAtNewFloor;
                Elevators.Add(newElevator);
            }
        }

        private void ElevatorArrivedAtNewFloor(object sender, EventArgs e)
        {
            ElevatorArrived?.Invoke(sender, e);
        }

        public Elevator CallElevator(int floorNumber)
        {
            //Calculate the differences between all of the elevators and the current floor and then find the elevator whos floor is nearest to the requested floor
            var nearestElevator = Elevators.FirstOrDefault(x => x.CurrentFloor == Elevators.Select(y => y.CurrentFloor - floorNumber).Min() + floorNumber);

            nearestElevator.SelectNewFloor(floorNumber);
            return nearestElevator;
        }
    }
}
