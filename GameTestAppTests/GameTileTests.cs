namespace GameTestAppTests
{
    using GameTestApp.Models;
    using NUnit.Framework;
    using System.Drawing;

    public class GameTileTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstruction()
        {
            GameTile tile = new GameTile();

            Assert.IsTrue(tile.State == GameTile.TileState.Unvisited);
            Assert.IsTrue(tile.Location == Point.Empty);
        }

        // Probably not a lot of point testing property notifications, but in some circumstances this may be valid
    }
}
