namespace GameTestAppTests
{
    using System.Drawing;
    using GameTestApp.Models;
    using NUnit.Framework;
    using System.Linq;

    public class GameBoardTests
    {
        private GameBoard fixedBoard;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstruction()
        {
            GameBoard gameBoard = new GameBoard();

            Assert.IsNotNull(gameBoard.Mines);
            Assert.IsNotNull(gameBoard.Tiles);
            Assert.IsTrue(gameBoard.NumberOfMines == 10);

            // And they are all unique locations
            Assert.IsTrue(gameBoard.Mines.Select(p => p.Location).Distinct().Count() == 10);
        }

        [Test]
        public void TestSettingNumberOfMinesResetsBoard()
        {
            GameBoard gameBoard = new GameBoard
            {
                NumberOfMines = 12
            };

            // There are 12 mines in the collection
            Assert.IsTrue(gameBoard.Mines.Count == 12);

            // And they are all unique locations
            Assert.IsTrue(gameBoard.Mines.Select(p => p.Location).Distinct().Count() == 12);
        }

        [Test]
        public void TestDefaultTileStates()
        {
            GameBoard gameBoard = new GameBoard
            {
                NumberOfMines = 10
            };

            // Test the tiles are all set to default
            Assert.IsTrue(gameBoard.Tiles.Any(p => p.State != GameTile.TileState.Unvisited));

            // And that the player occupied square is set 
            Assert.IsNotNull(gameBoard.Tiles.FirstOrDefault(p => p.State == GameTile.TileState.PlayerOccupied));

            // And that the default location is as we expect
            Assert.IsTrue(gameBoard.Tiles.FirstOrDefault(p => p.State == GameTile.TileState.PlayerOccupied).Location.Equals(new Point(1, 4)));

            // And they are all unique locations
            Assert.IsTrue(gameBoard.Tiles.Select(p => p.Location).Distinct().Count() == 64);
        }

        [Test]
        public void TestSetStateOnTileInvalidLocation()
        {
            GameBoard gameBoard = new GameBoard
            {
                NumberOfMines = 10
            };

            // Could throw an exception from this method, but instead invalid requests are ignored
            Assert.IsFalse(gameBoard.SetStateOnTile(new Point(0, 0), GameTile.TileState.MineExploded));

            // Test the tiles are all set to default and nothing has changed
            // And that the player occupied square is set 
            Assert.IsNotNull(gameBoard.Tiles.FirstOrDefault(p => p.State == GameTile.TileState.PlayerOccupied));

            // And that the default location is as we expect
            Assert.IsTrue(gameBoard.Tiles.FirstOrDefault(p => p.State == GameTile.TileState.PlayerOccupied).Location.Equals(new Point(1, 4)));
        }

        [Test]
        public void TestSetStateOnTileValidLocation()
        {
            GameBoard gameBoard = new GameBoard
            {
                NumberOfMines = 10
            };

            Point location = new Point(2, 2);

            // Ensure setting tile state valid
            Assert.IsTrue(gameBoard.SetStateOnTile(location, GameTile.TileState.MineExploded));

            // Test at least one tile has changed state
            Assert.IsTrue(gameBoard.Tiles.Any(p => p.State == GameTile.TileState.MineExploded));

            // Test the tile state is as we have just set
            Assert.IsTrue(gameBoard.GetTileState(location) == GameTile.TileState.MineExploded);
        }
    }
}
