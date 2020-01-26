using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge
{
    class Program
    {
        static ElevatorHandler _elevatorHandler;
        static void Main(string[] args)
        {
            Console.Title = "Elevator Challenge";
            Console.WriteLine("Elevator Challenge!");

            Console.WriteLine("Config:");
            var elevatorCount = ReadAndValidateIntegerInput("Elevator Count: ");
            var floorCount = ReadAndValidateIntegerInput("Floor Count: ");
            var weightLimit = ReadAndValidateIntegerInput("Weight Limit: ");

            displayHelp();

            _elevatorHandler = new ElevatorHandler(elevatorCount, floorCount, weightLimit);
            _elevatorHandler.ElevatorArrived += ElevatorHandler_ElevatorArrived;

            Console.WriteLine("You can type help to issue a list of all available commands and example inputs.");
            while (true)
            {
                Console.Write("Command: ");
                var input = Console.ReadLine();
                var command = input.ToLower().Split(' ')[0];
                int paramater = 0;
                if (input.ToLower().Split(' ').Count() > 1 && !int.TryParse(input.Split(' ')[1].ToString(), out paramater))
                {
                    if (string.IsNullOrEmpty(input[1].ToString()))
                    {
                        Console.WriteLine("Please specify your floor");
                    }
                    continue;
                }

                switch (command)
                {
                    case "help":
                        displayHelp();
                        break;
                    case "call":
                        if (paramater > floorCount)
                        {
                            Console.WriteLine("Invalid floor input");
                            continue;
                        }

                        var elevator = _elevatorHandler.CallElevator(paramater);
                        elevator = boardAndUnboardPassengers(elevator);
                        var newFloorTargetFloor = ReadAndValidateIntegerInput("New target floor: ");
                        elevator.SelectNewFloor(newFloorTargetFloor);
                        break;
                    case "list":
                        listElevatorStates();
                        break;
                    default:
                        break;
                }
            }
        }

        static void ElevatorHandler_ElevatorArrived(object sender, EventArgs e)
        {
            var elevator = (Elevator)sender;
            Console.WriteLine($"Elevator {elevator.ElevatorId} has arrived at floor {elevator.CurrentFloor}");
        }

        static int ReadAndValidateIntegerInput(string prompt)
        {
            int returnInteger;
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out returnInteger))
            {
                return returnInteger;
            }
            return ReadAndValidateIntegerInput(prompt);
        }

        static void displayHelp()
        {
            Console.WriteLine("Help:");
            Console.WriteLine("Command: call {floor Number}");
            Console.WriteLine("Description: Calls the nearest elevator to the floor provided.");
            Console.WriteLine("Example: \"Command: call 3\"");
            Console.WriteLine($"{Environment.NewLine}");
            Console.WriteLine("Command: list");
            Console.WriteLine("Description: Dispays the location and state of all lifts.");
            Console.WriteLine("Example: \"Command: list\"");
            Console.WriteLine($"{Environment.NewLine}");
        }

        static void listElevatorStates()
        {
            foreach (var elevator in _elevatorHandler.Elevators)
            {
                Console.WriteLine($"Elevator {elevator.ElevatorId} is at floor {elevator.CurrentFloor} and has state {elevator.ElevatorState}");
            }
        }

        static Elevator boardAndUnboardPassengers(Elevator elevator)
        {
            var enteringPassengers = ReadAndValidateIntegerInput("Number of passengers entering: ");
            var leavingPassengers = ReadAndValidateIntegerInput("Number of passengers leaving: ");

            try
            {
                elevator.BoardAndUnboardPassengers(enteringPassengers, leavingPassengers);
            }
            catch (WeightOverloadException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("The lift has been reset as if nobody new enetered or left the elevator");
                return boardAndUnboardPassengers(elevator);
            }

            return elevator;
        }
    }
}
    