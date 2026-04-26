

namespace RM.Core.Helpers
{
    public static class Accessor
    {
        public static int? Set(string value)
        {
            return !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(value) ? int.Parse(Cryptography.AES.Decrypt(value)) : null;
        }
        public static string Get<T>(T value)
        {
            return value != null ? Cryptography.AES.Encrypt(value.ToString()) : null;
        }
    }
}

