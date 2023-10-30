using MarsRovers.src.Features.MarsRover;
using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRovers.src.Utilities
{
    public class MarsRoverUtility
    {
        public static void ExecuteMarsRoverCaluclationsInParallel(List<Tuple<string, string, string, string, string, string, int>> input, string inputFilePath = "")
        {
            // Count the number of skipped Mars Rovers
            int marsRoverSkipCounter = 0;

            // Create ConcurrentQueue to be returned - Use ConcurrentQueue because (O)1 complexity when pushing and popping, and Type is thread safe
            ConcurrentQueue<MarsRover> marsRoverResults = new ();

            // Declare bag
            ConcurrentBag<MarsRover> marsRoverCalculations = new ();

            // Declare Task list to be added to the bag
            List<Task> bagAddTasks = new ();

            // Create all MarsRover objects concurrently from input and add them to bag + Dictionary
            foreach (var tuple in input)
            {
                // Create MarsRover from input

                MarsRover currentRover = new (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7);

                // Ensure no null rovers are added to marsRoverCalculations by generating default non null value
                currentRover =  currentRover ?? new(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty);


                // Add rovers to bag if the input was valid and object created did not throw an exception -> OutputOrder != -1
                if (!(String.IsNullOrEmpty(tuple.Item3) || String.IsNullOrEmpty(tuple.Item4) || String.IsNullOrEmpty(tuple.Item5) || String.IsNullOrEmpty(tuple.Item6)) && currentRover.OutputOrder != -1)
                {
                    bagAddTasks.Add(Task.Run(() => marsRoverCalculations.Add(currentRover)));
                }
                else
                {
                    marsRoverSkipCounter += 1;
                }
            }

            // Wait for the MarsRovers to be created
            Task.WaitAll(bagAddTasks.ToArray());

            // Consume the MarsRovers in the bag - Perform calculations on them
            List<Task> bagConsumeTasks = new ();
            int itemsInBag = 0;

            // Track time
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            // Perform calclations and add resulting items to ConcurrentQueue - (O)1 complexity when pushing and popping
            while (!marsRoverCalculations.IsEmpty)
            {
                bagConsumeTasks.Add(Task.Run(() =>
                {
                    // MarsRover marsRover;

                    if (marsRoverCalculations.TryTake(out MarsRover marsRover))
                    {
                        // Assume null rover is end of file or incorrectly defined rover
                        try
                        {
                            marsRover.CalculateMomement();
                            marsRoverResults.Enqueue(marsRover);
                        }
                        catch (ArgumentNullException)
                        {
                            // Output nothing
                        }

                        // Increment itemsInBag by refernece to avoid copying the variable from memory
                        Interlocked.Increment(ref itemsInBag);
                    }
                }));
            }


            // Wait for calculations to complete
            Task.WaitAll(bagConsumeTasks.ToArray());
            watch.Stop();
            if (itemsInBag > 0)
            {
                AnsiConsole.Markup($"There were [lightgreen_1]{itemsInBag}[/] Mars Rovers in the concurrent bag. All [slateblue1]{itemsInBag}[/] calculations took [mediumvioletred]{watch.ElapsedMilliseconds}[/] ms to complete.\n");

                // Prompt user to display results
                if (AnsiConsole.Confirm("Output Results? Note: [yellow]If total inputted Mars Rover >= 100, the results will be appended to the input file.[/]"))
                {
                    // Use dictionary for O(1) time complexity when adding results to output as well as retrieving them for output
                    Dictionary<int, MarsRover> marsRoverOutput = new ();

                    var test = marsRoverResults.ToArray();

                    // FIFO - Loop through each item and store in outputArroutputMarsRovers dict based on OutputOrder - O(N*1) Time complexity
                    foreach (MarsRover marsRover in test)
                    {
                        marsRoverOutput.Add(marsRover.OutputOrder, marsRover);
                    }

                    // Based on sorted liftime array, output Mars Rover results from Dictionary
                    for (int c = 0; c < marsRoverOutput.Count; c++)
                    {
                        if (marsRoverOutput.TryGetValue(c, out var marsRover))
                        {
                            // Output different colors for fun based on even or odd
                            string outputString = (c % 2 == 0) ? $"[springgreen3]{marsRover}[/]\n" : $"[indianred1]{marsRover}[/]\n";

                            // Append results to end of input file if inputted Mars Rovers >= 100 and inputFilePath is not empty
                            if (marsRoverOutput.Count >= 100 && !string.IsNullOrEmpty(inputFilePath)) 
                            {
                                using (StreamWriter outputFile = new StreamWriter(inputFilePath, true))
                                {
                                    outputFile.WriteLine(marsRover);
                                }
                            }
                            // Console.WriteLine(marsRover.OutputOrder); - for testing purposes
                            AnsiConsole.Markup(outputString);
                        }
                        else
                        {
                            Console.WriteLine($"Inputted item {c} not found.");
                        }
                    }

                    // Finally output skipped mars rovers if there are any
                    if (marsRoverSkipCounter > 0)
                    {
                        Console.WriteLine(marsRoverSkipCounter + " total Mars Rover skipped as a result of invalid instructions.");
                    }
                }
            }
            else
            {
                AnsiConsole.Markup("[red]It looks like no valid Mars Rovers were inputted. Please try again.[/]\n");
            }

            // If this line is outputted something went seriously wrong
            if (marsRoverCalculations.TryPeek(out MarsRover unexpectedMarsRover))
                Console.WriteLine("Found a Mars Rover in the bag when it should be empty");
        }
    }
}
