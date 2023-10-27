using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Core.Enums.Helpers
{
    internal class HeadingConversion
    {
        // If you don't know where you are going assume north. (:
        public static Heading ConvertToHeading(char heading)
        {
            switch (heading)
            {
                case 'N':
                    return Heading.N;
                case 'E':
                    return Heading.E;
                case 'S':
                    return Heading.S;
                case 'W':
                    return Heading.W;
                default:
                    return Heading.N;
            }
        }
    }
}
