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

        [TestCase(2, 90, 190, "VRB02G28KT")]
        [TestCase(3, 100, 120, "09003G28KT")]
        [TestCase(3, 100, 200, "VRB03G28KT")]
        [TestCase(10, 200, 250, "09010G28KT")]
        [TestCase(20, 200, 350, "09020KT200V350")]
        public void Average_wind_direction_is_correct_extra(double? averageSpeed, double? minDirection, double? maxDirection, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = 90,
                AverageWindSpeed = averageSpeed,
                MaximumWindSpeed = 28,
                MinimumWindDirection = minDirection,
                MaximumWindDirection = maxDirection
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(100, 280, "VRB20KT")]
        [TestCase(100, 350, "VRB20KT")]
        [TestCase(100, 150, "09020KT")]
        public void Average_wind_direction_is_correct_extra_extra(double? minDirection, double? maxDirection, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = 90,
                AverageWindSpeed = 20,
                MaximumWindSpeed = 28,
                MinimumWindDirection = minDirection,
                MaximumWindDirection = maxDirection
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, "00000KT")]
        [TestCase(1, "VRB01G28KT")]
        public void Average_wind_direction_is_correct_extra_extra_extra(double? windSpeed, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = 90,
                AverageWindSpeed = windSpeed,
                MaximumWindSpeed = 28,
                MinimumWindDirection = 100,
                MaximumWindDirection = 200
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(380, 120, 150, "InvalidWindDirectionError")]
        [TestCase(100, 0, 20, "InvalidWindDirectionError")]
        [TestCase(100, 120, 400, "InvalidWindDirectionError")]
        [TestCase(5, 400, 500, "InvalidWindDirectionError")]
        [TestCase(100, 120, 150, "10020KT")]
        public void Invalid_wind_direction(double? averageWindDirection, double? minimumWindDirection, double? maximumWindDirection, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = averageWindDirection,
                AverageWindSpeed = 20,
                MaximumWindSpeed = 28,
                MinimumWindDirection = minimumWindDirection,
                MaximumWindDirection = maximumWindDirection
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(-40, "InvalidMaximumWindSpeedError")]
        [TestCase(140, "10020GP99KT")]
        [TestCase(40, "10020G40KT")]
        public void Invalid_maximum_wind_speed(double? maximumWindSpeed, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = 100,
                AverageWindSpeed = 20,
                MaximumWindSpeed = maximumWindSpeed,
                MinimumWindDirection = 90,
                MaximumWindDirection = 110
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(22.7, 25, "10023KT")]
        [TestCase(22.5, 25, "10022KT")]
        [TestCase(22.3, 25, "10022KT")]
        [TestCase(22, 35.7, "10022G36KT")]
        //[TestCase(22, 35.5, "10022G35KT")]
        [TestCase(22, 35.3, "10022G35KT")]
        public void Check_wind_speed_rounding(double? averageWindSpeed, double? maximumWindSpeed, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = 100,
                AverageWindSpeed = averageWindSpeed,
                MaximumWindSpeed = maximumWindSpeed,
                MinimumWindDirection = 90,
                MaximumWindDirection = 110
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

    }
}
