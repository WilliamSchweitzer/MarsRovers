using MarsRovers.src.Core.Enums;
using MarsRovers.src.Core.Sctructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// MarsRover(XAxisBound, YAxisBound, XOrigin, YOrigin, DirectionalHeading, TurnMoveInstructions)
// XAxisBound, YAxisBound, XOrigin, YOrigin are all ulong type
// DirectionalHeading is char? type - null = 'N',
// TurnMoveInstructions is string? type - null value = "" or no movement

namespace MarsRovers.src.Core.Interfaces
{
    internal interface IMarsRover
    {
        Position Position { get; set; }

        Heading Heading { get; set; }

        // direction with bool value 'true' should be considered a positive direction on the given axis
        
        void CalculateMomement();
    }
}
