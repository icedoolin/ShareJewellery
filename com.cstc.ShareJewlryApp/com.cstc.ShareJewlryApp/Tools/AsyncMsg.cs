using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    //AM类
    public class AsyncMsg
    {
        public delegate void AsyncMsgEventHandler(object tag, string e);

        public event AsyncMsgEventHandler Completion;
        public event AsyncMsgEventHandler Cancel;

        public bool IsCompletion = false;//是否完成
        public bool IsCancel = false;//是否取消
        public string Tag { get; set; } = "";
        public object TagObj { get; set; } = "";
        public void OnCompletion(object tag, string e)
        {
            if (Completion != null)
            {
                Completion(tag, e);
                IsCompletion = true;
            }
        }
        public void OnCompletion()
        {
            OnCompletion(null, "");
        }
        public void OnCancel(object tag, string e)
        {
            if (Cancel != null)
            {
                Cancel(tag, e);
                IsCancel = true;
            }
        }
        public void OnCancel()
        {
            OnCancel(null, "");
        }
    }
}
