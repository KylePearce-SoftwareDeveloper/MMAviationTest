﻿namespace Mma.Common
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
                    result.Append("P99");
                }
                //Gfmfm
                if (windData.MaximumWindSpeed - windData.AverageWindSpeed >= 10)
                {
                    if (windData.MaximumWindSpeed > 99)
                    {
                        result.Append($"GP99");
                    }
                    else
                    {
                        result.Append($"G{windData.MaximumWindSpeed,00}");
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
                            windData.MinimumWindDirection = RoundDown(windData.MinimumWindDirection);
                            windData.MaximumWindDirection = RoundDown(windData.MaximumWindDirection);
                            int count3 = CountDigits(windData.MinimumWindDirection);
                            int count4 = CountDigits(windData.MaximumWindDirection);
                            if (count3 == 0)
                            {
                                result.Append($"00{windData.MinimumWindDirection,000}");
                            }
                            else if (count3 == 1)
                            {
                                result.Append($"0{windData.MinimumWindDirection,000}");
                            }
                            else if (count3 == 2)
                            {
                                result.Append($"{windData.MinimumWindDirection,000}");
                            }
                            result.Append("V");//V
                            if (count4 == 0)
                            {
                                result.Append($"00{windData.MaximumWindDirection,000}");
                            }
                            else if (count4 == 1)
                            {
                                result.Append($"0{windData.MaximumWindDirection,000}");
                            }
                            else if (count4 == 2)
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
