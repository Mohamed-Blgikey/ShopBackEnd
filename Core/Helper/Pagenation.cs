using Core.Entities;

namespace Core.Helper
{
    public class Pagenation<T> where T : class
    {
        public int pageIndex { set; get; }
        public int pageSize { set; get; }
        public int count { set; get; }
        public int TotalPage { set; get; }
        public IReadOnlyList<T> Items { set; get; } 

        public Pagenation(int pageIndex, int pageSize,int count, int totalCount, IReadOnlyList<T> items)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.count = count;
            this.TotalPage = (int)Math.Ceiling(totalCount / (double) pageSize);
            Items = items;
        }
    }
}
