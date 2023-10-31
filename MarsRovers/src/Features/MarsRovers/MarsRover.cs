using MarsRovers.src.Core.Enums;
using MarsRovers.src.Core.Enums.Helpers;
using MarsRovers.src.Core.Interfaces;
using MarsRovers.src.Core.Sctructs;
using MarsRovers.src.Features.MarsRovers;
using MarsRovers.src.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Features.MarsRover
{
    public class MarsRover : IMarsRover
    {
        private ulong _XAxisBound;
        private ulong _YAxisBound;
        private ulong _XOrigin;
        private ulong _YOrigin;
        private char _DirectionalHeading;
        private string? _TurnMoveInstructions { get; }

        public string? Uuid { get; } // This was going to be used for key in parallel calculation. Just adding OutputOrder was a much better solution.
        public Position Position { get; set; }
        public int OutputOrder { get; set; }

        public MarsRover(string? xAxisBoundInput, string? yAxisBoundInput, string? xOriginInput, string? yOriginInput, string? directionalHeadingInput, string? turnMoveInstructionsInput, int outputOrder = -1)
        {
            // ulong must be 0-64bit, therefore code should throw OverflowException

            try
            {
                OutputOrder = outputOrder;
                Uuid = UUID.RecursivelyGenerateUUID();
                _XAxisBound = Convert.ToUInt64(xAxisBoundInput);
                _YAxisBound = Convert.ToUInt64(yAxisBoundInput);
                _XOrigin = Convert.ToUInt64(xOriginInput);
                _YOrigin = Convert.ToUInt64(yOriginInput);

                // In math, a 2D plane is described by having more than 1 point that does not lie on the same line. X and Y dimensions cannot equal 0.
                if (_XAxisBound == 0 || _YAxisBound == 0)
                {
                    throw new ArgumentException("In math, a 2D plane is described by having more than 1 point that does not lie on the same line. X and Y dimensions cannot equal 0.");
                }

                // If the inputted (X, Y) origins exceed the dimensions of the board, they will be set to (0, 0)
                if (_XOrigin > _XAxisBound)
                {
                    _XOrigin = 0; 
                }

                if (_YOrigin > _YAxisBound)
                {
                    _YOrigin = 0;
                }
            }
            //catch (FormatException)
            //{
            //    Console.WriteLine("Some input caused a format exception. Program integrity lost. Stopping.");
            //    throw new StackOverflowException("Some input caused a format exception. Program integrity lost. Stopping.");
            //}
            catch (OverflowException)
            {
                Console.WriteLine("Some input caused a stack overflow. Program integrity lost. Stopping.");
                throw new StackOverflowException("Some input caused a stack overflow. Program integrity lost. Stopping.");
            }
            catch (Exception)
            {
                // Don't generate user facing error when negative values are passed to ulongs
            }

            // Attempt to convert input given to correct Types. Using null coalescing, convert null inputs to empty values or default direction for Heading case

            try
            {
                _DirectionalHeading = Convert.ToChar(directionalHeadingInput ?? "N");
                _TurnMoveInstructions = turnMoveInstructionsInput ?? "";
            }
            catch (Exception)
            {
                // Don't generate user facing error
            }

            // Convert input to Heading enum
            Heading heading = HeadingConversion.ConvertToHeading(_DirectionalHeading);
            
            // Create position struct based on above validated input
            Position = new Position(_XOrigin, _YOrigin, _XAxisBound, _YAxisBound, heading);
        }

        public Position CalculateMomement()
        {
            // Call out to MovementCalculator algorithm defined in services
            MovementCalculator marsRoverCalculator = new ();
            Position = marsRoverCalculator.Calculate(_TurnMoveInstructions ?? "", Position);
            return Position;
        }

        // Don't want to automatically generate a UUID in this case becuase it is not needed and will slow down computations during console app runtime
        string RecursivelyGenerateUUID(HashSet<string> uuids, string currentUUID = "")
        {
            // Return currentUUID, if the parameter passed is not an empty string - base case
            if (!string.IsNullOrEmpty(currentUUID))
            {
                // Could set the UUID here
                return currentUUID;
            }
            else
            {
                // UUID to generate
                currentUUID = Guid.NewGuid().ToString();

                // If generated UUID is not in the HashSet, add it and return generated UUID - recusrion complete
                if (!uuids.Contains(currentUUID))
                {
                    uuids.Add(currentUUID);
                    return currentUUID;
                }
                // Else, UUID is contained in in the HashSet (UUID not unique), return an empty string to the recursive function to generate a new UUID - recursive step (start recursion)
                else
                {
                    return RecursivelyGenerateUUID(uuids, string.Empty);
                }
            }
        }

        public override string ToString()
        {
            return $"{this.Position.X} {this.Position.Y} {this.Position.Heading}";
        }
    }
}
