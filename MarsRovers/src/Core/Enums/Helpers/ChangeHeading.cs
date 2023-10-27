using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Core.Enums.Helpers
{
    public class ChangeHeading
    {
        // Direction should be only = 'L' or 'R'

        public static Heading ChangeDirection(Heading heading, char direction)
        {
            var allowedDirections = new Dictionary<char, int>()
            {
                { 'L', 1 },
                { 'R', 2 }
            };

            // Only cast once
            int headingValue = (int)heading;

            if (allowedDirections.ContainsKey(direction))
            {
                // Cast heading as int to compare, enum Heading range from 0-3
                if (direction == 'R' && headingValue < 3 && headingValue >= 0)
                {
                    headingValue += 1;
                    return (Heading)headingValue;
                }
                else if (direction == 'R' && headingValue == 3)
                {
                    // Turn right at 3 then loop back to 0.
                    return Heading.N;
                }
                else if (direction == 'L' && headingValue == 0)
                {
                    // Turn left at 0 then loop back to 3.
                    return Heading.W;
                }
                else if (direction == 'L' && headingValue > 0 && headingValue <= 3)
                {
                    // Turn left when not zero but within range of enum
                    headingValue -= 1;
                    return (Heading)headingValue;
                }
            }

            Console.WriteLine("Heading failed to convert as direction given was invalid.");
            return heading;    
        }
    }
}
