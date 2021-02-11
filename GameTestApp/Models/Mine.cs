namespace GameTestApp.Models
{
    using System.Drawing;
    using Prism.Mvvm;

    /// <summary>
    /// Class to represent a mine on the game board
    /// </summary>
    public class Mine : BindableBase, IGameObjectLocation
    {
        private bool hasExploded = false;
        private Point location = Point.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this mine instance has exploded
        /// </summary>
        public bool HasExploded
        {
            get { return this.hasExploded; }
            set { this.SetProperty(ref this.hasExploded, value); }
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