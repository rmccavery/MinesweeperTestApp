namespace GameTestAppTests
{
    using System.Drawing;
    using System.Linq;
    using GameTestApp.Business;
    using GameTestApp.Models;
    using NUnit.Framework;
    using Prism.Events;

    public class GameControllerTests
    {
        private IEventAggregator eventAggregator;
        private bool gameOver = false;
        private bool gameWin = false;
        private bool scoreChanged = false;
        private bool gameStarted = false;
        private bool mineExploded = false;

        [SetUp]
        public void Setup()
        {
            this.eventAggregator = new EventAggregator();
            this.gameOver = false;
            this.gameWin = false;
            this.scoreChanged = false;
            this.gameStarted = false;
            this.mineExploded = false;
        }

        [Test]
        public void TestConstruction()
        {
            GameController gameController = new GameController(this.eventAggregator);

            // Game board been created
            Assert.IsNotNull(gameController.GameBoard);
            Assert.IsNotNull(gameController.Player);
            Assert.IsTrue(gameController.Player.Location == new Point(1, 4));
            Assert.IsTrue(gameController.GameBoard.NumberOfMines == 8);
            Assert.IsTrue(gameController.GameBoard.Mines.Count == 8);

            // And the mines are all unique locations
            Assert.IsTrue(gameController.GameBoard.Mines.Select(p => p.Location).Distinct().Count() == 8);
        }


        [Test]
        public void TestMoveHitsMinesAndManagesStateToGameOver()
        {
            GameController gameController = this.CreateFixedGameController();

            // Now need to test that the basic game operation is working
            // An uber test
            // Check player is at 1,4
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(1, 4)) == GameTile.TileState.PlayerOccupied);

            // 1
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);

            // Ensure player has moved
            Assert.IsTrue(gameController.Player.Location == new Point(2, 4));

            // Test the previous position square has changed to visited empty
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(1, 4)) == GameTile.TileState.VisitedEmpty);

            // Player should have hit a mine
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(2, 4)) == GameTile.TileState.MineExploded);

            // Test player has been punished
            Assert.IsTrue(gameController.Player.NumberOfLivesRemaining == 2);

            // And not rewarded
            Assert.IsTrue(gameController.Player.CurrentScore == 0);
            Assert.IsFalse(this.scoreChanged);

            // Check mine exploded event
            Assert.IsTrue(this.mineExploded);

            // 2
            // Move right again
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);

            // Should have hit another bomb
            // Test player has been punished
            Assert.IsTrue(gameController.Player.NumberOfLivesRemaining == 1);

            // And not rewarded
            Assert.IsTrue(gameController.Player.CurrentScore == 0);
            Assert.IsFalse(this.scoreChanged);

            // Test the bomb on previous tile changed
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(2, 4)) == GameTile.TileState.VisitedMine);

            // And current is exploded
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(3, 4)) == GameTile.TileState.MineExploded);

            // 3
            // Move right again
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);

            // Should have hit another bomb
            // Test player has been punished
            Assert.IsTrue(gameController.Player.NumberOfLivesRemaining == 0);

            // And not rewarded
            Assert.IsTrue(gameController.Player.CurrentScore == 0);
            Assert.IsFalse(this.scoreChanged);

            // Test the bomb on previous tile changed
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(3, 4)) == GameTile.TileState.VisitedMine);

            // And current is exploded
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(4, 4)) == GameTile.TileState.MineExploded);

            // Should be game over
            Assert.IsTrue(this.gameOver);

            // Check game not started
            Assert.IsFalse(this.gameStarted);
        }

        [Test]
        public void TestMoveHitsMinesAndManagesStateToGameWin()
        {
            GameController gameController = this.CreateFixedGameController();

            // Now need to test that the basic game operation is working
            // An uber test
            // Check player is at 1,4
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(1, 4)) == GameTile.TileState.PlayerOccupied);

            // 1
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Up));
            gameController.MovePlayer(Player.MoveDirection.Up);

            // Ensure player has moved
            Assert.IsTrue(gameController.Player.Location == new Point(1, 3));

            // Test the previous position square has changed to visited empty
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(1, 4)) == GameTile.TileState.VisitedEmpty);

            // Player should be clear and safe
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(1, 3)) == GameTile.TileState.PlayerOccupied);

            // Test player has not been punished
            Assert.IsTrue(gameController.Player.NumberOfLivesRemaining == 3);

            // And rewarded
            Assert.IsTrue(gameController.Player.CurrentScore == 1);
            Assert.IsTrue(this.scoreChanged);
            this.scoreChanged = false;

            // 2
            // Move right again
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);

            // Should have hit another bomb
            // Test player has not been punished
            Assert.IsTrue(gameController.Player.NumberOfLivesRemaining == 3);

            // And rewarded
            Assert.IsTrue(gameController.Player.CurrentScore == 2);
            Assert.IsTrue(this.scoreChanged);
            this.scoreChanged = false;

            // Test the previous tile changed
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(1, 3)) == GameTile.TileState.VisitedEmpty);

            // And current is occupied
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(2, 3)) == GameTile.TileState.PlayerOccupied);

            // 3
            // Move right again
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);

            // Test player has not been punished
            Assert.IsTrue(gameController.Player.NumberOfLivesRemaining == 3);

            // And rewarded
            Assert.IsTrue(gameController.Player.CurrentScore == 3);
            Assert.IsTrue(this.scoreChanged);

            // Test the bomb on previous tile changed
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(2, 3)) == GameTile.TileState.VisitedEmpty);

            // And current is occupied
            Assert.IsTrue(gameController.GameBoard.GetTileState(new Point(3, 3)) == GameTile.TileState.PlayerOccupied);
            Assert.IsFalse(this.gameOver);

            // Move to end
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);
            Assert.IsTrue(gameController.CanPlayerMove(Player.MoveDirection.Right));
            gameController.MovePlayer(Player.MoveDirection.Right);

            // Check expected score
            Assert.IsTrue(gameController.Player.CurrentScore == 8);

            // And score changed and we have won
            Assert.IsTrue(this.scoreChanged);
            Assert.IsTrue(this.gameWin);

            // Check game started
            Assert.IsFalse(this.gameStarted);
        }

        private GameController CreateFixedGameController()
        {
            GameController gameController = new GameController(this.eventAggregator);

            // Override random mines
            gameController.GameBoard.NumberOfMines = 0;

            // Don't allow random mines, but add fixed location
            gameController.GameBoard.Mines.Add(new Mine { Location = new Point(2, 4) });
            gameController.GameBoard.Mines.Add(new Mine { Location = new Point(3, 4) });
            gameController.GameBoard.Mines.Add(new Mine { Location = new Point(4, 4) });
            gameController.GameBoard.Mines.Add(new Mine { Location = new Point(2, 5) });
            gameController.GameBoard.Mines.Add(new Mine { Location = new Point(3, 5) });
            gameController.GameBoard.Mines.Add(new Mine { Location = new Point(4, 5) });

            this.eventAggregator.GetEvent<GameOverEvent>().Subscribe(() =>
            {
                this.gameOver = true;
            });

            this.eventAggregator.GetEvent<GamePlayerWonEvent>().Subscribe(() =>
            {
                this.gameWin = true;
            });

            this.eventAggregator.GetEvent<GameScoreChangedEvent>().Subscribe(() =>
            {
                this.scoreChanged = true;
            });

            this.eventAggregator.GetEvent<GameStartedEvent>().Subscribe(() =>
            {
                this.gameStarted = true;
            });

            this.eventAggregator.GetEvent<GameMineExplodedEvent>().Subscribe(() =>
            {
                this.mineExploded = true;
            });

            return gameController;
        }
    }
}
