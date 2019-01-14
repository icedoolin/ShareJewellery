using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cstc.ShareJewlryApp.Tools
{
    public interface Ihud 
    {
        void Show_StatusMessage(string StatusMessage);   //弹出加载框
        void Show_Success(string Message);     //弹出有成功标识的弹窗
        void Show_Error(string Message);      //弹出有错误标识的弹窗
        void Show_Toast(string Message);      //弹出带有文字的弹窗
        void Dismiss();                       //关闭弹窗
    }
}
