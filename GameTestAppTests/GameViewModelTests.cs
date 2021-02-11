namespace GameTestAppTests
{
    using System.Linq;
    using GameTestApp.Business;
    using GameTestApp.ViewModels;
    using NUnit.Framework;
    using Prism.Events;

    public class GameViewModelTests
    {
        private EventAggregator eventAggregator;
        private GameController gameController;

        [SetUp]
        public void Setup()
        {
            this.eventAggregator = new EventAggregator();
            this.gameController = new GameController(this.eventAggregator);
        }

        [Test]
        public void TestConstruction()
        {
            // NOTE most of the functions of the view model have been tested elsewhere
            GameViewModel viewModel = new GameViewModel(this.eventAggregator, this.gameController);

            // Check home point and score
            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(1, 4));
            Assert.IsTrue(viewModel.CurrentScore == 0);
            Assert.IsTrue(viewModel.HighScore == 0);
            Assert.IsTrue(viewModel.NumberOfLivesRemaining == 3);

            // Any tiles
            Assert.IsTrue(viewModel.GameTiles?.Any());
            Assert.IsTrue(viewModel.GameTiles?.Count() == 64);

            // Game started?
            Assert.IsTrue(viewModel.IsGameInProgress);

        }

        [Test]
        public void TestResetGame()
        {
            // NOTE most of the functions of the view model have been tested elsewhere
            GameViewModel viewModel = new GameViewModel(this.eventAggregator, this.gameController);

            Assert.IsTrue(viewModel.RestartCommand.CanExecute());
            viewModel.RestartCommand.Execute();

            // Now test everything ok
            // Check home point and score
            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(1, 4));
            Assert.IsTrue(viewModel.CurrentScore == 0);
            Assert.IsTrue(viewModel.HighScore == 0);
            Assert.IsTrue(viewModel.NumberOfLivesRemaining == 3);

            // Any tiles
            Assert.IsTrue(viewModel.GameTiles?.Any());
            Assert.IsTrue(viewModel.GameTiles?.Count() == 64);

            // Game started?
            Assert.IsTrue(viewModel.IsGameInProgress);
        }


        [Test]
        public void TestCommands()
        {
            // NOTE most of the functions of the view model have been tested elsewhere
            GameViewModel viewModel = new GameViewModel(this.eventAggregator, this.gameController);

            // Shouldn't be able to move left
            Assert.IsFalse(viewModel.MoveLeftCommand.CanExecute());
            viewModel.RestartCommand.Execute();

            // Should be able to move right
            Assert.IsTrue(viewModel.MoveRightCommand.CanExecute());
            viewModel.MoveRightCommand.Execute();

            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(2, 4));

            // Should be able to move left
            Assert.IsTrue(viewModel.MoveLeftCommand.CanExecute());
            viewModel.MoveLeftCommand.Execute();

            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(1, 4));

            // Should be able to move up
            Assert.IsTrue(viewModel.MoveUpCommand.CanExecute());
            viewModel.MoveUpCommand.Execute();

            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(1, 3));

            Assert.IsTrue(viewModel.MoveUpCommand.CanExecute());
            viewModel.MoveUpCommand.Execute();

            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(1, 2));

            Assert.IsTrue(viewModel.MoveUpCommand.CanExecute());
            viewModel.MoveUpCommand.Execute();

            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(1, 1));
            Assert.IsFalse(viewModel.MoveUpCommand.CanExecute());

            // Move down
            Assert.IsTrue(viewModel.MoveDownCommand.CanExecute());
            viewModel.MoveDownCommand.Execute();

            Assert.IsTrue(viewModel.CurrentPosition == new System.Drawing.Point(1, 2));
            Assert.IsTrue(viewModel.MoveUpCommand.CanExecute());
        }
    }
}
