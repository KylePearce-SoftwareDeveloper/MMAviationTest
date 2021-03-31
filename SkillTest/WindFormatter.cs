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
        double? RoundDown(double? toRound)
        {
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
            if (windData.AverageWindSpeed < 1)
            {
                result.Append("00000KT");
            }
            else
            {
                if (windData.AverageWindDirection == null)
                {
                    result.Append("///");
                }
                else
                {//ddd
                    windData.AverageWindDirection = RoundDown(windData.AverageWindDirection);
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
                        int count = CountDigits(windData.AverageWindDirection);
                        //double temp = (double) windData.AverageWindDirection / 10.0;
                        //while(temp >= 1.0)
                        //{
                        //    count++;
                        //    temp /= 10.0;
                        //}
                        if (count == 0)
                        {
                            result.Append($"00{windData.AverageWindDirection,000}");
                        }
                        else if (count == 1)
                        {
                            result.Append($"0{windData.AverageWindDirection,000}");
                        }
                        else if (count == 2)
                        {
                            result.Append($"{windData.AverageWindDirection,000}");
                        }
                    }
                }
                //ff
                int count2 = CountDigits(windData.AverageWindSpeed);
                if (count2 == 0)
                {
                    result.Append($"0{windData.AverageWindSpeed,00}");
                }
                else if (count2 == 1)
                {
                    result.Append($"{windData.AverageWindSpeed,00}");
                }
                else if (count2 == 2)
                {
                    result.Append($"P99");
                }

                if (windData.MaximumWindSpeed - windData.AverageWindSpeed > 10)
                    result.Append($"G{windData.MaximumWindSpeed,00}");//Gfmfm
                result.Append("KT");//KT
                if (windData.AverageWindDirection != null)
                {
                    if (!vrb)
                    {
                        if (windData.MaximumWindDirection - windData.MinimumWindDirection >= 60 && windData.MaximumWindDirection - windData.MinimumWindDirection <= 180 && windData.AverageWindSpeed > 3)
                        {
                            result.Append($"{windData.MinimumWindDirection,000}");//dndndn
                            result.Append("V");//V
                            result.Append($"{windData.MaximumWindDirection,000}");//dxdxdx
                        }
                    }
                }
            }
            return result.ToString();
        }
    }
}
