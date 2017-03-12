using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using Microsoft.Build.Framework.XamlTypes;
using Version = Lucene.Net.Util.Version;

namespace RapidSearch
{
    public class FileSearcher
    {
        public DataTable Search(string textToSearch)
        {
            
            var index = FileIndexer.CreateIndex();
            DataTable dt = new DataTable();
            using (Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            using (var reader = IndexReader.Open(index, true))
            using (var searcher = new IndexSearcher(reader))
            {
                var parsedQuery = new QueryParser(Version.LUCENE_30, "content", analyzer).Parse(textToSearch);


                var collector = TopScoreDocCollector.Create(1000, true);

                searcher.Search(parsedQuery, collector);
                var matches = collector.TopDocs().ScoreDocs;

                foreach (var item in matches)
                {
                    var id = item.Doc;
                    var doc = searcher.Doc(id);

                    var row = dt.NewRow();

                    row["FileName"] = doc.GetField("path").StringValue;
                    row["Text"] = doc.GetField("contents").StringValue;

                    dt.Rows.Add(row);
                }
                
            }

            return dt;
        }
    }
}
