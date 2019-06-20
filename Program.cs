using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //////

            Application word = new Application();
            Document doc = new Document();

            object fileName = @"C:\temp\20.pdf";

            // Define an object to pass to the API for missing parameters
            object missing = System.Type.Missing;
            doc = word.Documents.Open(ref fileName,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing);

            String read = string.Empty;
            List<string> data = new List<string>();


            //var lines = doc.SaveAs2()

            WdSaveFormat format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;


            fileName = @"C:\temp\21.doc";
            doc.SaveAs2(fileName, format);
            
           /*
            for (int i = 0; i < doc.Paragraphs.Count; i++)
            {
                string temp = doc.Paragraphs[i + 1].Range.Text.Trim();
                if (temp != string.Empty)
                    File.WriteAllText(@"C:\temp\" + i.ToString() + ".txt", temp); 
            }
           */

            ((_Document)doc).Close();
            ((_Application)word).Quit();
            
        }
    }
}
