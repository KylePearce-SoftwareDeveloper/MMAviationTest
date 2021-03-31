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

        public string FormatWind(WindData windData)
        {
            var result = new StringBuilder();
            //ddd ff Gfmfm KT dndndnVdxdxdx
            if (windData.AverageWindDirection == null)
            {
                result.Append("///");
            }
            else
            {//ddd
                windData.AverageWindDirection = RoundDown(windData.AverageWindDirection);
                int count = 0;
                double temp = (double) windData.AverageWindDirection / 10.0;
                while(temp >= 1.0)
                {
                    count++;
                    temp /= 10.0;
                }
                if (count == 0)
                {
                    result.Append($"00{windData.AverageWindDirection,000}");
                }else if(count == 1)
                {
                    result.Append($"0{windData.AverageWindDirection,000}");
                }else if(count == 2)
                {
                    result.Append($"{windData.AverageWindDirection,000}");
                }
            }
            result.Append($"{windData.AverageWindSpeed,00}");//ff
            if(windData.MaximumWindSpeed - windData.AverageWindSpeed > 10)
                result.Append($"G{windData.MaximumWindSpeed,00}");//Gfmfm
            result.Append("KT");//KT
            if (windData.AverageWindDirection != null)
            {
                if (windData.MinimumWindDirection - windData.MaximumWindDirection >= 60 && windData.MinimumWindDirection - windData.MaximumWindDirection <= 180 && windData.AverageWindSpeed > 3)
                {
                    result.Append($"{windData.MinimumWindDirection,000}");//dndndn
                    result.Append("V");//V
                    result.Append($"{windData.MaximumWindDirection,000}");//dxdxdx
                }
            }

            return result.ToString();
        }
    }
}
