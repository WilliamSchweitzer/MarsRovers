using MarsRovers.src.Core.Enums;
using MarsRovers.src.Core.Enums.Helpers;
using MarsRovers.src.Core.Sctructs;
using MarsRovers.src.Features.MarsRover;
using System.Net.NetworkInformation;

namespace MarsRoversUnitTests;



[TestClass]
public class Test_MarsRover_Movement
{
    // MarsRover(XAxisBound, YAxisBound, XOrigin, YOrigin, DirectionalHeading, TurnMoveInstructions)
    // XAxisBound, YAxisBound, XOrigin, YOrigin are all ulong type
    // DirectionalHeading is char? type - null = 'N',
    // TurnMoveInstructions is string? type - null value = "" or no movement

    [TestMethod]
    public void MarsRover_StartsAtOrigin()
    {
        // Arrange
        MarsRover marsRover1 = new MarsRover("5", "5", "0", "0", "N", "LMLMLMLMM");
        MarsRover marsRover2 = new MarsRover("10", "10", "1", "4", "N", "LMLMLMLMM");

        // Act & Assert
        Assert.AreEqual((ulong)0, marsRover1.Position.X);
        Assert.AreEqual((ulong)0, marsRover1.Position.Y);

        Assert.AreEqual((ulong)1, marsRover2.Position.X);
        Assert.AreEqual((ulong)4, marsRover2.Position.Y);
    }

    [TestMethod]
    public void MarsRover_StartsAtNonNegativeXOrigin()
    {
        // Arrange & Act & Assert
        Assert.ThrowsException<OverflowException>(() => new MarsRover("5", "5", "-1", "0", "N", "LMLMLMLMM"));
    }

    [TestMethod]
    public void MarsRover_StartsAtNonNegativeYOrigin()
    {
        // Arrange & Act & Assert
        Assert.ThrowsException<OverflowException>(() => new MarsRover("10", "10", "1", "-4", "N", "LMLMLMLMM"));
    }

    [TestMethod]
    public void MarsRover_StartsAtNonNegativeXYOrigin()
    {
        // Arrange & Act & Assert
        Assert.ThrowsException<OverflowException>(() => new MarsRover("10", "10", "-1", "-4", "N", "LMLMLMLMM"));
    }

    [TestMethod]
    public void MarsRover_CanMoveInPositiveXDirection()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "E", "M");

        // Act
        marsRover.CalculateMomement();

        // Assert
        Assert.AreEqual(new Position(1, 0, 5, 5, Heading.E), marsRover.Position);
    }

    [TestMethod]
    public void MarsRover_CanMoveInPositiveYDirection()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "N", "M");

        // Act
        // Position is a struct. Therefore, does not require heap allocation. No references to data are stored, just the data itself.
        // Therefore, do not test marsRover.Position.IncrementY(); in MarsRover test class. Test in the Position struct test class.
        marsRover.CalculateMomement();

        // Assert
        Assert.AreEqual(new Position(0, 1, 5, 5, Heading.N), marsRover.Position);
    }

    [TestMethod]
    public void MarsRover_CannotMoveInNegativeXDirection()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "S", "M");

        // Act
        marsRover.Position.Move();

        // Assert
        Assert.AreEqual((ulong)0, marsRover.Position.X);
        Assert.AreEqual((ulong)0, marsRover.Position.Y);
    }

    [TestMethod]
    public void MarsRover_CannotMoveInNegativeYDirection()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "N", "LMLMLMLMM");

        // Act
        marsRover.Position.DecrementY();

        // Assert
        Assert.AreEqual((ulong)0, marsRover.Position.X);
        Assert.AreEqual((ulong)0, marsRover.Position.Y);
    }

    [TestMethod]
    public void MarsRover_XCoordinateDoesNotGoNegativeAfterTurningLeft()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "N", "LM");

        // Act
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'L'));
        marsRover.Position.IncrementX();

        // Assert
        Assert.IsTrue(marsRover.Position.X >= 0);
    }

    [TestMethod]
    public void MarsRover_XCoordinateDoesNotGoNegativeAfterTurningRight()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "S", "RM");

        // Act
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'R'));
        marsRover.Position.DecrementX();

        // Assert
        Assert.IsTrue(marsRover.Position.X >= 0);
    }

    [TestMethod]
    public void MarsRover_YCoordinateDoesNotGoNegativeAfterTurningLeft()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "W", "LM");

        // Act
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'L'));
        marsRover.Position.DecrementY();

        // Assert
        Assert.IsTrue(marsRover.Position.Y >= 0);
    }

    [TestMethod]
    public void MarsRover_YCoordinateDoesNotGoNegativeAfterTurningRight()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "E", "RM");

        // Act
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'R'));
        marsRover.Position.Move();

        // Assert
        Assert.IsTrue(marsRover.Position.Y >= 0);
    }

    [TestMethod]
    public void MarsRover_XCoordinateDoesNotGoNegativeAfterMoving()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "W", "M");

        // Act
        marsRover.Position.Move();

        // Assert
        Assert.IsTrue(marsRover.Position.X >= 0);
    }

    [TestMethod]
    public void MarsRover_YCoordinateDoesNotGoNegativeAfterMoving()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "0", "0", "S", "M");

        // Act
        marsRover.Position.Move();

        // Assert
        Assert.IsTrue(marsRover.Position.Y >= 0);
    }

    [TestMethod]
    public void MarsRover_XYCoordinatesDoNotGoNegativeAfterComplexMovement()
    {
        // Arrange
        MarsRover marsRover = new MarsRover("5", "5", "1", "2", "N", "LMLMLMRMM");

        // Act
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'L'));
        marsRover.Position.Move();
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'L'));
        marsRover.Position.Move();
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'L'));
        marsRover.Position.Move();
        marsRover.Position.SetHeading(ChangeHeading.ChangeDirection(marsRover.Position.Heading, 'R'));
        marsRover.Position.Move();
        marsRover.Position.Move();


        // Assert
        Assert.IsTrue(marsRover.Position.X >= 0);
        Assert.IsTrue(marsRover.Position.Y >= 0);
    }

    [TestMethod]
    public void MarsRover_CalculateMovementReturnsCorrectPosition()
    {
        // Arrange
        MarsRover marsRover1 = new MarsRover("5", "5", "1", "2", "N", "LMLMLMLMM");
        MarsRover marsRover2 = new MarsRover("5", "5", "3", "3", "E", "MMRMMRMRRM");

        // Act
        marsRover1.CalculateMomement();
        marsRover2.CalculateMomement();

        // Assert
        Assert.AreEqual(new Position(1, 3, 5, 5, Heading.N), marsRover1.Position);

        Assert.AreEqual(new Position(5, 1, 5, 5, Heading.E), marsRover2.Position);
    }
}