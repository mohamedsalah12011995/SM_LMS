using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RM.Core.Helpers.ApplicationOperation;

namespace RM.Core.Helpers
{
    public static class Paginations
    {
        public static ApplicationOperation.Pagination GetPagination(ApplicationOperation.Pagination Pagination,int NumberOfRecord,ref int DefaultPaginationCount)
        {
            int CurrentPageIndex = 1;
            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;
            }
            else DefaultPaginationCount = NumberOfRecord > 0 ? NumberOfRecord : DefaultPaginationCount;


            return new ApplicationOperation.Pagination
            {
                TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount),
                CurrentPageIndex = CurrentPageIndex,
                TotalItemsCount = NumberOfRecord
            };
        }
    }
}
