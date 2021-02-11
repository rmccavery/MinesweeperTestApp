namespace GameTestApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using GameTestApp.Business;
    using GameTestApp.Models;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Point = System.Drawing.Point;

    /// <summary>
    /// Provides view model functionality for the main game view
    /// </summary>
    public class GameViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly GameController gameController;
        private int highScore;
        private bool isGameInProgress = true;
        private DelegateCommand moveDownCommand;
        private DelegateCommand moveLeftCommand;
        private DelegateCommand moveRightCommand;
        private DelegateCommand moveUpCommand;
        private DelegateCommand restartCommand;

        /// <summary>
        /// Initializes a new instance of the GameViewModel class
        /// </summary>
        /// <param name="eventAggregator">THe prism event aggregator</param>
        /// <param name="gameController">The game controller object</param>
        public GameViewModel(IEventAggregator eventAggregator, GameController gameController)
        {
            this.eventAggregator = eventAggregator;
            this.gameController = gameController;

            // Wire up some events from the event aggregator to ensure we
            // only fire property change notifications when necessary
            this.eventAggregator.GetEvent<GameStartedEvent>().Subscribe(() =>
            {
                // Game started
                this.IsGameInProgress = true;
                this.RaisePropertyChanged(nameof(this.NumberOfLivesRemaining));
                this.RaisePropertyChanged(nameof(this.CurrentScore));
                this.RaisePropertyChanged(nameof(this.CurrentPosition));
                this.RaisePropertyChanged(nameof(this.GameTiles));
            });

            this.eventAggregator.GetEvent<GameOverEvent>().Subscribe(() =>
            {
                // Game ended
                this.IsGameInProgress = false;

                // Show a message box
                MessageBox.Show("You lost! Press restart to try again", Application.Current?.MainWindow?.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            });

            this.eventAggregator.GetEvent<GameMineExplodedEvent>().Subscribe(() =>
            {
                // Mine exploded, just refresh number of lives
                this.RaisePropertyChanged(nameof(this.NumberOfLivesRemaining));
                this.RaisePropertyChanged(nameof(this.CurrentPosition));
            });

            this.eventAggregator.GetEvent<GameScoreChangedEvent>().Subscribe(() =>
            {
                // Score has changed
                this.RaisePropertyChanged(nameof(this.CurrentScore));
                this.RaisePropertyChanged(nameof(this.CurrentPosition));
            });

            this.eventAggregator.GetEvent<GamePlayerWonEvent>().Subscribe(() =>
            {
                // Player got to other side and won
                // Disable further input
                this.IsGameInProgress = false;

                // Update the high score
                this.HighScore = Math.Max(this.CurrentScore, this.HighScore);

                // Show a message box
                MessageBox.Show($"You won with a score of {this.CurrentScore}! Press restart to try again", Application.Current?.MainWindow?.Title, MessageBoxButton.OK,
                    MessageBoxImage.Information);
            });
        }

        /// <summary>
        /// Gets the move left command
        /// </summary>
        public DelegateCommand MoveLeftCommand
        {
            get { return this.moveLeftCommand ??= new DelegateCommand(this.ExecuteMoveLeftCommand, this.CanExecuteMoveLeftCommand); }
        }

        /// <summary>
        /// Gets the move right command
        /// </summary>
        public DelegateCommand MoveRightCommand
        {
            get { return this.moveRightCommand ??= new DelegateCommand(this.ExecuteMoveRightCommand, this.CanExecuteMoveRightCommand); }
        }

        /// <summary>
        /// Gets the move up command
        /// </summary>
        public DelegateCommand MoveUpCommand
        {
            get { return this.moveUpCommand ??= new DelegateCommand(this.ExecuteMoveUpCommand, this.CanExecuteMoveUpCommand); }
        }

        /// <summary>
        /// Gets the move down command
        /// </summary>
        public DelegateCommand MoveDownCommand
        {
            get { return this.moveDownCommand ??= new DelegateCommand(this.ExecuteMoveDownCommand, this.CanExecuteMoveDownCommand); }
        }

        /// <summary>
        /// Gets the restart delegate command
        /// </summary>
        public DelegateCommand RestartCommand
        {
            get { return this.restartCommand ??= new DelegateCommand(this.ExecuteRestart); }
        }

        /// <summary>
        /// Gets or sets a value determining whether the game is in progress
        /// </summary>
        public bool IsGameInProgress
        {
            get { return this.isGameInProgress; }
            set { this.SetProperty(ref this.isGameInProgress, value); }
        }

        /// <summary>
        /// Gets the collection of tiles from the board
        /// </summary>
        public List<GameTile> GameTiles
        {
            get { return this.gameController?.GameBoard?.Tiles; }
        }

        /// <summary>
        /// Gets the number of lives remaining
        /// </summary>
        public int NumberOfLivesRemaining
        {
            get
            {
                return this.gameController
                           .Player
                           .NumberOfLivesRemaining;
            }
        }

        /// <summary>
        /// Gets the current score
        /// </summary>
        public int CurrentScore
        {
            get
            {
                return this.gameController
                           .Player
                           .CurrentScore;
            }
        }

        /// <summary>
        /// Gets the current player location
        /// </summary>
        public Point CurrentPosition
        {
            get
            {
                return this.gameController
                           .Player
                           .Location;
            }
        }

        /// <summary>
        /// Gets or sets the high score
        /// </summary>
        public int HighScore
        {
            get { return this.highScore; }
            set { this.SetProperty(ref this.highScore, value); }
        }


        private bool CanExecuteMoveDownCommand()
        {
            return this.IsGameInProgress
                   && this.gameController.CanPlayerMove(Player.MoveDirection.Down);
        }

        private bool CanExecuteMoveUpCommand()
        {
            return this.IsGameInProgress
                   && this.gameController.CanPlayerMove(Player.MoveDirection.Up);
        }

        private bool CanExecuteMoveRightCommand()
        {
            return this.IsGameInProgress
                   && this.gameController.CanPlayerMove(Player.MoveDirection.Right);
        }

        private bool CanExecuteMoveLeftCommand()
        {
            return this.IsGameInProgress
                   && this.gameController.CanPlayerMove(Player.MoveDirection.Left);
        }

        private void ExecuteMoveUpCommand()
        {
            if (true == this.CanExecuteMoveUpCommand())
            {
                this.gameController.MovePlayer(Player.MoveDirection.Up);
            }
        }

        private void ExecuteMoveDownCommand()
        {
            if (true == this.CanExecuteMoveDownCommand())
            {
                this.gameController.MovePlayer(Player.MoveDirection.Down);
            }
        }

        private void ExecuteMoveLeftCommand()
        {
            if (true == this.CanExecuteMoveLeftCommand())
            {
                this.gameController.MovePlayer(Player.MoveDirection.Left);
            }
        }

        private void ExecuteMoveRightCommand()
        {
            if (true == this.CanExecuteMoveRightCommand())
            {
                this.gameController.MovePlayer(Player.MoveDirection.Right);
            }
        }

        private void ExecuteRestart()
        {
            // Perform reset on game controller
            this.gameController
                .ResetGame();
        }
    }
}