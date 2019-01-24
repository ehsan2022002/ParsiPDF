using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFFarsi
{
    public class ReadAndWrite
    {

        public void WritePdf(string filePath,string textdocument)
        {
            using (var document = new Document(PageSize.A4))
            {
                var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                FontFactory.Register("c:\\windows\\fonts\\tahoma.ttf");
                var tahoma = FontFactory.GetFont("tahoma", BaseFont.IDENTITY_H);

                ColumnText.ShowTextAligned(
                            canvas: writer.DirectContent,
                            alignment: Element.ALIGN_CENTER,
                            phrase: new Phrase(textdocument, tahoma),
                            x: 100,
                            y: 100,
                            rotation: 0,
                            runDirection: PdfWriter.RUN_DIRECTION_RTL,
                            arabicOptions: 0);
            }
            
        }

        public string ReadPdf(string filePath)
        {
            var reader = new PdfReader(filePath);
            int intPageNum = reader.NumberOfPages;
            string sOut = string.Empty;


            for (int i = 1; i <= intPageNum; i++)
            {
                var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                text = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(text));
                text = new UnicodeCharacterPlacement
                {
                    Font = new System.Drawing.Font("Tahoma", 12)
                }.Apply(text);

                sOut += text;
            }
            reader.Close();
            return sOut;

        }


    }
}
