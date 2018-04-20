using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using PanGu;
using Quartz;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Linq;

namespace Shop.TimerTask.Jobs.SearchEngine
{
    public class CreateSearchIndexJob: IJob
    {
        private IGoodsQueryService _goodsQueryService;//Q 端

        public CreateSearchIndexJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _goodsQueryService = container.Resolve<IGoodsQueryService>();
            //初始化分词器
            Segment.Init(PanGuXmlPath);
        }

        public void Execute(IJobExecutionContext context)
        {
            CreateSearchIndex();
        }

        public void CreateSearchIndex()
        {
            //路径判断
            if(!System.IO.Directory.Exists(SearchIndexPath))
            {
                System.IO.Directory.CreateDirectory(SearchIndexPath);
            }

            Directory indexDirectory = FSDirectory.Open(new System.IO.DirectoryInfo(SearchIndexPath));
            var analyzer = new PanGuAnalyzer();
            IndexWriter writer = null;

            try
            {
                //重新创建索引
                bool isCreate = true;// !IndexReader.IndexExists(indexDirectory);
                writer = new IndexWriter(indexDirectory, analyzer, isCreate, IndexWriter.MaxFieldLength.UNLIMITED);

                //所有的上架已审核的商品
                var goodses = _goodsQueryService.Goodses().Where(
                    g => g.IsPublished && g.Status == GoodsStatus.Verifyed);

                //开始添加索引
                foreach (var goods in goodses)
                {
                    AddIndex(writer, goods);
                }
                writer.Optimize();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (analyzer != null)
                    analyzer.Close();
                if (writer != null)
                    writer.Dispose();
                if (indexDirectory != null)
                    indexDirectory.Dispose();
            }
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
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"lib/PanGu.xml";
            }
        }

        private void AddIndex(IndexWriter indexWriter,QueryServices.Dtos.GoodsDetails goods)
        {
            try
            {
                Document doc = new Document();
                //存储,不分词索引
                doc.Add(new Field("id", goods.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("pics", goods.Pics, Field.Store.YES, Field.Index.NOT_ANALYZED));
                //存储，分词索引
                doc.Add(new Field("title", goods.Name, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new NumericField("price", Field.Store.YES, false).SetFloatValue(Convert.ToSingle(goods.Price)));
                doc.Add(new NumericField("originalprice", Field.Store.YES, false).SetFloatValue(Convert.ToSingle(goods.OriginalPrice)));
                doc.Add(new NumericField("benevolence", Field.Store.YES, false).SetFloatValue(Convert.ToSingle(goods.Benevolence)));
                doc.Add(new NumericField("sellout", Field.Store.YES, false).SetIntValue(goods.SellOut));
                doc.Add(new NumericField("rate", Field.Store.YES, false).SetFloatValue(goods.Rate));
                //存储,不分词索引(该文档主要用于不给查询条件时显示所有文档)
                doc.Add(new Field("all", "all", Field.Store.NO, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("createdon", goods.CreatedOn.ToString("yyyy-MM-dd"), Field.Store.YES, Field.Index.NOT_ANALYZED));

                indexWriter.AddDocument(doc);
            }
            catch (System.IO.FileNotFoundException fnfe)
            {
                throw fnfe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
