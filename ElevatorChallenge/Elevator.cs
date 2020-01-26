using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorChallenge
{
    public class Elevator
    {
        public event EventHandler ArrivedAtNewFloor;

        public Elevator(int elevatorId, int weightLimit)
        {
            ElevatorId = elevatorId;
            WeightLimit = weightLimit;
        }

        public int ElevatorId { get; set; }

        private ElevatorStates _elevatorState;
        public ElevatorStates ElevatorState
        {
            get => _elevatorState;
            set
            {
                _elevatorState = value;
                UpdateElevatorState();
            }
        }

        public int CurrentFloor { get; set; }
        public int TargetFloor { get; set; }
        public int PassengerCount { get; set; }
        public int WeightLimit { get; set; }

        public void BoardAndUnboardPassengers(int boardingPassengerCount, int unboardingPassengerCount)
        {
            var newPassengerCount = PassengerCount;
            newPassengerCount -= unboardingPassengerCount;
            newPassengerCount += boardingPassengerCount;

            if (newPassengerCount > WeightLimit)
                throw new WeightOverloadException($"Weight Overload, please decrease the number of passengers in the lift by {newPassengerCount - WeightLimit}", newPassengerCount - WeightLimit);
            else
                PassengerCount = newPassengerCount;
        }

        public void SelectNewFloor(int requestedFloor)
        {
            TargetFloor = requestedFloor;
            ElevatorState = requestedFloor > CurrentFloor ? ElevatorStates.MovingUp : ElevatorStates.MovingDown;
        }


        private void UpdateElevatorState()
        {
            switch (_elevatorState)
            {
                case ElevatorStates.MovingUp:
                case ElevatorStates.MovingDown:
                    //Simulate some movement time
                    Thread.Sleep(1000);

                    //The elevator is now stationary
                    ElevatorState = ElevatorStates.Stationary;

                    //Raise the event that the elevator has arrived
                    ArrivedAtNewFloor?.Invoke(this, new EventArgs());
                    break;
                case ElevatorStates.Stationary:
                    CurrentFloor = TargetFloor;
                    break;
            }
        }
    }
}
