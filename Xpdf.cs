using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFFarsi
{
    public class Xpdf
    {

        public string ReadPDF(string pdfPath)
        {
            PDFLibNet.PDFWrapper wrapper = new PDFLibNet.PDFWrapper();

            wrapper.LoadPDF(pdfPath);
            int i = wrapper.PageCount;
            string page1Text =string.Empty;
            
            for (int j = 1; j <= i; j++)
            {
                page1Text += Environment.NewLine + wrapper.Pages[j].Text;
            }

            return page1Text;
        }
    }
}
