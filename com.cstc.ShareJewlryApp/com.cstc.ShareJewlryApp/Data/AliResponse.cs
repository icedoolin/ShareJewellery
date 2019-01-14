using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Data
{
    public  class AliResponse
    {
        public memostring memo { get; set; }
        public string RequestType { get; set; }
    }

   public class memostring
    {
        public string result { get; set; }
        public string ResultStatus { get; set; }
        public string memo { get; set; }
    }

   
}
