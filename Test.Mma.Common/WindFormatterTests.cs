﻿namespace Test.Mma.Common
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

    }
}
