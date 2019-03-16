using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PDFFarsi
{
    class MyWord
    {
        public enum MLang
        {
            EN = 0,
            FA = 1,
            NUM =2
        }

        private string myword;
        private MLang mylang;

        public MLang myLang { get { return mylang; } set { this.mylang = value; } }
        
        public string myWord
        {
            get
            {
                return this.myword;
            }
            set
            {
                if (getLang(value) == MLang.NUM)
                {
                    var vword = value.ToCharArray().Reverse();
                    this.myword = string.Join("", vword);
                    mylang = MLang.NUM;
                }                   
                else
                { //english
                    this.myword = value;
                    mylang = MLang.FA;
                }
            }
        }
        
        public MLang getLang(string word)
        {

            if (Regex.IsMatch(word, @"\d+"))
                return MLang.NUM;            
            else 
                return MLang.FA;

        }
    }
}
