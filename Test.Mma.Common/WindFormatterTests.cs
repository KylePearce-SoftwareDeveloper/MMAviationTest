namespace Test.Mma.Common
{
    using global::Mma.Common;
    using global::Mma.Common.models;
    using NUnit.Framework;

    [TestFixture]
    public class Wind_formatter_tests
    {
        private IWindFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            formatter = new WindFormatter();
            Assert.IsNotNull(formatter);
        }

        [TestCase(null, "///25KT")]
        [TestCase(10, "01025KT")]
        [TestCase(15, "01025KT")]
        [TestCase(350, "35025KT")]
        public void Average_wind_direction_is_correct(double? direction, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = direction,
                AverageWindSpeed = 25,
                MaximumWindSpeed = 28,
                MinimumWindDirection = direction,
                MaximumWindDirection = direction
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

    }
}
