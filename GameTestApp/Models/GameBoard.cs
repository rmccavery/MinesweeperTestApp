namespace GameTestApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Prism.Mvvm;

    /// <summary>
    /// Class for representing the game board
    /// </summary>
    public class GameBoard : BindableBase
    {
        // RJM - May want to make these properties / dynamic
        public const int BoardWidth = 8;
        public const int BoardHeight = 8;

        public static Point StartingLocation = new Point(1, 4);
        private int numberOfMines = 10;

        /// <summary>
        /// Initializes a new instance GameBoard class
        /// </summary>
        public GameBoard()
        {
            this.Tiles = new List<GameTile>();
            this.Mines = new List<Mine>();

            this.ResetBoard();
        }

        /// <summary>
        /// Gets the collection of GameTiles that make up the board
        /// </summary>
        public List<GameTile> Tiles { get; }

        /// <summary>
        /// Gets the collection of mines located on the game board
        /// </summary>
        public List<Mine> Mines { get; }

        /// <summary>
        /// Gets or sets the number of mines
        /// </summary>
        public int NumberOfMines
        {
            get { return this.numberOfMines; }
            set
            {
                bool resetRequired = value != this.numberOfMines;

                this.SetProperty(ref this.numberOfMines, value);

                if (true == resetRequired)
                {
                    // Need to rebuild the mines collection
                    this.ResetBoard();
                }
            }
        }

        /// <summary>
        /// Resets / creates the game board
        /// </summary>
        public void ResetBoard()
        {
            if (false == this.Tiles.Any())
            {
                // Create the board tiles with location
                for (int y = 1; y <= BoardHeight; y++)
                {
                    for (int x = 1; x <= BoardWidth; x++)
                    {
                        this.Tiles.Add(new GameTile
                        {
                            Location = new Point(x, y),
                            State = GameTile.TileState.Unvisited,
                        });
                    }
                }
            }
            else
            {
                // Just reset all tiles to unvisited
                // If board size changed we would have to rebuild collection
                this.Tiles.ForEach(p => p.State = GameTile.TileState.Unvisited);
            }

            this.Mines.Clear();

            // Now create the collection of mines
            for (int i = 0; i < this.NumberOfMines; i++)
            {
                this.Mines.Add(new Mine
                {
                    HasExploded = false,
                    Location = this.GetRandomLocation(),
                });
            }

            // Show the player start position
            this.SetStateOnTile(StartingLocation, GameTile.TileState.PlayerOccupied);
        }

        /// <summary>
        /// Sets the state of a tile
        /// </summary>
        /// <param name="tileLocation">The location of the tile</param>
        /// <param name="tileState">The tile state to set</param>
        public bool SetStateOnTile(Point tileLocation, GameTile.TileState tileState)
        {
            GameTile tile = this.Tiles
                                .FirstOrDefault(p => p.Location.Equals(tileLocation));

            if (null != tile)
            {
                tile.State = tileState;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the state of a tile at the specified location
        /// </summary>
        /// <param name="tileLocation">The tile location</param>
        /// <returns>The tile state</returns>
        public GameTile.TileState GetTileState(Point tileLocation)
        {
            GameTile tile = this.Tiles
                                .FirstOrDefault(p => p.Location.Equals(tileLocation));

            if (null != tile)
            {
                return tile.State;
            }

            return GameTile.TileState.Unvisited;
        }

        /// <summary>
        /// Gets a random unique mine location
        /// </summary>
        /// <returns></returns>
        private Point GetRandomLocation()
        {
            Random rnd = new Random();

            while (true)
            {
                Point pointToTest = new Point(rnd.Next(1, 8), rnd.Next(1, 8)); // creates a number between 1 and 8

                // Must not already have this space occupied or the user starting place
                if (false == pointToTest.Equals(StartingLocation)
                    && false == this.Mines.Any(m => m.Location.Equals(pointToTest)))
                {
                    return pointToTest;
                }
            }
        }
    }
}