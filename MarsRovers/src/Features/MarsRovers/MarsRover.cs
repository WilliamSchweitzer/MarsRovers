using MarsRovers.src.Core.Enums;
using MarsRovers.src.Core.Enums.Helpers;
using MarsRovers.src.Core.Interfaces;
using MarsRovers.src.Core.Sctructs;
using MarsRovers.src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Features.MarsRover
{
    public class MarsRover : IMarsRover
    {
        private ulong xAxisBound;
        private ulong yAxisBound;
        private ulong xOrigin;
        private ulong yOrigin;
        private char directionalHeading;
        private string? TurnMoveInstructions { get; }
        public Position Position { get; set; }
        public Heading Heading { get; set; }

        public MarsRover(string? xAxisBoundInput, string? yAxisBoundInput, string? xOriginInput, string? yOriginInput, string? directionalHeadingInput, string? turnMoveInstructionsInput)
        {
            // ulong must be 0-64bit, therefore code should throw OverflowException otherwise. Console.ReadLine() returns a string.

            try
            {
                xAxisBound = Convert.ToUInt64(xAxisBoundInput);
                yAxisBound = Convert.ToUInt64(yAxisBoundInput);
                xOrigin = Convert.ToUInt64(xOriginInput);
                yOrigin = Convert.ToUInt64(yOriginInput);
            }
            catch (OverflowException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }


            // Attempt to convert input given to correct Types. Using null coalescing, convert null inputs to empty values or default direction for Heading case

            try
            {
                directionalHeading = Convert.ToChar(directionalHeadingInput ?? "N");
                TurnMoveInstructions = turnMoveInstructionsInput ?? "";
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }

            // Convert input to Heading enum
            Heading = HeadingConversion.ConvertToHeading(directionalHeading);
            
            // Create position struct based on above validated input
            Position = new Position(xOrigin, yOrigin, xAxisBound, yAxisBound, Heading);

            // Console.WriteLine("MarsRover input validated.");
            // Console.WriteLine("--------------------------");
        }

        public void CalculateMomement()
        {
            // Call out to MovementCalculator algorithm defined in services
            MovementCalculator marsRoverCalculator = new MovementCalculator();
            Position = marsRoverCalculator.Calculate(TurnMoveInstructions, Position);
        }

        public override string ToString()
        {
            return this..ToString();
        }
    }
}
