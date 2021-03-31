namespace Mma.Common.models
{
    public class WindData
    {
        public double? AverageWindDirection { get; set; } // Value in degrees (ddd)

        public double? AverageWindSpeed { get; set; } // Value in knots (ff)

        public double? MaximumWindSpeed { get; set; } // Value in knots (Gfmfm)

        public double? MinimumWindDirection { get; set; } // Null if more than 360 deg (dndndn)

        public double? MaximumWindDirection { get; set; } // Null if more than 360 deg (dxdxdx)
    }
}
