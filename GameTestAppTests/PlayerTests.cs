namespace GameTestAppTests
{
    using System.Drawing;
    using GameTestApp.Models;
    using NUnit.Framework;

    public class PlayerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstruction()
        {
            Player player = new Player();
            Assert.IsTrue(player.CurrentScore == 0);
            Assert.IsTrue(player.Location == Point.Empty);
            Assert.IsTrue(player.NumberOfLivesRemaining == 3);
        }

        [Test]
        public void TestInvalidCanMoveAfterConstruction()
        {
            Player player = new Player();
            Assert.IsFalse(player.CanPlayerMove(Player.MoveDirection.Down));
        }

        [Test]
        public void TestInvalidCanMoveLeft()
        {
            Player player = new Player
            {
                Location = new Point(1, 4)
            };

            Assert.IsFalse(player.CanPlayerMove(Player.MoveDirection.Left));
        }

        [Test]
        public void TestInvalidCanMoveRight()
        {
            Player player = new Player
            {
                Location = new Point(8, 4)
            };

            Assert.IsFalse(player.CanPlayerMove(Player.MoveDirection.Right));
        }

        [Test]
        public void TestInvalidCanMoveUp()
        {
            Player player = new Player
            {
                Location = new Point(1, 1)
            };

            Assert.IsFalse(player.CanPlayerMove(Player.MoveDirection.Up));
        }

        [Test]
        public void TestInvalidCanMoveDown()
        {
            Player player = new Player
            {
                Location = new Point(1, 8)
            };

            Assert.IsFalse(player.CanPlayerMove(Player.MoveDirection.Down));
        }

        [Test]
        public void TestCanMoveLeft()
        {
            Player player = new Player
            {
                Location = new Point(2, 4)
            };

            // Test can move and then move
            Assert.IsTrue(player.CanPlayerMove(Player.MoveDirection.Left));
            Assert.IsTrue(player.MovePlayer(Player.MoveDirection.Left));

            // Test new location
            Assert.IsTrue(player.Location.X == 1);
            Assert.IsTrue(player.Location.Y == 4);
        }

        [Test]
        public void TestCanMoveRight()
        {
            Player player = new Player
            {
                Location = new Point(7, 4)
            };

            // Test can move and then move
            Assert.IsTrue(player.CanPlayerMove(Player.MoveDirection.Right));
            Assert.IsTrue(player.MovePlayer(Player.MoveDirection.Right));

            // Test new location
            Assert.IsTrue(player.Location.X == 8);
            Assert.IsTrue(player.Location.Y == 4);
        }

        [Test]
        public void TestCanMoveUp()
        {
            Player player = new Player
            {
                Location = new Point(1, 2)
            };

            // Test can move and then move
            Assert.IsTrue(player.CanPlayerMove(Player.MoveDirection.Up));
            Assert.IsTrue(player.MovePlayer(Player.MoveDirection.Up));

            // Test new location
            Assert.IsTrue(player.Location.X == 1);
            Assert.IsTrue(player.Location.Y == 1);
        }

        [Test]
        public void TestCanMoveDown()
        {
            Player player = new Player
            {
                Location = new Point(1, 7)
            };

            // Test can move and then move
            Assert.IsTrue(player.CanPlayerMove(Player.MoveDirection.Down));
            Assert.IsTrue(player.MovePlayer(Player.MoveDirection.Down));

            // Test new location
            Assert.IsTrue(player.Location.X == 1);
            Assert.IsTrue(player.Location.Y == 8);
        }

        [Test]
        public void TestRespawn()
        {
            Player player = new Player
            {
                Location = new Point(1, 7),
                CurrentScore = 10,
                NumberOfLivesRemaining = 3,
            };

            player.Respawn();

            // Test the respawn resets the score and lives
            Assert.IsTrue(player.CurrentScore == 0);
            Assert.IsTrue(player.NumberOfLivesRemaining == 3);

            // Test the location has not changed
            Assert.IsTrue(player.Location.X == 1);
            Assert.IsTrue(player.Location.Y == 7);
        }
    }
}