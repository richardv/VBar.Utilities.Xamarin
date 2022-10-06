namespace VBarUtilities.Tests
{
    using Views;

    public class FlightsPageShould
    {
        [TestCase(2022, 8, 22, 0)]
        [TestCase(2022, 8, 23, 1)]
        [TestCase(2022, 8, 24, 2)]
        [TestCase(2022, 8, 25, 3)]
        [TestCase(2022, 8, 26, 4)]
        [TestCase(2022, 8, 27, 5)]
        [TestCase(2022, 8, 28, 6)]
        [TestCase(2022, 8, 29, 0)]
        public void DayOfMondayWeek_TakesDate_ReturnsExpectedDay(int year, int month, int  day, int expected)
        {
            // Arrange
            var date = new DateTime(year, month, day);

            // Act
            var dayOfWeek = FlightsPage.DayOfMondayWeek(date);

            // Assert
            dayOfWeek.Should().Be(expected);
        }

        [TestCase(2022, 8, 22, 2022, 8, 28)]
        [TestCase(2022, 8, 23, 2022, 8, 28)]
        [TestCase(2022, 8, 24, 2022, 8, 28)]
        [TestCase(2022, 8, 25, 2022, 8, 28)]
        [TestCase(2022, 8, 26, 2022, 8, 28)]
        [TestCase(2022, 8, 27, 2022, 8, 28)]
        [TestCase(2022, 8, 28, 2022, 8, 28)]
        [TestCase(2022, 8, 29, 2022, 9, 4)]
        public void LastDayOfWeek_TakesDate_ReturnsExpectedDate(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
        {
            // Arrange
            var date = new DateTime(year, month, day);
            var expected = new DateTime(expectedYear, expectedMonth, expectedDay);

            // Act
            var lastDayOfWeek = FlightsPage.LastDayOfWeek(date);

            // Assert
            lastDayOfWeek.Should().Be(expected);
        }
    }
}