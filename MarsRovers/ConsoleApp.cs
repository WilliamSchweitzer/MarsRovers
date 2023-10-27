// See https://aka.ms/new-console-template for more information

using MarsRovers.src.Features.MarsRover;

Console.WriteLine("~Welcome to Will Schwietzer's Mars Rovers!~");
Console.WriteLine("-------------------------------------------");
MarsRover marsRover1 = new MarsRover("5", "5", "1", "2", "N", "LMLMLMLMM");
marsRover1.CalculateMomement();
Console.WriteLine(marsRover1.Position.ToString());
MarsRover marsRover2 = new MarsRover("5", "5", "3", "3", "E", "MMRMMRMRRM");
marsRover2.CalculateMomement();
Console.WriteLine(marsRover2.Position.ToString());

