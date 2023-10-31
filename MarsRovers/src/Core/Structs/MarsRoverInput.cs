using MarsRovers.src.Core.Sctructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Core.Structs
{
    // Tuple<string, string, string, string, string, string, int>
    // Same fields as a MarsRover, but using strings instead - Input will be validated prior to creating a struct
    public struct MarsRoverInput
    {
        public string XAxisBound;
        public string YAxisBound;
        public string XOrigin;
        public string YOrigin;
        public string DirectionalHeading;
        public string TurnMoveInstructions;
        public int OutputOrder;
    }
}
