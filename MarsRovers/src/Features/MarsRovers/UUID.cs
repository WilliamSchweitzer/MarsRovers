using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Features.MarsRovers
{
    internal static class UUID
    {
        public static HashSet<string> UUIDS = new HashSet<string>();

        internal static string RecursivelyGenerateUUID(string currentUUID = "")
        {
            // Return currentUUID, if the parameter passed is not an empty string - base case
            if (!string.IsNullOrEmpty(currentUUID))
            {
                return currentUUID;
            }
            else
            {
                // UUID to generate
                currentUUID = Guid.NewGuid().ToString();

                // If generated UUID is not in the HashSet, add it and return generated UUID - recusrion complete
                if (!UUIDS.Contains(currentUUID))
                {
                    UUIDS.Add(currentUUID);
                    return currentUUID;
                }
                // Else, UUID is contained in in the HashSet (UUID not unique), return an empty string to the recursive function to generate a new UUID - recursive step (start recursion)
                else
                {
                    return RecursivelyGenerateUUID(string.Empty);
                }
            }

        }
    }
}