using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;


namespace RM.Core.Helpers
{
    public class Strings
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            if (!string.IsNullOrEmpty(base64EncodedData))
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            return string.Empty;
        }
        public static bool CheckStringNullOrEmptyOrWhiteSpace(string Value)
        {
            bool CheckValue = false;
            CheckValue = string.IsNullOrEmpty(Value);
            if (!CheckValue) return CheckValue;
            CheckValue = string.IsNullOrWhiteSpace(Value);
            return CheckValue;

        }
        public static bool CheckStringNullOrEmptyOrWhiteSpaceOrZero(string Value)
        {
            bool CheckValue = false;
            CheckValue = string.IsNullOrEmpty(Value);
            if (CheckValue) return CheckValue;
            CheckValue = string.IsNullOrWhiteSpace(Value);
            if (CheckValue) return CheckValue;
            CheckValue = Value.Trim() == "0";
            return CheckValue;

        }
        public static bool CheckSaudiMobileNumber(string PhoneNumber)
        {
            if (PhoneNumber==null) return false;
            string pattern = @"^(05)([0-9]{8})$";
            // Create a Regex  
            Regex rg = new Regex(pattern);
            return rg.IsMatch(PhoneNumber);
        }
        public static bool CheckEmailValidity(string Email)
        {
            if (Email == null) return false;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match Match = regex.Match(Email);
            return Match.Success;
        }
        public static bool CheckBase64Validiy(string Value)
        {
            //Value = Value.Trim();
            //return (Value.Length % 4 == 0) && Regex.IsMatch(Value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

            Span<byte> buffer = new Span<byte>(new byte[Value.Length]);
            return Convert.TryFromBase64String(Value, buffer, out int bytesParsed);


        }
        public static bool CheckIdCardNumberValidity(string IdCardNumber)
        {
            long LngIDCardNumber = 0;
            if (!long.TryParse(IdCardNumber, out LngIDCardNumber) || IdCardNumber.Trim().Length != 10) return false;
            else return true;

        }
        public static string ConvertListToString(List<string> Item,string Splitter="$")
        {
            return string.Join(Splitter, Item.ToArray());
        }
        public static string ConvertListToStringInSearchEngine(List<string> Item, string Splitter = ";")
        {
            return string.Join(Splitter, Item.ToArray());
        }
        
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
        public static string GenerateRandomName(int Length)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < Length)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;


        }
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString();
        }
        public static long RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return long.Parse(s);
        }
        public static long RandomDigits(DateTime date, long MinValue , long MaxValue)
        {
            string input = date.ToString();
            long maxValue = MaxValue;
            long minValue = MinValue;
            Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden	
            long item = (long)Math.Round(random.NextDouble() * (maxValue - minValue - 1)) + minValue;
            return item;
        }

        public static string GenerateRandomOTP(int iOTPLength)

        {

            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
        public static string ConvertNumberToStringWithPaddingDigits(int Number,int NumberPaddingCount, int Prefex=0, int PrefexPaddingCount=0)
        {
            string PrefexPadding = string.Empty;
            string NumberPadding = string.Empty;
            if (Prefex > 0)
            {
                PrefexPadding = Prefex.ToString("D" + PrefexPaddingCount);
            }
            NumberPadding = Number.ToString("D" + NumberPaddingCount);
            string Result = PrefexPadding + NumberPadding;
            return Result;
        }
        public static string HandleGetResourcesPath(bool IsIntranetRequestType, string GetPath, string IntranetGetPath)
        {
            return IsIntranetRequestType ? IntranetGetPath : GetPath;
        }

        public static string ReplaceUrlsInContent(bool IsIntranetRequestType, string Content,string GetPath,string IntranetGetPath)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                try
                {
                    List<string> GetPathList = GetPath.Split("/").ToList();
                    List<string> IntranetGetPathList = IntranetGetPath.Split("/").ToList();
                    GetPath = String.Join("/", GetPathList.Take(GetPathList.Count - 2));
                    IntranetGetPath = String.Join("/", IntranetGetPathList.Take(IntranetGetPathList.Count - 2));

                    if (IsIntranetRequestType)
                    {
                        Content = Content.Replace(GetPath, IntranetGetPath);
                        //return Content;
                    }
                    else Content = Content.Replace(IntranetGetPath, GetPath);
                }
                catch (Exception ex)
                {
                    return Content;
                }
            }
            return Content;
        }

        public static List<string> ConvertStringToList(string str, string spliter = "$")
        {
            return str.Split(spliter, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
