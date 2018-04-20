using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.AspNetCore.Hosting;
using PanGu;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Shop.Apis.Helpers
{
    public class SearchEngine
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public SearchEngine(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Segment.Init(PanGuXmlPath);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="query"></param>
        /// <param name="price">100-300</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<GoodsAlias> SearchGoods(string query,string price,DateTime? startDate,DateTime? endDate)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();//计时开始 

            var ver = Lucene.Net.Util.Version.LUCENE_30;
            Directory indexDirectory = FSDirectory.Open(new System.IO.DirectoryInfo(SearchIndexPath));

            Analyzer analyzer = new PanGuAnalyzer();


            IndexSearcher searcher = null;
            List<GoodsAlias> goodses=new List<GoodsAlias>();
            int recCount = 0;
            bool isHasQuery = !string.IsNullOrEmpty(query),
            isHasPrice = !string.IsNullOrEmpty(price),
            isHasStartDate = startDate.HasValue,
            isHasEndDate = endDate.HasValue;

            try
            {
                searcher = new IndexSearcher(indexDirectory, true);

                BooleanQuery booleanQuery = new BooleanQuery();
              
                if (isHasQuery)
                {
                    var title = GetKeyWordsSplitBySpace(query);
                    Query query3 = new QueryParser(ver, "title", analyzer).Parse(title);
                    booleanQuery.Add(query3, Occur.MUST);
                }
               
                //按价格范围搜索(对数字搜索)
                if (isHasPrice)
                {
                    string[] prices = price.Split(new char[] { '-' });
                    float min = float.Parse(prices[0]);
                    float max = prices.Length < 2 ? 10000000000.00f : float.Parse(prices[1]);
                    Query query4 = NumericRangeQuery.NewFloatRange("price", min, max, true, true);
                    booleanQuery.Add(query4, Occur.MUST);
                }
         
                //按日期范围搜索(对日期搜索)
                if (isHasStartDate || isHasEndDate)
                {
                    string mindate = isHasStartDate ? startDate.Value.ToString("yyyy-MM-dd")
                        : DateTime.MinValue.ToString("yyyy-MM-dd");
                    string maxdate = isHasEndDate ? endDate.Value.ToString("yyyy-MM-dd")
                        : DateTime.Today.ToString("yyyy-MM-dd");
                    Query query5 = new TermRangeQuery("createdon", mindate, maxdate, true, true);
                    booleanQuery.Add(query5, Occur.MUST);
                }

                //如果没有查询关键字则显示全部的数据
                if (!isHasQuery && !isHasPrice && !isHasStartDate && !isHasEndDate)
                {
                    Query query6 = new TermQuery(new Term("all", "all"));
                    booleanQuery.Add(query6, Occur.MUST);
                }

                //执行搜索，获取查询结果集对象
                TopDocs topDocs = searcher.Search(booleanQuery, null, 100);

                recCount = topDocs.TotalHits;//获取命中的文档个数
                ScoreDoc[] hits = topDocs.ScoreDocs;//获取命中的文档信息对象

                stopWatch.Stop();//计时停止
                
                foreach (var item in hits)
                {
                    goodses.Add(new GoodsAlias
                    {
                        Id = new Guid(searcher.Doc(item.Doc).Get("id")),
                        Name = searcher.Doc(item.Doc).Get("title"),
                        Pics = searcher.Doc(item.Doc).Get("pics"),
                        Price = Convert.ToDecimal(searcher.Doc(item.Doc).Get("price")),
                        OriginalPrice = Convert.ToDecimal(searcher.Doc(item.Doc).Get("originalprice")),
                        Benevolence = Convert.ToDecimal(searcher.Doc(item.Doc).Get("benevolence")),
                        SellOut = Convert.ToInt32(searcher.Doc(item.Doc).Get("sellout")),
                        Rate = Convert.ToSingle(searcher.Doc(item.Doc).Get("rate")),
                        CreatedOn = Convert.ToDateTime(searcher.Doc(item.Doc).Get("createdon"))
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (searcher != null)
                {
                    searcher.Dispose();
                }
            }

            return goodses;
        }

        /// <summary>
        /// 索引存放目录
        /// </summary>
        protected string SearchIndexPath
        {
            get
            {
                var indexPath = @"C:\ShopSearchIndex";
                return System.IO.Path.GetFullPath(indexPath);
            }
        }

        /// <summary>
        /// 盘古分词的配置文件
        /// </summary>
        protected string PanGuXmlPath
        {
            get
            {
                return _hostingEnvironment.WebRootPath + "/lib/PanGu.xml";
            }
        }

        private string GetKeyWordsSplitBySpace(string keywords)
        {
            PanGuTokenizer ktTokenizer = new PanGuTokenizer();
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);

            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }
    }
}