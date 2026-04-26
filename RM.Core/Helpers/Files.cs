using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;


namespace RM.Core.Helpers
{
    public class Files
    {
        public static string GetFileExtension(string base64String)
        {
            base64String = base64String.Split(',')[1];//base64String.Replace("data:application/pdf;base64,", string.Empty);
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                //case "E1XYD":
                //    return "rtf";
                case "EW0KI":
                    return "txt";
                case "R0LGO":
                    return "gif";
                case "UESDB":
                    return "xlsx";
                case "QUMXM":
                    return "dwg";
              
                //case "MQOWM":
                //case "77U/M":
                //    return "srt";
                default:
                    return string.Empty;
            }
        }

        public static string SaveBase64FileToServer(string FileName , string Base64, string FilePath)
        {
            byte[] Data = null;
            Base64 = Base64.Substring(Base64.IndexOf("base64,")+7).Replace(@"""", string.Empty);
            Data = Convert.FromBase64String(Base64);
            // FileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".mp4";

            System.IO.File.WriteAllBytes(FilePath+"\\"+ FileName, Data);
            return FileName;
        }

        public static float GetBase64FileSizeMb(string Base64)
        {
            var stringLength = Base64.Length;

            var sizeInBytes = 4 * Math.Ceiling((float)(stringLength / 3)) * 0.5624896334383812;
            var sizeInMb = sizeInBytes / 1000 / 1000;
            return (float)sizeInMb;
        }

        public static string getFileName(string attachedFile)
        {
            string fname;
            byte[] thdata = Convert.FromBase64String(attachedFile);

            Encoding _encoding = Encoding.Unicode;
            using (MemoryStream _memoryStream = new MemoryStream(thdata))
            {

                BinaryReader _theReader = new BinaryReader(_memoryStream);
                _theReader.BaseStream.Position = 0;
                byte[] _headerData = _theReader.ReadBytes(16);

                int _fileSize = (int)_theReader.ReadUInt32();
                int _attachmentNameLength = (int)_theReader.ReadUInt32() * 2;

                byte[] _fileNameBytes = _theReader.ReadBytes(_attachmentNameLength);
                fname = _encoding.GetString(_fileNameBytes, 0, _attachmentNameLength - 2);
                if (fname.Length > 0)
                    return fname;
            }
            return fname;
        }

        public static string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z]", string.Empty);
        }

        public static byte[] CompressToZip(List<(byte[], string)> inputBytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var input in inputBytes)
                    {
                        var zipEntry = archive.CreateEntry(input.Item2, CompressionLevel.Optimal);
                        using (var entryStream = zipEntry.Open())
                        {
                            entryStream.Write(input.Item1, 0, input.Item1.Length);
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }


    }
}
