using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FW.UintOfWork.Collections
{
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// 起始页 值
        /// </summary>
        public int IndexFrom { get; set; }
        /// <summary>
        /// 当前页 值 
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 当前页数据
        /// </summary>
        public IList<T> Items { get; set; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage => PageIndex - IndexFrom > 0;
        /// <summary>
        /// 是否有下一页
        /// </summary>s
        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页几条</param>
        /// <param name="indexFrom">从第几页开始</param>
        internal PagedList(IEnumerable<T> source,int pageIndex, int pageSize,int indexFrom)
        {
            if (IndexFrom > PageIndex)
            {
                throw new ArgumentException($"indexFrom:{IndexFrom}>pageIndex:{PageIndex},起始页必须小于等于当前页");
            }

            if (source is IQueryable<T> querable)
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                IndexFrom = indexFrom;
                TotalCount = querable.Count();
                TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

                Items = querable.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                IndexFrom = indexFrom;
                TotalCount = source.Count();
                TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

                Items = source.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize).ToList();
            }
        }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        internal PagedList() => Items = new T[0];
    }

    internal class PagedList<TSource, TResult> : IPagedList<TResult>
    {
        /// <summary>
        /// 起始页 值
        /// </summary>
        public int IndexFrom { get; set; }
        /// <summary>
        /// 当前页 值
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 当前页数据
        /// </summary>
        public IList<TResult> Items { get; set; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage => PageIndex - IndexFrom > 0;
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

        public PagedList(IEnumerable<TSource> source,Func<IEnumerable<TSource>,IEnumerable<TResult>> converter,int pageIndex,int pageSize,int indexFrom)
        {
            if (IndexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom:{IndexFrom}>pageIndex:{PageIndex},起始页必须小于等于当前页");
            }

            if (source is IQueryable<TSource> querable)
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                IndexFrom = indexFrom;
                TotalCount = querable.Count();
                TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

                var items = querable.Skip((PageIndex - IndexFrom) * PageSize).Take(pageSize).ToArray();

                Items = new List<TResult>(converter(items));
            }
            else
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                IndexFrom = indexFrom;
                TotalCount = source.Count();
                TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

                var items = source.Skip((PageIndex - IndexFrom) * PageSize).Take(pageSize).ToArray();

                Items = new List<TResult>(converter(items));
            }
        }

        public PagedList(IPagedList<TSource> source,Func<IEnumerable<TSource>,IEnumerable<TResult>> converter)
        {
            PageIndex = source.PageIndex;
            PageSize = source.PageSize;
            IndexFrom = source.IndexFrom;
            TotalCount = source.TotalCount;
            TotalPages = source.TotalPages;

            Items = new List<TResult>(converter(source.Items));
        }
    }

    public static class PagedList
    {
        public static IPagedList<T> Empty<T>() => new PagedList<T>();

        public static IPagedList<TResult> From<TResult, TSource>( IPagedList<TSource> source,Func<IEnumerable<TSource>, IEnumerable<TResult>> converter ) => new PagedList<TSource, TResult>(source,converter); 
    }
}
