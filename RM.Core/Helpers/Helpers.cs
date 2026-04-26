using FastMember;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


namespace RM.Core.Helpers
{
    public static class Helpers
    {
        public static FileStreamResult GetResoures(string path, string fileName)
        {
            FileStream fileStream;
            FileStreamResult fileStreamResult;

            var dest = path + fileName;
            fileStream = new FileStream(dest, FileMode.Open, FileAccess.Read);

            fileStreamResult = new FileStreamResult(fileStream, GetContentType(dest));
            return fileStreamResult;

        }

        public static string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public static DataTable CreateDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static DataTable GetdataTable<T>(List<T> data)
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data))
            {
                table.Load(reader);
            }
            return table;
        }

        public static string CronName(int? cronTypeId, string lang)
        {
            if (cronTypeId == (int)Enums.CronType.WhenFinishToDate)
                if (lang == "ar") return "عند انتهاء الوقت"; else return "When Finish ToDate";

            else if (cronTypeId == (int)Enums.CronType.EveryDay)
                if (lang == "ar") return "كل يوم"; else return "Every Day";

            else if (cronTypeId == (int)Enums.CronType.EveryWeek)
                if (lang == "ar") return "كل اسبوع"; else return "Every Week";

            else if (cronTypeId == (int)Enums.CronType.EveryMonth)
                if (lang == "ar") return "كل شهر"; else return "Every Month";

            else if (cronTypeId == (int)Enums.CronType.EveryQuaters)
                if (lang == "ar") return "كل ربع سنوي"; else return "Every Quaters";
            else return string.Empty;
        }

    }

    public static class GenericHelpers
    {
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default)
        {
            foreach (var c in collection.Where(c => EqualityComparer<K>.Default.Equals(parent_id_selector(c), root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }

        public static async Task<(ApplicationOperation.Pagination Pagination, IQueryable<T> Data)> TakePagginationAsync<T>(
            this IQueryable<T> collection, ApplicationOperation.Pagination Pagination, int DefaultPaginationCount)
        {
            int CurrentPageIndex = 1;
            int NumberOfRecord = await collection.CountAsync();
            var paginationItem = new ApplicationOperation.Pagination();

            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;
            }
            else
            {
                DefaultPaginationCount = NumberOfRecord > 0 ? NumberOfRecord : DefaultPaginationCount; ;
            }

            paginationItem.TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount);
            paginationItem.CurrentPageIndex = CurrentPageIndex;
            paginationItem.TotalItemsCount = NumberOfRecord;
            paginationItem.RecordPerPage = DefaultPaginationCount;
            return (paginationItem, collection.Skip((CurrentPageIndex - 1) * DefaultPaginationCount).Take(DefaultPaginationCount));
        }
        public static (ApplicationOperation.Pagination Pagination, IQueryable<T> Data) TakePaggination<T>(
        this IQueryable<T> collection, ApplicationOperation.Pagination Pagination, int DefaultPaginationCount)
        {
            int CurrentPageIndex = 1;
            int NumberOfRecord = collection.Count();
            var paginationItem = new ApplicationOperation.Pagination();

            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;
            }
            else
            {
                DefaultPaginationCount = NumberOfRecord > 0 ? NumberOfRecord : DefaultPaginationCount; ;
            }

            paginationItem.TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount);
            paginationItem.CurrentPageIndex = CurrentPageIndex;
            paginationItem.TotalItemsCount = NumberOfRecord;
            paginationItem.RecordPerPage = DefaultPaginationCount;
            return (paginationItem, collection.Skip((CurrentPageIndex - 1) * DefaultPaginationCount).Take(DefaultPaginationCount));
        }

        public static (ApplicationOperation.Pagination Pagination, IEnumerable<T> Data) TakePaggination<T>(
        this IEnumerable<T> collection, ApplicationOperation.Pagination Pagination, int DefaultPaginationCount)
        {
            int CurrentPageIndex = 1;
            int NumberOfRecord = collection.Count();
            var paginationItem = new ApplicationOperation.Pagination();

            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;
            }
            else
            {
                DefaultPaginationCount = NumberOfRecord > 0 ? NumberOfRecord : DefaultPaginationCount; ;
            }

            paginationItem.TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount);
            paginationItem.CurrentPageIndex = CurrentPageIndex;
            paginationItem.TotalItemsCount = NumberOfRecord;
            paginationItem.RecordPerPage = DefaultPaginationCount;
            return (paginationItem, collection.Skip((CurrentPageIndex - 1) * DefaultPaginationCount).Take(DefaultPaginationCount));
        }
    }

    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }

    public static class InvokeService<T>
    {
        public static async Task<T> Invoke(string ServiceUrl, string Token, dynamic RequestedData)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request;

                var RequestEncapsulation = JsonSerializer.Serialize(RequestedData);
                request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
                //  request.Headers.Add("Authorization", Token);
                request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);

                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/soap+xml");
                var response = await httpClient.SendAsync(request);
                return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
            }
        }


        public static async Task<T> Invoke(string serviceUrl, dynamic RequestedData)
        {
            //var handler = new HttpClientHandler();
            //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //handler.ServerCertificateCustomValidationCallback =
            //    (httpRequestMessage, cert, cetChain, policyErrors) =>
            //    {
            //        return true;
            //    };
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request;

                    var RequestEncapsulation = JsonSerializer.Serialize(RequestedData);
                    request = new HttpRequestMessage(HttpMethod.Post, serviceUrl);

                    request.Content = new StringContent(RequestEncapsulation, Encoding.UTF8);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await httpClient.SendAsync(request);
                    return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
                }
                catch (Exception ex) { throw; }
            }


        }

        public static async Task<T> Invoke(string serviceUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request;
                request = new HttpRequestMessage(HttpMethod.Post, serviceUrl);
                request.Content = new StringContent("", Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await httpClient.SendAsync(request);
                return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
            }
        }


    }

    public class ConvertDate
    {
        public static DateTime ConvertHiriToGregorian(string DateConv)
        {
            CultureInfo arSA = new CultureInfo("ar-SA");
            arSA.DateTimeFormat.Calendar = new HijriCalendar();
            return DateTime.ParseExact(DateConv, "dd/MM/yyyy", arSA);
        }

    }

}
