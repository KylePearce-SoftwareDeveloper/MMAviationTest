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
        public string FormatWind(WindData windData)
        {
            var result = new StringBuilder();
            //ddd ff Gfmfm KT dndndnVdxdxdx
            if (windData.AverageWindDirection == null)
            {
                result.Append("///");
            }
            else
            {
                result.Append($"{windData.AverageWindDirection,000}");//ddd
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
