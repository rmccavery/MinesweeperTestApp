namespace GameTestApp.Models
{
    using System.Drawing;

    /// <summary>
    /// Provides an interface for game objects
    /// </summary>
    public interface IGameObjectLocation
    {
        /// <summary>
        /// Gets or sets the game object location
        /// </summary>
        Point Location { get; set; }
    }
}