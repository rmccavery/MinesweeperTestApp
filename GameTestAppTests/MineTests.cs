namespace GameTestAppTests
{
    using GameTestApp.Models;
    using NUnit.Framework;
    using System.Drawing;

    public class MineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstruction()
        {
            Mine mine = new Mine();

            Assert.IsFalse(mine.HasExploded);
            Assert.IsTrue(mine.Location == Point.Empty);
        }

        // Probably not a lot of point testing property notifications, but in some circumstances this may be valid
    }
}
