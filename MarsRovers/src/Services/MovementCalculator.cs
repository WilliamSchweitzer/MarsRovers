using MarsRovers.src.Core.Enums;
using MarsRovers.src.Core.Enums.Helpers;
using MarsRovers.src.Core.Interfaces;
using MarsRovers.src.Core.Sctructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Services
{
    public class MovementCalculator : IMovementCalculator
    {
        // Characters to consider 'L', 'R', 'M'
        // Note: Heading will default to 'N'
        public Position Calculate(string movementInstructions, Position position)
        {
            Console.WriteLine("Moved to:");

            // Usecase: No movement instructions given
            if (String.IsNullOrEmpty(movementInstructions)) 
            { 
                return position;
            }

            // If instructions contain any character outside of 'L', 'R', or 'M' then do nothing
            foreach (char instruction in movementInstructions)
            {
                if (instruction == 'L' || instruction == 'R')
                {
                    position.Heading = ChangeHeading.ChangeDirection(position.Heading, instruction);
                }
                else if (instruction == 'M')
                {
                    position.Move();
                }
                else
                {
                    Console.WriteLine("Incorrect instruction skipped.");
                }
            }

            // Successful calculation
            return position;
        }
    }
}
