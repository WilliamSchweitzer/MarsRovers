using MarsRovers.src.Core.Sctructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Core.Structs
{
    // Tuple<string, string, string, string, string, string, int>
    // Same properties as a MarsRover, but using strings instead - Input will be validated prior to creating a struct
    public struct MarsRoverInput
    {
        public string XAxisBound { get; set; }
        public string YAxisBound { get; set; }
        public string XOrigin { get; set; }
        public string YOrigin { get; set; }
        public string DirectionalHeading { get; set; }
        public string TurnMoveInstructions { get; }
        public int OutputOrder { get; set; }
    }
}
