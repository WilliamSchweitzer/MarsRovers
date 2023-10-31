# MarsRovers

:sparkles: *Try the input text file 211200roversLongInstructions.txt to see the full capabilities of this console app* :sparkles:

My PC was able to compute the new position of all 211200 "rovers" in 1041ms.

![1041ms for 211200 rovers](211200roversScreenshot.jpg?raw=true)

## TODO
- [x] Implement input from users via ConsoleApp
- [x] Finish ConsoleApp to allow for X "MarsRovers"
- [x] Final pass of code for typos and clarity
- [x] Ensure all correct results from manual and file input
- [x] Remove all hardcore
- [x] Create a struct 'MarsRoverInput' to store input from user instead of using Tuple<string, string, string, string, string, string, int> 
- [x] Change the return values of the input functions in the ConsoleUtility class to be of type 'MarsRoverInput' 
- [x] Change the parameters of the caluclation function in the MarsRovereUtility class to be of type 'MarsRoverInput'
### Extra credit :smiley:
- [ ] Finish all possible unit tests
- [x] Stress test console app with very large inputs greater than 15,000 Mars Rovers
- [ ] Misc documentation
