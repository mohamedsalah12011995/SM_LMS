using System.Security.Cryptography;
using System.Text;


namespace RM.Core.Helpers
{
    public class Cryptography
    {
        static string key = "2266881010";
        public class AES
        {
            public static object? Strings { get; private set; }

            public static string Encrypt(string source)
            {
                TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

                byte[] byteHash;
                byte[] byteBuff;

                byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                desCryptoProvider.Key = byteHash;
                desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = Encoding.UTF8.GetBytes(source);

                string encoded =
                    Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                return RM.Core.Helpers.Strings.Base64Encode(encoded);
            }
            public static string Encrypt(int ID)
            {
                string source = ID.ToString();
                TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

                byte[] byteHash;
                byte[] byteBuff;

                byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                desCryptoProvider.Key = byteHash;
                desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = Encoding.UTF8.GetBytes(source);

                string encoded =
                    Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                return RM.Core.Helpers.Strings.Base64Encode(encoded);
            }
            public static string Decrypt(string encodedText)
            {
                encodedText = RM.Core.Helpers.Strings.Base64Decode(encodedText);
                TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

                byte[] byteHash;
                byte[] byteBuff;

                byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                desCryptoProvider.Key = byteHash;
                desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = Convert.FromBase64String(encodedText);

                string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                return plaintext;
            }
        }
        public class DES
        {
            public static string Encrypt(string TextToEncrypt)
            {
                byte[] MyEncryptedArray = Encoding.UTF8.GetBytes(TextToEncrypt);
                MD5CryptoServiceProvider MyMD5CryptoService = new MD5CryptoServiceProvider();
                byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(key));
                MyMD5CryptoService.Clear();
                var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();
                MyTripleDESCryptoService.Key = MysecurityKeyArray;
                MyTripleDESCryptoService.Mode = CipherMode.ECB;
                MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;
                var MyCrytpoTransform = MyTripleDESCryptoService.CreateEncryptor();
                byte[] MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyEncryptedArray, 0, MyEncryptedArray.Length);
                MyTripleDESCryptoService.Clear();
                return Convert.ToBase64String(MyresultArray, 0, MyresultArray.Length);
            }
            public static string Encrypt(int ID)
            {
                string TextToEncrypt = ID.ToString();
                byte[] MyEncryptedArray = Encoding.UTF8.GetBytes(TextToEncrypt);
                MD5CryptoServiceProvider MyMD5CryptoService = new MD5CryptoServiceProvider();
                byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(key));
                MyMD5CryptoService.Clear();
                var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();
                MyTripleDESCryptoService.Key = MysecurityKeyArray;
                MyTripleDESCryptoService.Mode = CipherMode.ECB;
                MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;
                var MyCrytpoTransform = MyTripleDESCryptoService.CreateEncryptor();
                byte[] MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyEncryptedArray, 0, MyEncryptedArray.Length);
                MyTripleDESCryptoService.Clear();
                return Convert.ToBase64String(MyresultArray, 0, MyresultArray.Length);
            }

            public static string Decrypt(string TextToDecrypt)
            {
                byte[] MyDecryptArray = Convert.FromBase64String(TextToDecrypt);
                MD5CryptoServiceProvider MyMD5CryptoService = new MD5CryptoServiceProvider();
                byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(key));
                MyMD5CryptoService.Clear();
                var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();
                MyTripleDESCryptoService.Key = MysecurityKeyArray;
                MyTripleDESCryptoService.Mode = CipherMode.ECB;
                MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;
                var MyCrytpoTransform = MyTripleDESCryptoService.CreateDecryptor();
                byte[] MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyDecryptArray, 0, MyDecryptArray.Length);
                MyTripleDESCryptoService.Clear();
                return Encoding.UTF8.GetString(MyresultArray);
            }
        }

        public class RC4
        {

            static byte[] key = { 1, 2, 3, 4, 5 };

            private static byte[] InternalEncrypt(byte[] pwd, byte[] data)
            {
                int a, i, j, k, tmp;
                int[] key, box;
                byte[] cipher;

                key = new int[256];
                box = new int[256];
                cipher = new byte[data.Length];

                for (i = 0; i < 256; i++)
                {
                    key[i] = pwd[i % pwd.Length];
                    box[i] = i;
                }
                for (j = i = 0; i < 256; i++)
                {
                    j = (j + box[i] + key[i]) % 256;
                    tmp = box[i];
                    box[i] = box[j];
                    box[j] = tmp;
                }
                for (a = j = i = 0; i < data.Length; i++)
                {
                    a++;
                    a %= 256;
                    j += box[a];
                    j %= 256;
                    tmp = box[a];
                    box[a] = box[j];
                    box[j] = tmp;
                    k = box[(box[a] + box[j]) % 256];
                    cipher[i] = (byte)(data[i] ^ k);
                }
                return cipher;
            }
            private static byte[] InternalDecrypt(byte[] pwd, byte[] data)
            {
                return InternalEncrypt(pwd, data);
            }
            static public string Encrypt(string Value)
            {
                byte[] enc = InternalEncrypt(key, Encoding.UTF8.GetBytes(Value));
                return Convert.ToBase64String(enc);
            }
            static public string Decrypt(string Value)
            {
                byte[] code = Convert.FromBase64String(Value);
                return Encoding.UTF8.GetString(InternalDecrypt(key, code));
            }

        }

    }
}
