namespace GameTestApp.Business
{
    using System.Drawing;
    using System.Linq;
    using GameTestApp.Models;
    using Prism.Events;

    /// <summary>
    /// Class for representing the logic of the game
    /// </summary>
    public class GameController
    {
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the GameController class
        /// </summary>
        public GameController(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            // Initialize the game board
            this.GameBoard = new GameBoard
            {
                NumberOfMines = 8
            };

            // And player object
            this.Player = new Player
            {
                Location = GameBoard.StartingLocation
            };
        }

        /// <summary>
        /// Gets or sets the games board object
        /// </summary>
        public GameBoard GameBoard { get; }

        /// <summary>
        /// Gets the game player object
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Handles the player move command
        /// </summary>
        /// <param name="moveDirection">The direction in which to move the player</param>
        public void MovePlayer(Player.MoveDirection moveDirection)
        {
            Point previousLocation = this.Player.Location;

            // Test we can move the player
            if (true == this.Player.MovePlayer(moveDirection))
            {
                // Yes player has moved
                // Change the state of the tile we are vacating

                // Is there a mine at this location
                Mine mineExists = this.GameBoard
                                      .Mines
                                      .FirstOrDefault(p => p.Location.Equals(previousLocation));

                if (null != mineExists)
                {
                    this.GameBoard.SetStateOnTile(previousLocation, GameTile.TileState.VisitedMine);
                }
                else
                {
                    this.GameBoard.SetStateOnTile(previousLocation, GameTile.TileState.VisitedEmpty);
                }

                // Have they stepped on a mine in the new location?
                Mine mineFound = this.GameBoard
                                     .Mines
                                     .FirstOrDefault(p => p.Location.Equals(this.Player.Location)
                                                          && false == p.HasExploded);
                if (null != mineFound)
                {
                    // Boom!
                    mineFound.HasExploded = true;

                    this.Player.NumberOfLivesRemaining--;

                    this.eventAggregator
                        .GetEvent<GameMineExplodedEvent>()
                        .Publish();

                    // Need to change state of occupied tile
                    this.GameBoard.SetStateOnTile(this.Player.Location, GameTile.TileState.MineExploded);

                    // Have they pushed their luck too much?
                    if (this.Player.NumberOfLivesRemaining <= 0)
                    {
                        // Game over
                        this.eventAggregator
                            .GetEvent<GameOverEvent>()
                            .Publish();
                    }
                }
                else
                {
                    // They got away with it!
                    // Increment their score
                    // TODO - Dont increment score when they have already visited the tile
                    this.Player.CurrentScore++;

                    // Need to change state of occupied tile
                    this.GameBoard.SetStateOnTile(this.Player.Location, GameTile.TileState.PlayerOccupied);

                    // Notify game score changed
                    this.eventAggregator
                        .GetEvent<GameScoreChangedEvent>()
                        .Publish();

                    // Now, just finally check, did they get to the end (ie column 8)
                    if (this.Player.Location.X == GameBoard.BoardWidth)
                    {
                        // They got to the other side
                        // Notify player won
                        this.eventAggregator
                            .GetEvent<GamePlayerWonEvent>()
                            .Publish();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the player can move in the specified direction
        /// </summary>
        /// <param name="moveDirection">The direction in which to move</param>
        /// <returns>A value indicating whether the player can move</returns>
        public bool CanPlayerMove(Player.MoveDirection moveDirection)
        {
            return this.Player.CanPlayerMove(moveDirection);
        }

        /// <summary>
        /// Resets the game back to starting defaults
        /// </summary>
        public void ResetGame()
        {
            this.Player.Location = GameBoard.StartingLocation;
            this.Player.Respawn();
            this.GameBoard.ResetBoard();

            // Need to change state of occupied tile
            this.GameBoard.SetStateOnTile(this.Player.Location, GameTile.TileState.PlayerOccupied);

            // Notify game started
            this.eventAggregator
                .GetEvent<GameStartedEvent>()
                .Publish();
        }
    }
}