using System;
using System.Collections;
using System.Collections.Generic;

namespace Foundation.Data
{
    public interface IPagedData
    {
        int StartPage { get; }
        int EndPage { get; }
        int Page { get; }
        int PageSize { get; }
        int Total { get; }
        int StartNumber { get; }
        int EndNumber { get; }
        int TotalPage { get; set; }
    }

    public class PagedData<TData> : IEnumerable<TData>, IPagedData
    {
        public PagedData(int page, int pageSize, int total, IEnumerable<TData> data)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            Data = data;
            TotalPage = (int)Math.Ceiling((double)Total / pageSize);
            StartPage = Page;
            EndPage = Page == TotalPage ? Page : Page + 1;
            StartNumber = (StartPage - 1) * PageSize + 1;
            EndNumber = (StartPage - 1) * PageSize + PageSize;
        }

        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; private set; }
        public int StartNumber { get; }
        public int EndNumber { get; }
        public int TotalPage { get; set; }
        public IEnumerable<TData> Data { get; private set; }
        public IEnumerator<TData> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}
