using System.Text;

namespace RM.Exams.Helper
{


    public class RandomCertifcationNumberGenerator
    {
        private static readonly Random random = new Random();

        public static string GenerateCertifcationNumber()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";

            StringBuilder code = new StringBuilder();


            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    code.Append(digits[random.Next(digits.Length)]);
                }
                else
                {
                    code.Append(letters[random.Next(letters.Length)]);
                }
            }

            code.Append('-');

            for (int i = 0; i < 6; i++)
            {
                if (i < 2)
                {
                    code.Append(digits[random.Next(digits.Length)]);
                }
                else
                {
                    code.Append(letters[random.Next(letters.Length)]);
                }
            }

            return code.ToString();
        }

        public static void Main()
        {
            string randomCode = GenerateCertifcationNumber();
            Console.WriteLine(randomCode);
        }
    }

}
