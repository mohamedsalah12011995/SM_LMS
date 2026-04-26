
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace RM.Core.Helpers
{
    public class Images
    {
        public static string ConvertFromImageToBase64(string ImageURl)
        {
            if (!File.Exists(ImageURl)) return "";
            byte[] image = System.IO.File.ReadAllBytes(ImageURl);
            return Convert.ToBase64String(image, 0, image.Length);
        }
        public static SixLabors.ImageSharp.Image ConvertFromBase64ToImage(string Base64String)
        {
            SixLabors.ImageSharp.Image returnImage = null;
            Base64String = Regex.Replace(Base64String, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
            byte[] imageBytes = Convert.FromBase64String(Base64String);
            try
            {
                MemoryStream ms = new MemoryStream(imageBytes);
                returnImage = SixLabors.ImageSharp.Image.Load(ms);
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            }
            return returnImage;
        }

        public static SixLabors.ImageSharp.Image ScaleImage(SixLabors.ImageSharp.Image image, int maxHeight)
        {
            var ratio = (double)maxHeight / image.Height;

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            image.Mutate(x => x.Resize(newWidth, newHeight));

            // var newImage = new Bitmap(newWidth, newHeight);
            //using (var g = Graphics.FromImage(newImage))
            //{
            //    g.DrawImage(image, 0, 0, newWidth, newHeight);
            //}
            return image;
        }
        private static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
        }
        public static String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }

        public static bool IsBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }


        public static string SaveSingleImageOnServer(string images, int? MaxImageWidth = null, string PathUrl = "", bool SaveWithIcon = false, int? IconImageWidth = null, string IconPath = "")
        {
            List<string> ImagesList = new List<string>();
            ImagesList.Add(images);
            return SaveImagesOnServer(ImagesList, MaxImageWidth, PathUrl, SaveWithIcon, IconImageWidth, IconPath)[0];
        }

        public static List<string> SaveImagesOnServer(List<string> images, int? MaxImageWidth = null, string PathUrl = "", bool SaveWithIcon = false, int? IconImageWidth = null, string IconPath = "")
        {
            string ImageName = "";
            List<string> OutImagesName = new List<string>();
            SixLabors.ImageSharp.Image Image = null;
            for (int i = 0; i < images.Count; i++)
            {
                Image = ConvertFromBase64ToImage(images[i]);

                if (images[i].Contains("data:image/png;base64,"))
                    ImageName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".png";

                else ImageName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".Jpeg";

                if (MaxImageWidth.HasValue)
                    ScaleImage(Image, MaxImageWidth.Value).Save(PathUrl + "\\" + ImageName);

                else Image.Save(PathUrl + "\\" + ImageName);

                if (SaveWithIcon && IconImageWidth.HasValue)
                    ScaleImage(Image, IconImageWidth.Value).Save(IconPath + "\\" + "ico_" + ImageName);

                OutImagesName.Add(ImageName);
            }

            return OutImagesName;
        }

        public static string GetYouTubeThumbnail(string YoutubeUrl)
        {
            string youTubeThumb = string.Empty;
            if (string.IsNullOrEmpty(YoutubeUrl))
                return "";

            if (YoutubeUrl.IndexOf("=") > 0)
            {
                youTubeThumb = YoutubeUrl.Split('=')[1];
            }
            else if (YoutubeUrl.IndexOf("/v/") > 0)
            {
                string strVideoCode = YoutubeUrl.Substring(YoutubeUrl.IndexOf("/v/") + 3);
                int ind = strVideoCode.IndexOf("?");
                youTubeThumb = strVideoCode.Substring(0, ind == -1 ? strVideoCode.Length : ind);
            }
            else if (YoutubeUrl.IndexOf('/') < 6)
            {
                youTubeThumb = YoutubeUrl.Split('/')[3];
            }
            else if (YoutubeUrl.IndexOf('/') > 6)
            {
                youTubeThumb = YoutubeUrl.Split('/')[1];
            }

            return "http://img.youtube.com/vi/" + youTubeThumb + "/mqdefault.jpg";
        }


    }
}
