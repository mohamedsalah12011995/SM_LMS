
using System.Globalization;


namespace RM.Core.Helpers
{
    public class Dates
    {
        private static readonly Dictionary<string, TimeSpan> CultureOffsets = new Dictionary<string, TimeSpan>
        {
            {
                "ar-SA", new TimeSpan(3, 0, 0)
            }
        };
        // UTC+3 for Saudi Arabia };
        public static DateTime ConvertUmAlquraStringDateToGeorogian(string HijriDate)
        {
            List<int> DateArray = HijriDate.Split('/').Select(x => int.Parse(x)).ToList();

            UmAlQuraCalendar IHijri = new UmAlQuraCalendar();
            GregorianCalendar Gergion = new GregorianCalendar();
            DateTime ResultDate = DateTime.MinValue;
            DateTime hijriDate = new DateTime(DateArray[0], DateArray[1], DateArray[2], IHijri);
            int Year = Gergion.GetYear(hijriDate);
            int Month = Gergion.GetMonth(hijriDate);
            int Day = Gergion.GetDayOfMonth(hijriDate);
            ResultDate = new DateTime(Year, Month, Day);

            return ResultDate;


        }

        public static DateTime ChangeDateFormat(string date)
        {
            string[] formats = { "dd-MM-yyyy" };
            return DateTime.ParseExact(date, formats, new CultureInfo("en-US"), DateTimeStyles.None);
        }
        public static DateTime ConvertFromGerogianToHijri(DateTime Date)
        {
            UmAlQuraCalendar hijriCalendar = new UmAlQuraCalendar();
            GregorianCalendar Gergion = new GregorianCalendar();
            int hijriYear = hijriCalendar.GetYear(Date);
            int hijriMonth = hijriCalendar.GetMonth(Date);
            int hijriDay = hijriCalendar.GetDayOfMonth(Date);
            return new DateTime(hijriYear, hijriMonth, hijriDay, Gergion);
        }

        public static string ConvertFromGerogianToHijriDateString(DateTime Date, string format = null)
        {
            UmAlQuraCalendar hijriCalendar = new UmAlQuraCalendar();
            string hijriDate = string.Empty;
            int hijriYear = hijriCalendar.GetYear(Date);
            int hijriMonth = hijriCalendar.GetMonth(Date);
            int hijriDay = hijriCalendar.GetDayOfMonth(Date);
            format = format ?? "yyyy-MM-dd";
            switch (format)
            {
                case "dd-MM-yyyy":
                    hijriDate = $"{hijriDay}-{hijriMonth}-{hijriYear}";
                    break;
                default:
                    hijriDate = $"{hijriYear}-{hijriMonth}-{hijriDay}";
                    break;
            }
            return hijriDate;
        }

        public static DateTime ConvertFromHijriToGerogian(string Date)
        {
            CultureInfo arSA = new CultureInfo("ar-SA");
            arSA.DateTimeFormat.Calendar = new HijriCalendar();
            return DateTime.ParseExact(Date, "yyyy-MM-dd", arSA);
        }
        public static string ConvertHijriStringDateToGeorogian(string HijriDate)
        {

            HijriCalendar hc = new HijriCalendar();
            int year = int.Parse(HijriDate.Substring(0, 4));
            string rem = HijriDate.Remove(0, 5);
            int end = rem.IndexOf('-', 0);
            int month = int.Parse(rem.Substring(0, end));
            rem = rem.Remove(0, end + 1);
            int day = int.Parse(rem);
            DateTime date = new DateTime(year, month, day, hc);
            return date.ToString("yyyy-MM-dd");

            //CultureInfo arSA = new CultureInfo("ar-SA");
            //arSA.DateTimeFormat.Calendar = new HijriCalendar();
            //var toGegorian = DateTime.ParseExact("16/02/1444", "dd/MM/yyyy", arSA);
        }

        public static DateTime GetCurrentDateTimeInCulture(string cultureCode = "ar-SA")
        {
            if (!CultureOffsets.ContainsKey(cultureCode))
            {
                throw new ArgumentException("Time zone offset not found for the specified culture.");
            }
            TimeSpan offset = CultureOffsets[cultureCode];
            DateTime currentDateTime = DateTime.UtcNow;
            DateTime dateTimeInTimeZone = currentDateTime.Add(offset); return dateTimeInTimeZone;
        }



    }
}
