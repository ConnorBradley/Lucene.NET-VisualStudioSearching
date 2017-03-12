using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net;
using System.IO;
using System.Net;
using EnvDTE;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Document = EnvDTE.Document;
using Version = Lucene.Net.Util.Version;

namespace RapidSearch
{
    public static class FileIndexer
    {

        public static Lucene.Net.Store.Directory CreateIndex()
        {
            var directory = new Lucene.Net.Store.RAMDirectory();
            var dte = DTEManager.getCurrentDTE();

            var content = ConvertTextToDataTable(GetSolutionDirectory(dte));
            

            using (Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30))
            using (var writer = new IndexWriter(directory, analyzer, new IndexWriter.MaxFieldLength(1000)))
            {
                foreach (DataRow row in content.Rows)
                {
                  
                    var document = new Lucene.Net.Documents.Document();

                    document.Add(new Field("path", row["FileName"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("contents", row["Text"].ToString(), Field.Store.YES, Field.Index.ANALYZED));

                    writer.AddDocument(document);
                }
  
                writer.Optimize();
                writer.Flush(true, true, true);
            }

            return directory;
        }

        private static DataTable ConvertTextToDataTable(string directory)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FileName", typeof(string));
            dt.Columns.Add("Text", typeof(string));

            DirectoryInfo d = new DirectoryInfo(directory);
            var files = GetAllFilesInFolder(directory);

            foreach (var file in files)
            {
                string contents = File.ReadAllText(file.FullName);
                dt.Rows.Add(file.FullName, contents);
            }

            return dt;

        }


        private static List<FileInfo> GetAllFilesInFolder(string directory, List<FileInfo> listToAppend = default(List<FileInfo>))
        {
            if (listToAppend == default(List<FileInfo>))
            {
                listToAppend = new List<FileInfo>();
            }
               
            foreach (var d in System.IO.Directory.GetDirectories(directory))
            {
                foreach (var f in System.IO.Directory.GetFiles(d))
                {
                    FileInfo file = new FileInfo(f);
                    listToAppend.Add(file);
                }
                GetAllFilesInFolder(d, listToAppend);
            }
            return listToAppend;
        }


        /// <summary>
        /// Gets all of the content of the current working directory.
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        private static string GetSolutionDirectory(DTE dte)
        {
            var unformattedDirectory = dte.Solution.FullName;
            List<string> files = new List<string>();
            return !string.IsNullOrEmpty(unformattedDirectory) ? unformattedDirectory.Remove(unformattedDirectory.LastIndexOf("\\", StringComparison.Ordinal)) : "error";
        }

    }
}
