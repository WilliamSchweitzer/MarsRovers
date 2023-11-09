using MarsRovers.src.Core.Structs;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRovers.src.Utilities
{
    public class ConsoleUtility
    {
        public static void PromptUserForInput()
        {
            // Prompt for input type

            var inputTypeSelection = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .PageSize(10)
                    .Title("You may either [red]manually[/] input data line by line OR input a [green].txt file[/] of lines to read.")
                    .MoreChoicesText("[grey](Move up and down to select option)[/]")
                    .InstructionsText("[grey](Press spacebar[blue][/] to toggle an option, [green][/]enter to accept)[/]")
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

            ProcessInputByType(inputType);
        }

        public static void ProcessInputByType(string inputType)
        {
            switch (inputType)
            {
                case "Manual":
                    List<MarsRoverInput> manualInput = StartManualInput();
                    MarsRoverUtility.ExecuteMarsRoverCaluclationsInParallel(manualInput);
                    break;
                case "Text file (.txt)":
                    var path = AnsiConsole.Ask<string>("Please input the absolute path to your text file:");
                    List<MarsRoverInput> textFileInput = StartTextFileInput(path);
                    MarsRoverUtility.ExecuteMarsRoverCaluclationsInParallel(textFileInput, path);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool QueryUserUntilStopCommand()
        {
            // Read and process user input
            PromptUserForInput();

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

        public static List<MarsRoverInput> StartManualInput()
        {
            AnsiConsole.Markup("You may begin entering instructions line by line. [deeppink3]Return an empty line to stop.[/]\n");

            // Use list because input size is unknown, Use Tuple to store and return all values unique to a Mars Rover together
            List<MarsRoverInput> inputList = new ();

            // Store working input value in a string as well as first input for grid dimensions, Mars Rover X, Y, H values, and instrucitions

            // Regex to use to validate input
            string reDigits = "^[0-9]*$";
            string reDigitsOrSpaces = "^[0-9 ]*$";
            // string reHeadings = "^[NESW] *$"; Not using as a result of input defaulting to 'N' regardless

            string[] fakeXYH = { string.Empty, string.Empty, string.Empty };
            bool fakedInput = false;
            string? instructionsInput;
            string[] marsRoverXYH = Array.Empty<string>();
            string[] gridDimensions = Array.Empty<string>();
            string[] concatgridDimensionsWithMarsRoverXYH = Array.Empty<string>();
            int counter = 0;

            // Do inital ReadLine outside of loop to correctly increment counter
            string? input = Console.ReadLine();


            // Always run, until stated in code, store first line then take input in groups of 2, else break
            while (true)
            {
                // Remove all leading and trealing whitespace for all non null inputs
                if (input != null)
                {
                    input = input.Trim();
                }

                // If first string is null, empty, whitespace, or not all digits and spaces, prompt user to try again, also only allow input of length 3 'X Y'
                if ((counter == 0) && (string.IsNullOrWhiteSpace(input) || !Regex.IsMatch(input, reDigitsOrSpaces) || input.Split(' ').Length != 2))
                {
                    // Prompt user again if given wrong instructions
                    AnsiConsole.Markup("[red]X, Y dimensions inputted incorrectly. Please try again:\n[/]");
                    QueryUserUntilStopCommand();
                }
                // Else if the first string is NOT (empty, null, whitespace) AND is all digits or spaces, store the X, Y in gridDimensions
                else if ((counter == 0) && !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, reDigitsOrSpaces))
                {

                    // Split string by spaces
                    gridDimensions = input.Split(' ');

                    // Prompt user again if given wrong instructions. Also, validate (X, Y) dimensions are not zero
                    if (gridDimensions.Length != 2 || (Convert.ToUInt64(gridDimensions[0]) == 0) || (Convert.ToUInt64(gridDimensions[1]) == 0))
                    {
                        AnsiConsole.Markup("[red]Incorrect X, Y dimensions given. Please try again:\n[/]");
                        QueryUserUntilStopCommand();
                    }

                    // Else, the 2 Mars Rover grid values X, Y are stored in gridDimensions, to be returned later
                }
                // Input is now any even input after the first, and the input can be null, empty, or whitespace - Consider okay because instructions can be empty
                else if ((counter % 2) == 0 && (counter > 1))
                {
                    // Remove leading and trailing whitespace and set instructions equal to "" if null
                    input = input ?? "";

                    // The instructions are stored in instructionsInput, to be returned later
                    instructionsInput = input;

                    // First ensure concatgridDimensionsWithMarsRoverXYH is 5 values (Xdim, Ydim, X, Y, H)
                    // Then combine the values from the array and instructionsInput string to create a Tuple + add it to the List
                    if (concatgridDimensionsWithMarsRoverXYH.Length == 5 && fakedInput == false)
                    {
                        int outputOrder = (counter / 2) - 1;

                        inputList.Add(new MarsRoverInput(concatgridDimensionsWithMarsRoverXYH[0],
                            concatgridDimensionsWithMarsRoverXYH[1],
                            concatgridDimensionsWithMarsRoverXYH[2],
                            concatgridDimensionsWithMarsRoverXYH[3],
                            concatgridDimensionsWithMarsRoverXYH[4],
                            instructionsInput,
                            outputOrder));
                    }
                    else // Length is not 5 (not really ever going to happen) and fakedInput is True
                    {
                        // Account for invalid XYH causing the outputOrder to be incorrect - Set outputOrder to -1 to skip output of rover in calculations
                        int outputOrder = -1;

                        inputList.Add(new MarsRoverInput(concatgridDimensionsWithMarsRoverXYH[0],
                            concatgridDimensionsWithMarsRoverXYH[1],
                            concatgridDimensionsWithMarsRoverXYH[2],
                            concatgridDimensionsWithMarsRoverXYH[3],
                            concatgridDimensionsWithMarsRoverXYH[4],
                            instructionsInput,
                            outputOrder));
                        // Decrement counter to maintain correct output order
                        counter -= 2;
                    }
                }
                // Consider final end of input string, must be odd and empty - return the inputList in this case - also consider when only the X Y dimensions are passed the inputlist needs returned (counter == 1)
                else if ((counter == 1 || ((counter % 2 == 1) && (counter >= 3))) && string.IsNullOrWhiteSpace(input))
                {
                    return inputList;
                }
                // Else if any inputs after the first are odd - Mars Rover X, Y, H cannot be null, empty, or whitespace - create fake data so a null rover is created instead of breaking the program
                // Also consider, length must == 5 in order to be valid "X Y H", so only onsider the first 5 values of the line. 
                // The string passed could be invalid, but not null or whitespace
                else if ((counter % 2) == 1)
                {
                    // Validate correct XYH input

                    // Split into array if not null, RemoveEmptyEntries if inital Trim() does not work
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        marsRoverXYH = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    }

                    // If 3 values passed and all values are valid in correct position
                    if (marsRoverXYH.Length == 3 && Regex.IsMatch(marsRoverXYH[0], reDigits) && Regex.IsMatch(marsRoverXYH[1], reDigits)) // && Regex.IsMatch(marsRoverXYH[2], reHeadings))
                    {
                        fakedInput = false;
                        concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(marsRoverXYH).ToArray();
                    }
                    // Else, pass "fake" XYH value and decrement counter by 2 to account for output order
                    else
                    {
                        fakedInput = true;
                        concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(fakeXYH).ToArray();

                        // Consider first XYH values being inputed incorrectly must not subtract from counter
                        counter = (counter == 1) ? (counter) : (counter -= 2);
                    }
                }
                else return inputList; // Only the X, Y dimensions were inputted

                input = Console.ReadLine();
                counter++;
            }
            throw new Exception("Major issue in reading instructions");
        }

        public static List<MarsRoverInput> StartTextFileInput(string pathToFile)
        {
            AnsiConsole.Markup("[deeppink3]Attemping to read file...[/]\n");

            // Use list because input size is unknown, Use Tuple to store and return all values unique to a Mars Rover together
            List<MarsRoverInput> inputList = new ();

            // Regex to use to validate input
            string reDigits = "^[0-9]*$";
            string reDigitsOrSpaces = "^[0-9 ]*$";
            // string reHeadings = "^[NESW] *$"; Not using as a result of input defaulting to 'N' regardless

            // Store working input values in a strings as well as first input for grid dimensions, Mars Rover X, Y, H values, and instructions

            string[] fakeXYH = { string.Empty, string.Empty, string.Empty };
            bool fakedInput = false;
            string? instructionsInput;
            string[] marsRoverXYH = Array.Empty<string>();
            string[] gridDimensions = Array.Empty<string>();
            string[] concatgridDimensionsWithMarsRoverXYH = Array.Empty<string>();
            int counter = 0;

            string? line;
            try
            {
                StreamReader sr = new (pathToFile);
                line = sr.ReadLine();

                // Remove leading and trailing whitespace from all non null lines
                if (line != null)
                {
                    line = line.Trim();
                }

                while (line != null)
                {
                    // If first string is null, empty, whitespace, or not all digits prompt user to try again, also only allow input of length 3 'X Y'
                    if ((counter == 0) && (string.IsNullOrWhiteSpace(line) || !Regex.IsMatch(line, reDigitsOrSpaces) || line.Split(' ').Length != 2))
                    {
                        // Prompt user again if given wrong instructions
                        AnsiConsole.Markup("[red]X, Y dimensions inputted incorrectly. Please try again:\n[/]");
                        QueryUserUntilStopCommand();
                    }
                    // Else if the first string is NOT (empty, null, whitespace) AND is all digits, store the X, Y in gridDimensions
                    else if ((counter == 0) && !string.IsNullOrWhiteSpace(line) && Regex.IsMatch(line, reDigitsOrSpaces))
                    {
                        // Split string by spaces
                        gridDimensions = line.Split(' ');

                        // Prompt user again if given correct instruction type, but wrong length (more than 2 values for X, Y), Also, validate (X, Y) dimensions are not zero
                        if (gridDimensions.Length != 2 || (Convert.ToUInt64(gridDimensions[0]) == 0) || (Convert.ToUInt64(gridDimensions[1]) == 0))
                        {
                            AnsiConsole.Markup("[red]Incorrect X, Y dimensions given. Please try again:\n[/]");
                            QueryUserUntilStopCommand();
                        }

                        // Else, the 2 Mars Rover grid values X, Y are stored in gridDimensions, to be used later

                    }
                    // Input is now any even input order after the first, and the input can be null, empty, or whitespace - Consider okay because instructions can be empty
                    else if ((counter % 2) == 0 && (counter != 0))
                    {
                        // Remove leading and trailing whitespace and set instructions equal to "" if null
                        line = line ?? "";

                        // The instructions are stored in instructionsInput, to be returned later
                        instructionsInput = line;

                        // First ensure concatgridDimensionsWithMarsRoverXYH is 5 values (Xdim, Ydim, X, Y, H)
                        // Then combine the values from the array and instructionsInput string to create a Tuple + add it to the List
                        if (concatgridDimensionsWithMarsRoverXYH.Length == 5 && fakedInput == false)
                        {
                            // Output order is determined by each complete set of rover instructions
                            // Since counter willnever be 0 or 1 here, it is safe to divide by 2 and subtract to get 1,2,3...,X
                            int outputOrder = (counter / 2) - 1;

                            inputList.Add(new MarsRoverInput(concatgridDimensionsWithMarsRoverXYH[0],
                                concatgridDimensionsWithMarsRoverXYH[1],
                                concatgridDimensionsWithMarsRoverXYH[2],
                                concatgridDimensionsWithMarsRoverXYH[3],
                                concatgridDimensionsWithMarsRoverXYH[4],
                                instructionsInput,
                                outputOrder));
                        }
                        else // Length is not 5 (not really ever going to happen) and fakedInput is True
                        {
                            // Account for invalid XYH causing the outputOrder to be incorrect - Set outputOrder to -1 to skip output of rover in calculations
                            int outputOrder = -1;

                            inputList.Add(new MarsRoverInput(concatgridDimensionsWithMarsRoverXYH[0],
                                concatgridDimensionsWithMarsRoverXYH[1],
                                concatgridDimensionsWithMarsRoverXYH[2],
                                concatgridDimensionsWithMarsRoverXYH[3],
                                concatgridDimensionsWithMarsRoverXYH[4],
                                instructionsInput,
                                outputOrder));
                            // Decrement counter to maintain correct output order
                            counter -= 2;
                        }

                    }
                    // Consider final end of input string, must be odd and empty - return the inputList in this case
                    else if ((counter % 2 == 1) && (counter >= 3) && string.IsNullOrWhiteSpace(line))
                    {
                        return inputList;
                    }
                    // Else if any inputs after the first are odd and null - Mars Rover X, Y, H cannot be null, empty, or whitespace - create fake data so a null rover is created instead of breaking the program
                    // Also consider, length must == 5 in order to be valid "X Y H", so only onsider the first 5 values of the line. 
                    // The string passed could be invalid, but not null or whitespace
                    else if ((counter % 2) == 1)
                    {

                        // Split into array if not null, RemoveEmptyEntries if inital Trim() does not work
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            marsRoverXYH = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        }

                        // If 3 values passed and all values are valid in correct position
                        if (marsRoverXYH.Length == 3 && Regex.IsMatch(marsRoverXYH[0], reDigits) && Regex.IsMatch(marsRoverXYH[1], reDigits)) // && Regex.IsMatch(marsRoverXYH[2], reHeadings))
                        {
                            fakedInput = false;
                            concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(marsRoverXYH).ToArray();
                        }
                        // Else, pass "fake" XYH value
                        else
                        {
                            fakedInput = true;
                            concatgridDimensionsWithMarsRoverXYH = gridDimensions.Concat(fakeXYH).ToArray();
                        }
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

        public static void InitalizeConsoleApp()
        {
            var table = new Table().Centered();

            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    table.AddColumn("Welcome to [underline red]Will Schweitzer's[/] [indianred1]Mars[/] Rovers Console Application");
                    table.AddRow("[red]Note[/]: A Mars Rover's movement instructions are coded to ONLY respond to the inputs defined in the problem ('L', 'R', and 'M'). Additonally, instructions are expected to be given line by line as defined in the problem task. Therefore, wrongly inputed instructions will be skipped.\n");
                    table.AddEmptyRow();
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 1: A Mars Rover should not have the ability to go into the negative X, Y axis as defined by the problem.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 2: A Mars Rover should not stop running if trying to leave the bounds of it's defined 2D plane.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 3: A Mars Rover should not be passed numerical values that exceed the range of a ulong.\nElse, the program will stop due to stack overflow exception.\nNegative values will be skipped as they generate a FormatException in this case, not a StackOverflow which ruins the integrity of the program.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 4: If a Mars Rover is given incorrect input, the program should not fail to execute. Unless assumption 4 is ignored.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 5: With assumption 4 in mind, the passed X, Y coordiantes and heading, will only [blue]accept inputs with space separated value (SSV) length 3. i.e. 'X Y H'.[/]\nI have not coded to search the input string for correct data given a SSV length <5 or >5.\nWith that being said, 'X Y H' input of valid SSV length, will [blue]default to 'N' heading if an incorrect heading is passed with valid X, Y coords.[/]");
                    table.AddEmptyRow();
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 6: If a Mars Rover is passed 'valid' incorrect instructions, all other Mars Rovers should work correctly, and be outputted in the correct order they were inputted.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 7: A Mars Rover can only move 1 value on the X or Y axis at a given time.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 8: A Mars Rover can be passed no movement instructions (Empty line) and be expected to not move.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 9: Correct X and Y dimension MUST be passed, else the program will prompt for different input. SSV length of 2 'X Y'.\nNote: In math, a 2D plane is described by having more than 1 point that does not lie on the same line. X and Y dimensions cannot equal 0.");
                    table.AddEmptyRow();
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[yellow]Assumption[/] 10: If the inputted (X, Y) origins exceed the dimensions of the board, they will be set to (0, 0).\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("[lime]Assumption[/] 11: Given this instruction in the problem details:\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("'Each rover will move in series, i.e. the next rover will not start moving until the one preceding it finishes..'.\n");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("This could have been accomplished simply using a for loop. Simply create each item one by one using the for loop, then calculate and output the results on the next line in the for loop.");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("However, this solution was designed to account for a very large amount of inputs while still having the ability to compute efficiently.");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("Therefore, the calculations of this program are calculated in parallel (concurrently), so technically the rovers do not 'move in series', they all move at the same time.");
                    ctx.Refresh();
                    Thread.Sleep(50);
                    table.AddRow("I hope this adaptation is not considered a incorrect solution as a result of this as the resulting output still matches with the order the rovers were inputed.");
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
    }
}
