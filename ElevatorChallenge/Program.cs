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
            //Set the title of the console
            Console.Title = "Elevator Challenge";
            Console.WriteLine("Elevator Challenge!");

            //Configure the number of elevators, the number of floors and the weight limit for all of the elevators
            Console.WriteLine("Config:");
            var elevatorCount = ReadAndValidateIntegerInput("Elevator Count: ");
            var floorCount = ReadAndValidateIntegerInput("Floor Count: ");
            var weightLimit = ReadAndValidateIntegerInput("Weight Limit: ");

            //Display the available commands
            displayHelp();

            //Create the handler amd assign the event handler for an elevator ariving
            _elevatorHandler = new ElevatorHandler(elevatorCount, floorCount, weightLimit);
            _elevatorHandler.ElevatorArrived += ElevatorHandler_ElevatorArrived;

            //Enter the main program loop awaiting user input
            while (true)
            {
                Console.Write(Environment.NewLine);
                Console.Write("Command: ");

                //Read in what the user has entered
                var input = Console.ReadLine();

                //Split it into the command the provided paramater
                var command = input.ToLower().Split(' ')[0];

                switch (command)
                {
                    case "help":
                        displayHelp();
                        break;
                    case "call":
                        int paramater = 0;
                        //If the user did mot specify a floor
                        if (input.ToLower().Split(' ').Count() > 1 && !int.TryParse(input.Split(' ')[1].ToString(), out paramater))
                        {
                            if (string.IsNullOrEmpty(input[1].ToString()))
                            {
                                Console.WriteLine("Please specify your floor");
                            }
                            continue;
                        }

                        //If the floor specified does not exist
                        if (paramater > floorCount || paramater < 0)
                        {
                            Console.WriteLine("Invalid floor input");
                            continue;
                        }

                        //call the elevator
                        var elevator = _elevatorHandler.CallElevator(paramater);
                        //board the new passengers
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
            Console.Write(Environment.NewLine);
            Console.WriteLine("Command: list");
            Console.WriteLine("Description: Dispays the location and state of all lifts.");
            Console.WriteLine("Example: \"Command: list\"");
            Console.Write(Environment.NewLine);
            Console.WriteLine("Command: help");
            Console.WriteLine("Description: Dispays the available commands.");
            Console.WriteLine("Example: \"Command: help\"");
            Console.Write(Environment.NewLine);
        }


        static void listElevatorStates()
        {
            foreach (var elevator in _elevatorHandler.Elevators)
            {
                Console.WriteLine($"Elevator {elevator.ElevatorId} is at floor {elevator.CurrentFloor} and has state {elevator.ElevatorState}");
            }
        }

        //Board the new passengers
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
                //The passsengers have gone over the weight limit
                Console.WriteLine(ex.Message);
                Console.WriteLine("The lift has been reset as if nobody new enetered or left the elevator");
                //Try to go again
                return boardAndUnboardPassengers(elevator);
            }

            return elevator;
        }
    }
}
    