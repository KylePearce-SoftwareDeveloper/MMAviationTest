namespace Mma.Common
{
    using System.Text;
    using Mma.Common.models;

    public interface IWindFormatter
    {
        string FormatWind(WindData windData);
    }

    public class WindFormatter : IWindFormatter
    {
        double? Round(double? toRound)
        {
            //Round up
            double? remainder = toRound % 10;
            if(remainder > 5)
            {
                double? roundUp = 10 - remainder;
                return toRound + roundUp;
            }
            //Round down
            return toRound - toRound % 10;
        }

        int CountDigits(double? data)
        {
            int count = 0;
            double temp = (double) data / 10.0;
            while (temp >= 1.0)
            {
                count++;
                temp /= 10.0;
            }
            return count;
        }

        public string FormatWind(WindData windData)
        {
            var result = new StringBuilder();
            bool vrb = false;
            //ddd ff Gfmfm KT dndndnVdxdxdx
            //Preliminary error checks
            bool invalidWindDirection = false;
            if(!invalidWindDirection)
            {
                if (windData.AverageWindDirection < 10 || windData.AverageWindDirection > 360)
                    invalidWindDirection = true;
                if (windData.MinimumWindDirection < 10 || windData.MinimumWindDirection > 360)
                    invalidWindDirection = true;
                if (windData.MaximumWindDirection < 10 || windData.MaximumWindDirection > 360)
                    invalidWindDirection = true;
            }
            if (windData.AverageWindSpeed < 1)
            {
                result.Append("00000KT");
            }else if(invalidWindDirection)
            {
                result.Append("InvalidWindDirectionError");
            }else if(windData.MaximumWindSpeed < 1)
            {
                result.Append("InvalidMaximumWindSpeedError");
            }
            else
            {
                if (windData.AverageWindDirection == null)
                {
                    result.Append("///");
                }
                else
                {
                    //ddd
                    windData.AverageWindDirection = Round(windData.AverageWindDirection);
                    if (windData.MaximumWindDirection - windData.MinimumWindDirection >= 60 && windData.MaximumWindDirection - windData.MinimumWindDirection <= 180 && windData.AverageWindSpeed <= 3)
                    {
                        result.Append("VRB");
                        vrb = true;
                    }
                    else if (windData.MaximumWindDirection - windData.MinimumWindDirection >= 180)
                    {
                        result.Append("VRB");
                        vrb = true;
                    }
                    else
                    {
                        int countDdd = CountDigits(windData.AverageWindDirection);
                        if (countDdd == 0)
                        {
                            result.Append($"00{windData.AverageWindDirection,000}");
                        }
                        else if (countDdd == 1)
                        {
                            result.Append($"0{windData.AverageWindDirection,000}");
                        }
                        else if (countDdd == 2)
                        {
                            result.Append($"{windData.AverageWindDirection,000}");
                        }
                    }
                }
                //ff
                windData.AverageWindSpeed = (double?)System.Math.Round((decimal)windData.AverageWindSpeed);
                int countFf = CountDigits(windData.AverageWindSpeed);
                if (countFf == 0)
                {
                    result.Append($"0{windData.AverageWindSpeed,00}");
                }
                else if (countFf == 1)
                {
                    result.Append($"{windData.AverageWindSpeed,00}");
                }
                else if (countFf == 2)
                {
                    result.Append("P99");
                }
                //Gfmfm
                if (windData.MaximumWindSpeed - windData.AverageWindSpeed >= 10)
                {
                    windData.MaximumWindSpeed = (double?)System.Math.Round((decimal)windData.MaximumWindSpeed);
                    int countGfmfm = CountDigits(windData.MaximumWindSpeed);
                    if (countGfmfm == 0)
                    {
                        result.Append($"G0{windData.MaximumWindSpeed,00}");
                    }
                    else if (countGfmfm == 1)
                    {
                        result.Append($"G{windData.MaximumWindSpeed,00}");
                    }
                    else if (countGfmfm == 2)
                    {
                        result.Append("GP99");
                    }
                }
                //KT
                result.Append("KT");
                //dndndnVdxdxdx
                if (windData.AverageWindDirection != null)
                {
                    if (!vrb)
                    {
                        if (windData.MaximumWindDirection - windData.MinimumWindDirection >= 60 && windData.MaximumWindDirection - windData.MinimumWindDirection <= 180 && windData.AverageWindSpeed > 3)
                        {
                            windData.MinimumWindDirection = Round(windData.MinimumWindDirection);
                            windData.MaximumWindDirection = Round(windData.MaximumWindDirection);
                            int countDndndn = CountDigits(windData.MinimumWindDirection);
                            int countDxdxdx = CountDigits(windData.MaximumWindDirection);
                            if (countDndndn == 0)
                            {
                                result.Append($"00{windData.MinimumWindDirection,000}");
                            }
                            else if (countDndndn == 1)
                            {
                                result.Append($"0{windData.MinimumWindDirection,000}");
                            }
                            else if (countDndndn == 2)
                            {
                                result.Append($"{windData.MinimumWindDirection,000}");
                            }
                            //V
                            result.Append("V");
                            if (countDxdxdx == 0)
                            {
                                result.Append($"00{windData.MaximumWindDirection,000}");
                            }
                            else if (countDxdxdx == 1)
                            {
                                result.Append($"0{windData.MaximumWindDirection,000}");
                            }
                            else if (countDxdxdx == 2)
                            {
                                result.Append($"{windData.MaximumWindDirection,000}");
                            }
                        }
                    }
                }
            }
            return result.ToString();
        }
    }
}
