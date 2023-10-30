using MarsRovers.src.Core.Enums;
using MarsRovers.src.Core.Sctructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Core.Interfaces
{
    internal interface IMovementCalculator
    {
        // Position assumes that the MarsRover does not wish to breakdown when attempting to exiting boundaries.
        // Therefore, the MarsRover will hold position until receiving a valid movement command.
        // Calculate movement given movementInstructions, headingDirection, and position. Return the resulting Position and headingDirection.
        // Although input may be null, return value must always be not null.

        Position Calculate(string movementInstructions, Position position);
    }
}
