using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFFarsi
{
    public class RTLUtil
    {
        public static string MirrorNum(string s)
        {
            List<MyWord> ls = new List<MyWord>();
            string r = string.Empty;

            foreach (var item in s.Split(' ').ToList())
            {
                MyWord mw = new MyWord();
                mw.myWord = item;
                ls.Add(mw);
            }

            foreach (var str in ls)
	        {
		        r += " " + str.myWord;
	        }

            return r;

        }


    }
}
