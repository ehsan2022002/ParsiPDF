# ParsiPDF
read and write farsi text (adobe acrobat file)

برای خواندن فایلهای فارسی این کلاس ایجاد شده است
متن و تلفیق فارسی و انگلیسی و اعداد را میتواند تبدیل کند
در متنهای ترکیبی مشکل دارد
برای مثال یک پروژه ایجاد کنید و کد زیر را بنوسید 


            var x = new PDFFarsi.ReadAndWrite();
            
            var r = x.ReadPdf(@"C:\temp\Document1.pdf");
            
