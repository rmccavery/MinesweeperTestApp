namespace GameTestApp.Models
{
    using System.Drawing;
    using Prism.Mvvm;

    /// <summary>
    /// Class for representing the game player on the game board
    /// </summary>
    public class Player : BindableBase, IGameObjectLocation
    {
        /// <summary>
        /// Movement direction
        /// </summary>
        public enum MoveDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        public const int DefaultNumLives = 3;
        private int currentScore = 0;

        private Point location = Point.Empty;
        private int numberOfLivesRemaining = DefaultNumLives;

        /// <summary>
        /// Gets or sets the number of lives remaining
        /// </summary>
        public int NumberOfLivesRemaining
        {
            get { return this.numberOfLivesRemaining; }
            set { this.SetProperty(ref this.numberOfLivesRemaining, value); }
        }

        /// <summary>
        /// Gets or sets the current score
        /// </summary>
        public int CurrentScore
        {
            get { return this.currentScore; }
            set { this.SetProperty(ref this.currentScore, value); }
        }

        /// <summary>
        /// Gets or sets the GameTile point location
        /// </summary>
        public Point Location
        {
            get { return this.location; }
            set { this.SetProperty(ref this.location, value); }
        }

        /// <summary>
        /// Determines whether the player can move in the specified direction
        /// </summary>
        /// <param name="moveDirection">The direction to move</param>
        /// <returns>A value indicating whether the player can move</returns>
        public bool CanPlayerMove(MoveDirection moveDirection)
        {
            if (this.Location.Equals(Point.Empty))
            {
                // Location not set
                return false;
            }

            // Move the player
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    {
                        // Can we move left
                        return this.Location.X > 1;
                    }

                case MoveDirection.Right:
                    {
                        // Can we move right
                        return this.Location.X < GameBoard.BoardWidth;
                    }

                case MoveDirection.Up:
                    {
                        // Move up
                        return this.Location.Y > 1;
                    }

                case MoveDirection.Down:
                    {
                        // Can we move down
                        return this.Location.Y < GameBoard.BoardHeight;
                    }
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Moves the player in the specified direction
        /// </summary>
        /// <param name="moveDirection">The direction to move</param>
        public bool MovePlayer(MoveDirection moveDirection)
        {
            if (false == this.CanPlayerMove(moveDirection))
            {
                return false;
            }

            // Move the player
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    {
                        // Move left
                        this.Location = new Point(this.Location.X - 1, this.Location.Y);
                    }
                    break;

                case MoveDirection.Right:
                    {
                        // Move right
                        this.Location = new Point(this.Location.X + 1, this.Location.Y);
                    }
                    break;

                case MoveDirection.Up:
                    {
                        // Move up
                        this.Location = new Point(this.Location.X, this.Location.Y - 1);
                    }
                    break;

                case MoveDirection.Down:
                    {
                        // Move down
                        this.Location = new Point(this.Location.X, this.Location.Y + 1);
                    }
                    break;

                default:
                    return false;
                    //throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
            }

            // If we got here the move was successful
            return true;
        }

        /// <summary>
        /// Regenerates the user to play again
        /// </summary>
        public void Respawn()
        {
            this.CurrentScore = 0;
            this.NumberOfLivesRemaining = DefaultNumLives;
        }
    }
}