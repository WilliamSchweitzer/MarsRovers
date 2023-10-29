using MarsRovers.src.Features.MarsRover;
using System.Runtime.Caching;
using Spectre.Console;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using MarsRovers.src.Core.Sctructs;

string RecursivelyGenerateUUID(HashSet<string> uuids, string currentUUID = "") {
    // Return currentUUID, if the parameter passed is not an empty string - base case
    if(!string.IsNullOrEmpty(currentUUID))
    {
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

List<Tuple<string, string, string, string, string, string, int>> StartManualInput()
{
    AnsiConsole.Markup("You may being entering instructions line by line. [deeppink3]Return an empty line to stop.[/]\n");

    // Use list because input size is unknown, Use Tuple to store and return all values unique to a Mars Rover together
    List<Tuple<string, string, string, string, string, string, int>> inputList = new List<Tuple<string, string, string, string, string, string, int>>();

    // Store working input value in a string as well as first input for grid dimensions, Mars Rover X, Y, H values, and instrucitions

    string? gridDimensionsInput;
    string? marsRoverXYHInput;
    string? instructionsInput = "";
    string[] marsRoverXYH = { };
    string[] gridDimensions = { };
    string[] concatgridDimensionsWithMarsRoverXYH = { };
    int counter = 0;

    // Always run, until stated in code, store first line then take input in groups of 2, else break

    while (true)
    {
        string? input = Console.ReadLine();

        // If first string is null, empty or whitespace break;
        // Else if the first string is not empty, null or whitespace, store the X, Y in gridDimensions
        // Else if second string is null, empty or whitespace break; because (count of the list (1) - 1) mod two == 0
        // Else if any second string after the X, Y bounds have been entered is null, empty or whitespace (add empty string to stack),  ((count of the list - 1) mod two) == 0 must be true
        // Else if any second string after the X, Y bounds have been entered is NOT null, empty or whitespace 
        // Else if string is null, empty or whitespace break;


        if ((counter == 0) && string.IsNullOrWhiteSpace(input))
        {
            // Prompt user again if given wrong instructions
            AnsiConsole.Markup("[red]Incorrect X, Y dimensions given. Please try again:\n[/]");
            QueryUserUntilStopCommand();
        }
        else if ((counter == 0) && !string.IsNullOrWhiteSpace(input))
        {
            // Remove leading and trailing whitespace

            gridDimensionsInput = input.Trim();

            // Split string by spaces
            gridDimensions = gridDimensionsInput.Split(' ');

            // Prompt user again if given wrong instructions
            if (gridDimensions.Length != 2) { AnsiConsole.Markup("[red]Incorrect X, Y dimensions given. Please try again:\n[/]"); QueryUserUntilStopCommand(); }

            // Else, the 2 Mars Rover grid values X, Y are stored in gridDimensions, to be returned later
            // Increment counter
            // counter++;

        }
        // Input is now any even input after the first, and the input can be null, empty, or whitespace - Consider okay because instructions can be empty
        else if ((counter % 2) == 0 && (counter != 0))
        {
            // Remove leading and trailing whitespace and set instructions equal to "" if null
            input = input ?? "";
            instructionsInput = input.Trim();

            // The instructions are stored in instructionsInput, to be returned later
            // Unless counter == 2, then we must add the first Tuple to the List

            // break; if given wrong instructions
            // if (marsRoverXYH.Length != 3) { AnsiConsole.Markup("[red]Incorrect input strings given. Program stopped early.\n[/]"); throw new ArgumentException(); }

            // First contact gridDimensions[] and marsRoverXYH[] to make array of length 5 
            // concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(marsRoverXYH).ToArray();

            // Then if the array is of correct length, combine the values from the array and instructionsInput string to create a Tuple + add it to the List
            if (concatgridDimensionsWithMarsRoverXYH.Length == 5)
            {
                int outputOrder = (counter / 2) - 1;

                inputList.Add(Tuple.Create(concatgridDimensionsWithMarsRoverXYH[0],
                    concatgridDimensionsWithMarsRoverXYH[1],
                    concatgridDimensionsWithMarsRoverXYH[2],
                    concatgridDimensionsWithMarsRoverXYH[3],
                    concatgridDimensionsWithMarsRoverXYH[4],
                    instructionsInput,
                    outputOrder));
            }

        }
        // Consider final end of input string, must be odd and empty, break;
        else if ((counter % 2 == 1) && (counter >= 3) && string.IsNullOrWhiteSpace(input))
        {
            return inputList;
        }
        // If any inputs after the first are odd and null - Mars Rover X, Y, H cannot be null, empty, or whitespace - break;
        else if (((counter % 2) == 1) && string.IsNullOrWhiteSpace(input))
        {
            // AnsiConsole.Markup("[red]Incorrect input strings given. Program stopped early.\n[/]");
            
            // Create fake input to pass to object, so program does not fail
            string[] spoofXYH = { string.Empty, string.Empty, string.Empty };
            concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(spoofXYH).ToArray();
            //throw new ArgumentException();
        }
        // If any inputs after the first are odd and NOT null - Mars Rover X, Y, H cannot be null, empty, or whitespace - prepare X, Y, H for Tuple
        // This will also be when to create and add the Tuple to the list
        else if ((counter / 2 == 1) && !string.IsNullOrWhiteSpace(input))
        {
            // Remove leading and trailing whitespace
            marsRoverXYHInput = input.Trim();

            // Split string by spaces
            marsRoverXYH = marsRoverXYHInput.Split(' ');

            // break; if given wrong instructions
            //if (marsRoverXYH.Length != 3) { AnsiConsole.Markup("[red]Incorrect input strings given. Program stopped early.\n[/]"); throw new ArgumentException(); }

            // Else, the 3 Mars Rover origin values X, Y, H are stored in marsRoverXYH, to be returned now
            // MarsRover(string? xAxisBoundInput, string? yAxisBoundInput, string? xOriginInput, string? yOriginInput, string? directionalHeadingInput, string? turnMoveInstructionsInput)
            // Format to return the tuple in

            // First contact gridDimensions[] and marsRoverXYH[] to make array of length 5 
            concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(marsRoverXYH).ToArray();
        }

        counter++;
    }
    throw new Exception("Major issue in reading instructions");
}

List<Tuple<string, string, string, string, string, string, int>> StartTextFileInput(string pathToFile)
{
    AnsiConsole.Markup("[deeppink3]Attemping to read file...[/]\n");

    // Use list because input size is unknown, Use Tuple to store and return all values unique to a Mars Rover together
    List<Tuple<string, string, string, string, string, string, int>> inputList = new List<Tuple<string, string, string, string, string, string, int>>();

    // Store working input value in a string as well as first input for grid dimensions, Mars Rover X, Y, H values, and instrucitions

    string? gridDimensionsInput;
    string? marsRoverXYHInput;
    string? instructionsInput = "";
    string[] marsRoverXYH = { };
    string[] gridDimensions = { };
    string[] concatgridDimensionsWithMarsRoverXYH = { };
    int counter = 0;

    string? line;
    try
    {
        StreamReader sr = new StreamReader(pathToFile);
        line = sr.ReadLine();

        while (line != null)
        {
            // If first string is null, empty or whitespace break;
            // Else if the first string is not empty, null or whitespace, store the X, Y in gridDimensions
            // Else if second string is null, empty or whitespace break; because (count of the list (1) - 1) mod two == 0
            // Else if any second string after the X, Y bounds have been entered is null, empty or whitespace (add empty string to stack),  ((count of the list - 1) mod two) == 0 must be true
            // Else if any second string after the X, Y bounds have been entered is NOT null, empty or whitespace 
            // Else if string is null, empty or whitespace break;


            if ((counter == 0) && string.IsNullOrWhiteSpace(line))
            {
                // Prompt user again if given wrong instructions
                AnsiConsole.Markup("[red]Incorrect X, Y dimensions given. Please try again:\n[/]");
                QueryUserUntilStopCommand();
            }
            else if ((counter == 0) && !string.IsNullOrWhiteSpace(line))
            {
                // Remove leading and trailing whitespace

                gridDimensionsInput = line.Trim();

                // Split string by spaces
                gridDimensions = gridDimensionsInput.Split(' ');

                // break; if given wrong instructions
                if (gridDimensions.Length != 2) { AnsiConsole.Markup("[red]Incorrect input strings given. Program stopped early.\n[/]"); throw new ArgumentException(); }

                // Else, the 2 Mars Rover grid values X, Y are stored in gridDimensions, to be returned later
                // Increment counter
                // counter++;

            }
            // Input is now any even input after the first, and the input can be null, empty, or whitespace - Consider okay because instructions can be empty
            else if ((counter % 2) == 0 && (counter != 0))
            {
                // Remove leading and trailing whitespace and set instructions equal to "" if null
                line = line ?? "";
                instructionsInput = line.Trim();

                // The instructions are stored in instructionsInput, to be returned later
                // Unless counter == 2, then we must add the first Tuple to the List

                // break; if given wrong instructions
                // if (marsRoverXYH.Length != 3) { AnsiConsole.Markup("[red]Incorrect input strings given. Program stopped early.\n[/]"); throw new ArgumentException(); }

                // First contact gridDimensions[] and marsRoverXYH[] to make array of length 5 
                // concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(marsRoverXYH).ToArray();

                // Then if the array is of correct length, combine the values from the array and instructionsInput string to create a Tuple + add it to the List
                if (concatgridDimensionsWithMarsRoverXYH.Length == 5)
                {
                    int outputOrder = (counter / 2) - 1;

                    inputList.Add(Tuple.Create(concatgridDimensionsWithMarsRoverXYH[0],
                        concatgridDimensionsWithMarsRoverXYH[1],
                        concatgridDimensionsWithMarsRoverXYH[2],
                        concatgridDimensionsWithMarsRoverXYH[3],
                        concatgridDimensionsWithMarsRoverXYH[4],
                        instructionsInput,
                        outputOrder));
                }

            }
            // Consider final end of input string, must be odd and empty, break;
            else if ((counter % 2 == 1) && (counter > 3) && string.IsNullOrWhiteSpace(line))
            {
                return inputList;
            }
            // If any inputs after the first are odd and null - Mars Rover X, Y, H cannot be null, empty, or whitespace - break;
            else if (((counter % 2) == 1) && string.IsNullOrWhiteSpace(line))
            {
                // Create fake input to pass to object, so program does not fail
                string[] spoofXYH = { string.Empty, string.Empty, string.Empty };
                concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(spoofXYH).ToArray();
            }
            // If any inputs after the first are odd and NOT null - Mars Rover X, Y, H cannot be null, empty, or whitespace - prepare X, Y, H for Tuple
            // This will also be when to create and add the Tuple to the list
            else if ((counter % 2 == 1) && !string.IsNullOrWhiteSpace(line))
            {
                // Remove leading and trailing whitespace
                marsRoverXYHInput = line.Trim();

                // Split string by spaces
                marsRoverXYH = marsRoverXYHInput.Split(' ');

                // break; if given wrong instructions
                // if (marsRoverXYH.Length != 3) { AnsiConsole.Markup("[red]Incorrect input strings given. Program stopped early.\n[/]"); throw new ArgumentException(); }

                // Else, the 3 Mars Rover origin values X, Y, H are stored in marsRoverXYH, to be returned now
                // MarsRover(string? xAxisBoundInput, string? yAxisBoundInput, string? xOriginInput, string? yOriginInput, string? directionalHeadingInput, string? turnMoveInstructionsInput)
                // Format to return the tuple in

                // First contact gridDimensions[] and marsRoverXYH[] to make array of length 5 
                concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(marsRoverXYH).ToArray();
            }
            line = sr.ReadLine();
            counter++;
        }

        sr.Close();
        AnsiConsole.Markup("[deeppink3]File successfully read[/]\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message);
        AnsiConsole.Markup("[deeppink3]Please check the path to your file. Note: Include 'C:\\' :)[/]\n");
    }

    return inputList;
}

void ExecuteMarsRoverCaluclationsInParallel(List<Tuple<string, string, string, string, string, string, int>> input)
{
    // Create in-memory cache inorder to cache intial MarsRover to reduce space complexity
    // ObjectCache cache = MemoryCache.Default;

    // Create hashtable to store UUIDS from O(1) lookup - Need a universal unique identifer to store/retrieve the correct object from in-memory cache
    HashSet<string> uuids = new HashSet<string>();

    // Create dicitionary to be returned - Use dictionary because retrieving values via. is close to O(1) meaning constant lookup time
    // Key = Initial MarsRover object, value = MarsRover object after calculation
    ConcurrentBag<MarsRover> marsRoverResults = new ConcurrentBag<MarsRover>();

    // Declare bag
    ConcurrentBag<MarsRover> marsRoverCalculations = new ConcurrentBag<MarsRover>();

    // Declare Task list to be added to the bag
    List<Task> bagAddTasks = new List<Task>();

    // Create all MarsRover objects concurrently from input and add them to bag + Dictionary
    foreach (var tuple in input)
    {
        // Create MarsRover from input
        MarsRover currentRover = new MarsRover(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7);

        // Add rovers to bag
        bagAddTasks.Add(Task.Run(() => marsRoverCalculations.Add(currentRover)));
    }

    // Wait for the MarsRovers to be created
    Task.WaitAll(bagAddTasks.ToArray());

    // Consume the MarsRovers in the bag - Perform calculations on them
    List<Task> bagConsumeTasks = new List<Task>();
    int itemsInBag = 0;

    // Track time
    var watch = new System.Diagnostics.Stopwatch();
    watch.Start();

    // Perform calclations and replace values in dictionary by key - (O)1 complexity
    while (!marsRoverCalculations.IsEmpty)
    {
        bagConsumeTasks.Add(Task.Run(() =>
        {
            MarsRover marsRover;

            if (marsRoverCalculations.TryTake(out marsRover))
            {
                // Be careful to not replace in-memory cache value as a result of multiple threads running concurrently
                // Cache inital MarsRover to avoid creating an additional object here - Last 10ms at most
                // cache.Set($"initalRover{cb.Count}", marsRover, DateTimeOffset.FromUnixTimeMilliseconds(10));

                // Retrieve inital MarsRover value from cache and add <inital, final> values to dictionary
                // explicit cast object type return from in-memory cache to MarsRover is also required
                // Assume null rover is end of file or incorrectly defined rover
                try
                {
                    marsRover.CalculateMomement();
                    marsRoverResults.Add(marsRover);
                    // output based off of the time generated!!!!
                }
                catch (ArgumentNullException ex) 
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
        if (AnsiConsole.Confirm("Output Results?"))
        {
            MarsRover[] valuesArray = marsRoverResults.ToArray();
            long[] lifetimes = new long[valuesArray.Length];
            string[] outputResults = new string[valuesArray.Length];



            int i = 0;

            // Create dictionary from results  for constant time lookup
            Dictionary<long, MarsRover> searchOutput = new Dictionary<long, MarsRover>();
            foreach (MarsRover marsRover in valuesArray)
            {
                long lifetimeStore = marsRover.Lifetime();
                searchOutput.Add(lifetimeStore, marsRover);
                // Put all lifetimes in array
                lifetimes[i] = lifetimeStore;
                i++;
            }

            // Sort lifetimes from min to max
            Array.Sort(lifetimes);

            // Based on sorted liftime array, output Mars Rover results from Dictionary
            for (int j = 0; j < lifetimes.Length; j++)
            {
                // Console.WriteLine(searchOutput[lifetimes[j]].ToString());
                if (searchOutput.TryGetValue(lifetimes[j], out MarsRover value))
                {
                    Console.WriteLine("Item found: " + value);
                }
                else
                {
                    Console.WriteLine("Item not found: " + lifetimes[j]);
                }
            }
        }
    }
    else
    {
        AnsiConsole.Markup("[red]It looks like no valid Mars Rovers were inputted. Please try again.[/]\n");
    }

    // If this line is outputted something went seriously wrong
    MarsRover unexpectedMarsRover;
    if (marsRoverCalculations.TryPeek(out unexpectedMarsRover))
        Console.WriteLine("Found a Mars Rover in the bag when it should be empty");
}

// Main:
string PromptUserForInput()
{
    // Prompt for input type

    var inputTypeSelection = AnsiConsole.Prompt(
        new MultiSelectionPrompt<string>()
            .PageSize(10)
            .Title("You may either [red]manually[/] input data line by line OR input a [green].txt file[/] of lines to read favorite fruits?")
            .MoreChoicesText("[grey](Move up and down to select option)[/]")
            .InstructionsText("[grey](Press spacebar [blue][/] to toggle an option, [green][/] enter to accept)[/]")
            .AddChoiceGroup("Input Type", new[]
            {
            "Manual", "Text file (.txt)"
            }));

    // Validate one type was selected

    var inputType = inputTypeSelection.Count == 1 ? inputTypeSelection[0] : null;
    if (string.IsNullOrWhiteSpace(inputType))
    {
        inputType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Ok, but if you could only choose [green]one input type[/]?")
                .MoreChoicesText("[grey](Move up and down to reveal more types)[/]")
                .AddChoices(inputTypeSelection));
    }

    return inputType;
}

void ProcessInputByType(string inputType)
{
    switch (inputType)
    {
        case "Manual":
            List<Tuple<string, string, string, string, string, string, int>> manualInput = StartManualInput();
            ExecuteMarsRoverCaluclationsInParallel(manualInput);
            break;
        case "Text file (.txt)":
            AnsiConsole.Markup("Note: The final line of the text file [deeppink3]MUST[/] be an empty newline. \n");
            var path = AnsiConsole.Ask<string>("Please input the absolute path to your text file:");
            List<Tuple<string, string, string, string, string, string, int>> textFileInput = StartTextFileInput(path);
            ExecuteMarsRoverCaluclationsInParallel(textFileInput);
            break;
        default:
            throw new NotImplementedException();
    }
}

void ProcessUserInput()
{
    // Prompt for input type

    string inputType = PromptUserForInput();

    // Select input type to process

    ProcessInputByType(inputType);
}

void InitalizeConsoleApp()
{
    var table = new Table().Centered();

    AnsiConsole.Live(table)
        .Start(ctx =>
        {
            table.AddColumn("Welcome to [underline red]Will Schweitzer's[/] [indianred1]Mars[/] Rovers - Coding Test Solution for Dealer[darkorange3]On[/]");
            table.AddRow("[red]Note[/]: A Mars Rover's movement instructions are coded to ONLY respond to the inputs defined in the problem ('L', 'R', and 'M'). Additonally, instructions are expected to be given line by line as defined in the problem task. Therefore, wrongly inputed instructions will be skipped.");
            table.AddEmptyRow();
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 1: A Mars Rover should not stop running if trying to leave the bounds of it's defined 2D plane.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 2: A Mars Rover should not have the ability to go into the negative X, Y axis as defined by the problem.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 3: A Mars Rover should not stop running if trying to leave the bounds of it's defined 2D plane.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 4: A Mars Rover should not be given a 2D plane that exceeds the range of a ulong on either axis.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 5: If a Mars Rover is given incorrect input, the program should not fail to execute.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 6: If a Mars Rover crashes for any reason, all other Mars Rovers should NOT crash.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 7: The maximum amount of Mars Rovers to be inputed should be no larger than 2^63 + 1.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 8: A Mars Rover can only move 1 value on the X or Y axis at a given time.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 9: A Mars Rover can be passed no instructions (Empty line) and be expected to not move.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 10: Correct X and Y dimension MUST be passed, else the program will prompt for different input.");
            ctx.Refresh();
            Thread.Sleep(50);
            table.AddRow("[yellow]Assumption[/] 11: A Mars Rover's movement instructions can only contain valid 'char' objects.");
            ctx.Refresh();
            Thread.Sleep(50);
        });

    if (!AnsiConsole.Confirm("Would you like to begin?"))
    {
        AnsiConsole.MarkupLine("Ok... :(");
    }
    else
    {
        QueryUserUntilStopCommand();
    }
}

bool QueryUserUntilStopCommand()
{
    // Read and process user input
    ProcessUserInput();

    // Confirm if user would like to input another file
    if (!AnsiConsole.Confirm("Would you like input Mars Rover(s) instructions for processing again?"))
    {
        return false;
    }
    else
    {
        // Read and process user input by calling function recursivley
        return QueryUserUntilStopCommand();
    }

}

// Initalize ConsoleApp
InitalizeConsoleApp();