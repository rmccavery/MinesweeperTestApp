namespace GameTestApp.Converters
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converter for turning point locations in to chess coordinates
    /// </summary>
    public class PointToChessPositionConverter : IValueConverter
    {
        public string[] Positions = {"A", "B", "C", "D", "E", "F", "G", "H"};

        /// <summary>
        /// Converts data bound values for display
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="targetType">The type of the value</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">The converter culture</param>
        /// <returns>Converted string position</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point pointValue)
            {
                if (pointValue.X >= 1 
                    && pointValue.X <= this.Positions.Length
                    && pointValue.Y <= 8)
                {
                    return $"{this.Positions[pointValue.X - 1]}{pointValue.Y}";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do nothing
            return null;
        }
    }
}