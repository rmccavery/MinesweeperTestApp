namespace GameTestAppTests
{
    using System.Drawing;
    using System.Globalization;
    using GameTestApp.Converters;
    using NUnit.Framework;

    public class ConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestChessPositionConverterValidValues()
        {
            PointToChessPositionConverter converter = new PointToChessPositionConverter();

            Assert.IsTrue((string)converter.Convert(new Point(1, 1), typeof(Point), null, CultureInfo.InvariantCulture) == "A1");
            Assert.IsTrue((string)converter.Convert(new Point(1, 8), typeof(Point), null, CultureInfo.InvariantCulture) == "A8");
            Assert.IsTrue((string)converter.Convert(new Point(2, 1), typeof(Point), null, CultureInfo.InvariantCulture) == "B1");
            Assert.IsTrue((string)converter.Convert(new Point(2, 8), typeof(Point), null, CultureInfo.InvariantCulture) == "B8");
            Assert.IsTrue((string)converter.Convert(new Point(8, 1), typeof(Point), null, CultureInfo.InvariantCulture) == "H1");
            Assert.IsTrue((string)converter.Convert(new Point(8, 8), typeof(Point), null, CultureInfo.InvariantCulture) == "H8");
        }


        [Test]
        public void TestChessPositionConverterInvalidValues()
        {
            PointToChessPositionConverter converter = new PointToChessPositionConverter();

            Assert.IsTrue((string)converter.Convert(new Point(0, 1), typeof(Point), null, CultureInfo.InvariantCulture) == string.Empty);
            Assert.IsTrue((string)converter.Convert(new Point(1, 9), typeof(Point), null, CultureInfo.InvariantCulture) == string.Empty);
            Assert.IsTrue((string)converter.Convert(new Point(0, 0), typeof(Point), null, CultureInfo.InvariantCulture) == string.Empty);
            Assert.IsTrue((string)converter.Convert(new Point(9, 9), typeof(Point), null, CultureInfo.InvariantCulture) == string.Empty);
        }
    }
}
