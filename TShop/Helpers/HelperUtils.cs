using System.Text;

namespace TShop.Helpers
{
    public class HelperUtils
    {
        public static string GenerateRamdomKey(int length = 5)
        {
            var pattern = @"qwertyuioASDFGHJKLzxcvVBNM!%&^*123456";
            var sb = new StringBuilder();
            var rd = new Random();

            for (int i = 0; i < pattern.Length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
