namespace GameTestApp.Models
{
    using System.Drawing;
    using Prism.Mvvm;

    /// <summary>
    /// Class to represent a tile on the game board
    /// </summary>
    public class GameTile : BindableBase, IGameObjectLocation
    {
        /// <summary>
        /// The state of the game tile
        /// </summary>
        public enum TileState
        {
            Unvisited,
            PlayerOccupied,
            VisitedEmpty,
            MineExploded,
            VisitedMine,
        }

        private Point location = Point.Empty;
        private TileState state = TileState.Unvisited;

        /// <summary>
        /// Gets or sets the GameTime state
        /// </summary>
        public TileState State
        {
            get { return this.state; }
            set { this.SetProperty(ref this.state, value); }
        }

        /// <summary>
        /// Gets or sets the GameTile point location
        /// </summary>
        public Point Location
        {
            get { return this.location; }
            set { this.SetProperty(ref this.location, value); }
        }
    }
}