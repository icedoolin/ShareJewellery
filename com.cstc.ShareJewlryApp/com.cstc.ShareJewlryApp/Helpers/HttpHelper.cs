using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.cstc.ShareJewlryApp.Helpers
{
    public class HttpHelper
    {

        
        public static string OSName
        {
            get
            {
                if (Xamarin.Forms.Device.RuntimePlatform.ToString() == Xamarin.Forms.Device.Android)
                {
                    return "Android";
                }
                else if (Xamarin.Forms.Device.RuntimePlatform.ToString() == Xamarin.Forms.Device.iOS)
                {
                    return "IOS";
                }
                return "";
            }
        }

        public static void AddPostJsonString(ref string json, Dictionary<string, string> values)
        {
            if (values == null || values.Count == 0)
                return;

            string line = "";
            foreach (var value in values)
            {
                if (value.Key == null || value.Key.Trim() == "")
                    continue;

                string 键值配对 = @"""" + value.Key + @"""" + ":" + @"""" + (value.Value == null ? "" : value.Value) + @"""";


                if (value.Value.ToString().StartsWith("["))
                {
                    键值配对 = @"""" + value.Key + @"""" + ":" + (value.Value == null ? "" : value.Value);
                }
                else
                {
                    键值配对 = @"""" + value.Key + @"""" + ":" + @"""" + (value.Value == null ? "" : value.Value) + @"""";
                }

                if (line == "")
                {
                    line += 键值配对;
                }
                else
                {
                    line += "," + 键值配对;
                }
            }

            line = "{" + line + "}";

            if (json == "")
            {
                json = line;
            }
            else
            {
                json = json + "," + line;
            }
        }



        public static void DeleteImgByUrl(string FilePath, string FileName, AsyncMsg am)
        {
            string url = MConfig.dbUrl + "DeleteFile?FileName=" + FileName.ToUpper() + "&FilePath=" + FilePath.ToUpper();
            System.Net.HttpWebRequest httpReq = null;
            try
            {
                //创建一个请求
                httpReq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
            }
            catch (System.Exception ex)
            {
                am.OnCancel(null, "DeleteImgByUrl中创建请求失败！" + ex.Message);
            }

            try
            {
                httpReq.BeginGetResponse(new AsyncCallback(asynCallback), new object[2] { httpReq, am });
            }
            catch (System.Exception ex)
            {
                am.OnCancel(null, "DeleteImgByUrl中获取响应失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <param name="am"></param>
        public static void DeleteImgByUrlForSmall(string FilePath, string FileName, AsyncMsg am)
        {
            string url =Helpers.MConfig.dbUrl + "Plugin_Text?ClassName=Plugin_SmallPicture.DeleteFile&FileName=" + FileName.ToUpper() + "&FilePath=" + FilePath.ToUpper();
            System.Net.HttpWebRequest httpReq = null;
            try
            {
                //创建一个请求
                httpReq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
            }
            catch (System.Exception ex)
            {
                am.OnCancel(null, "DeleteImgByUrl中创建请求失败！" + ex.Message);
            }

            try
            {
                httpReq.BeginGetResponse(new AsyncCallback(asynCallback), new object[2] { httpReq, am });
            }
            catch (System.Exception ex)
            {
                am.OnCancel(null, "DeleteImgByUrl中获取响应失败！" + ex.Message);
            }
        }

        public static void PostImgByUrl(string FilePath, string FileName, string postData, AsyncMsg am)
        {
            string url = MConfig.dbUrl + "UpLoadFile?FileName=" + FileName + "&FilePath=" + FilePath;

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "post";
                byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream((ar) =>
                {
                    var dataStream = (ar.AsyncState as WebRequest).EndGetRequestStream(ar);
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Dispose();
                    request.BeginGetResponse(new AsyncCallback((br) =>
                    {
                        WebRequest wreq = (WebRequest)br.AsyncState;

                        var response = (WebResponse)wreq.EndGetResponse(br);

                        //var response = (br.AsyncState as WebRequest).EndGetResponse(br) as WebResponse;

                        System.IO.Stream s = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(s);

                        string responseFromServer = reader.ReadToEnd();
                        am.OnCompletion(responseFromServer, "");

                    }
                    ), request);
                }, request);

            }
            catch (Exception ex)
            {
                am.OnCancel(null, ex.Message);

            }
        }

        public static void PostImgByUrlForSmall(string FilePath, string FileName, string postData, AsyncMsg am)
        {
            string url = MConfig.dbUrl + "UpLoadFile?FileName=" + FileName + "&FilePath=" + FilePath;

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "post";
                byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream((ar) =>
                {
                    var dataStream = (ar.AsyncState as WebRequest).EndGetRequestStream(ar);
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Dispose();
                    request.BeginGetResponse(new AsyncCallback((br) =>
                    {
                        WebRequest wreq = (WebRequest)br.AsyncState;

                        var response = (WebResponse)wreq.EndGetResponse(br);

                        //var response = (br.AsyncState as WebRequest).EndGetResponse(br) as WebResponse;

                        System.IO.Stream s = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(s);

                        string responseFromServer = reader.ReadToEnd();
                        am.OnCompletion(responseFromServer, "");



                    }
                    ), request);
                }, request);

            }
            catch (Exception ex)
            {
                am.OnCancel(null, ex.Message);

            }
        }


        public static void PostDataByUrl1(string postData, string url, AsyncMsg am)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "post";
                byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream((ar) =>
                {
                    var dataStream = (ar.AsyncState as WebRequest).EndGetRequestStream(ar);
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Dispose();
                    request.BeginGetResponse(new AsyncCallback((br) =>
                    {
                        WebRequest wreq = (WebRequest)br.AsyncState;

                        var response = (WebResponse)wreq.EndGetResponse(br);

                        //var response = (br.AsyncState as WebRequest).EndGetResponse(br) as WebResponse;

                        System.IO.Stream s = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(s);

                        string responseFromServer = reader.ReadToEnd();


                        am.OnCompletion(responseFromServer, "");

                    }
                    ), request);
                }, request);

            }
            catch (Exception ex)
            {
                am.OnCancel(null, ex.Message);

            }
        }

        public static void PostDataByUrlForPay(string postData, string url, AsyncMsg am)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "post";
                byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(postData);

                request.ContentType = "application/json";
                request.BeginGetRequestStream((ar) =>
                {
                    var dataStream = (ar.AsyncState as WebRequest).EndGetRequestStream(ar);
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Dispose();
                    request.BeginGetResponse(new AsyncCallback((br) =>
                    {
                        WebRequest wreq = (WebRequest)br.AsyncState;

                        var response = (WebResponse)wreq.EndGetResponse(br);

                        //var response = (br.AsyncState as WebRequest).EndGetResponse(br) as WebResponse;

                        System.IO.Stream s = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(s);

                        string responseFromServer = reader.ReadToEnd();


                        am.OnCompletion(responseFromServer, "");

                    }
                    ), request);
                }, request);

            }
            catch (Exception ex)
            {
                am.OnCancel(null, ex.Message);

            }
        }

        public static void PostDataByUrl(string sqlName, string postData, AsyncMsg am)
        {
            string url = MConfig.dbUrl + "ExSql?SqlCmdName=" + sqlName ;

            if (postData!="" && !postData.StartsWith("[") && !postData.EndsWith("]"))
                postData = "[" + postData + "]";
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "post";
                byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(postData);
                request.ContentType = "application/json";
                request.BeginGetRequestStream((ar) =>
                {
                    var dataStream = (ar.AsyncState as WebRequest).EndGetRequestStream(ar);
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Dispose();
                    request.BeginGetResponse(new AsyncCallback((br) =>
                    {
                        WebRequest wreq = (WebRequest)br.AsyncState;
                        var response = (WebResponse)wreq.EndGetResponse(br);
                        //var response = (br.AsyncState as WebRequest).EndGetResponse(br) as WebResponse;
                        System.IO.Stream s = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(s);
                        string responseFromServer = reader.ReadToEnd();
                        am.OnCompletion(responseFromServer, "");
                    }
                    ), request);
                }, request);
            }
            catch (Exception ex)
            {
                am.OnCancel(null, ex.Message);
            }
        }

 

        //public static void GetStringByUrlForPush(string ClientID, string Pushflag, AsyncMsg am)
        //{
        //    string url = PushUrl.Replace("{ClientID}", ClientID);

        //    url = url.Replace("{Pushflag}", Pushflag);
        //    HttpGet(url, am);
        //}

        public static void HttpGet(string url,   Helpers.AsyncMsg am)
        {
            Tools.Ihud hud =Xamarin.Forms.DependencyService.Get<Tools.Ihud>();
            System.Net.HttpWebRequest httpReq = null; 
            try
            {
                //创建一个请求
                httpReq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
            }
            catch (System.Exception ex)
            {
 
                am.OnCancel(null, "创建请求失败！" + ex.Message);
            }
            try
            {
                httpReq.BeginGetResponse(new AsyncCallback(asynCallback), new object[2] { httpReq, am });
            }
            catch (System.Exception ex)
            {
  
                am.OnCancel(null, "获取响应失败！" + ex.Message);
            }
        }

        public static void GetStringByUrl(string sqlName, string para, AsyncMsg am)
        {
            string url = MConfig.dbUrl+ "QueryData?SqlCmdName=" + sqlName + para;
            HttpGet(url, am);
        }

        /// <summary>
        /// 获取数据，并转换为对应类的列表
        /// </summary>
        /// <param name="am_return"></param>
        public static void GetDataItemList<T>(string sqlName, string para, Helpers.AsyncMsg am_return) where T : class
        {
            Helpers.AsyncMsg am_get = new Helpers.AsyncMsg();
            am_get.Completion += (object obj, string e) =>
            {
                string returnJson = obj.ToString();
                if (returnJson == "[]" || returnJson == "")  //如果没有数据
                {
                    am_return.OnCancel();
                    return;
                }
                object tempList;
                try
                {
                    tempList = Helpers.HttpHelper.GetItemList<T>(returnJson);
                    am_return.OnCompletion(tempList, "");
                }
                catch (Exception ex)
                {
                    am_return.OnCancel();
                    return;
                }

            };

            //获取
            Helpers.HttpHelper.GetStringByUrl(sqlName, para, am_get);
        }
        //public static void GetStringByUrlForLogitics(string cmd, string sqlName, string para, AsyncMsg am)
        //{
        //    if (para.Trim() == "")
        //    {

        //    }
        //    else
        //    {
        //        para = "&" + para;
        //    }
        //    string url = ServerUrl.Replace("{SQL}", sqlName + para);

        //    url = url.Replace("{CMD}", cmd);

        //    创建网络Get请求_logitics(url, am);


        //}

        public static void 创建网络Get请求_logitics(string url, Helpers.AsyncMsg am)
        {

            System.Net.HttpWebRequest httpReq = null;

            try
            {
                //创建一个请求
                httpReq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));

                //  System.Net.HttpWebResponse httpRes = (System.Net.HttpWebResponse)httpReq.EndGetResponse(null);
            }
            catch (System.Exception ex)
            {
                am.OnCancel(null, "创建请求失败！" + ex.Message);
            }

            try
            {
                httpReq.BeginGetResponse(new AsyncCallback(asynCallbackForLogitics), new object[2] { httpReq, am });
            }
            catch (System.Exception ex)
            {
                am.OnCancel(null, "获取响应失败！" + ex.Message);
            }
        }
        async private static void asynCallbackForLogitics(IAsyncResult asyn)
        {

            object[] objs = (object[])asyn.AsyncState;
            HttpWebRequest httpReq = (HttpWebRequest)objs[0];
            AsyncMsg am = (AsyncMsg)objs[1];
            //获取响应
            try
            {

                using (var httpRes = (HttpWebResponse)httpReq.EndGetResponse(asyn))
                {

                    //判断是否成功获取响应
                    if (httpRes.StatusCode == HttpStatusCode.OK)
                    {

                        //读取响应
                        StreamReader ResponseStream = null;
                        try
                        {
                            ResponseStream = new StreamReader(httpRes.GetResponseStream());
                        }
                        catch (System.Exception ex)
                        {
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取httpRes.GetResponseStream()异常：" + ex.Message);
                            return;
                        }
                        string json = "";
                        try
                        {
                            json = ResponseStream.ReadToEnd();
                        }
                        catch (System.Exception ex)
                        {
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取ResponseStream.ReadToEnd()异常：" + ex.Message);
                            return;
                        }
                        Newtonsoft.Json.Linq.JArray j_arr = null;
                        try
                        {
                            j_arr = await GetJArrayByJson(json);
                        }
                        catch (System.Exception ex)
                        {
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取GetJArrayByJson(text)异常：" + ex.Message + "json:" + (json == null ? "<null>" : json));
                            return;
                        }
                        try
                        {
                            string Return_OK = j_arr[0]["message"].ToString();
                            if (Return_OK == "ok" || Return_OK == "OK")
                            {
                                #region 正常返回
                                string tag = j_arr[0]["data"].ToString();
                                try
                                {
                                    am.OnCompletion(tag, "");
                                }
                                catch (System.Exception ex)
                                {
                                    am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()执行am.OnCompletion异常:" + ex.Message);
                                    return;
                                }
                                #endregion
                            }
                            else
                            {
                                string message = j_arr[0]["message"].ToString();
                                am.OnCancel(null, message);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取JSON结构异常:" + ex.Message + "JSON:" + json);
                            return;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()回调异常：" + ex.Message);
            }
        }
        async private static void asynCallback(IAsyncResult asyn)
        {
            Tools.Ihud hud = Xamarin.Forms.DependencyService.Get<Tools.Ihud>();
            object[] objs = (object[])asyn.AsyncState;
            HttpWebRequest httpReq = (HttpWebRequest)objs[0];
            AsyncMsg am = (AsyncMsg)objs[1];
            //获取响应
            try
            {
                using (var httpRes = (HttpWebResponse)httpReq.EndGetResponse(asyn))
                {
                    //判断是否成功获取响应
                    if (httpRes.StatusCode == HttpStatusCode.OK)
                    {
                        //读取响应
                        StreamReader ResponseStream = null;
                        try
                        {
                            ResponseStream = new StreamReader(httpRes.GetResponseStream());
                        }
                        catch (System.Exception ex)
                        {
                            hud.Show_Toast("网络不稳定，请检查您的网络设置");
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取httpRes.GetResponseStream()异常：" + ex.Message);
                            return;
                        }
                        string json = "";
                        try
                        {
                            json = ResponseStream.ReadToEnd();
                        }
                        catch (System.Exception ex)
                        {
                            hud.Show_Toast("网络不稳定，请检查您的网络设置");
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取ResponseStream.ReadToEnd()异常：" + ex.Message);
                            return;
                        }
                        Newtonsoft.Json.Linq.JArray j_arr = null;
                        try
                        {
                            j_arr = await GetJArrayByJson(json);
                        }
                        catch (System.Exception ex)
                        {
                            hud.Show_Toast("网络不稳定，请检查您的网络设置");
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取GetJArrayByJson(text)异常：" + ex.Message + "json:" + (json == null ? "<null>" : json));
                            return;
                        }
                        try
                        {
                            string Return_OK = j_arr[0]["OK"].ToString();
                            if (Return_OK == "True" || Return_OK == "true")
                            {
                                #region 正常返回
                                string tag = j_arr[0]["Tag"].ToString();
                                try
                                {
                                    am.OnCompletion(tag, "");
                                }
                                catch (System.Exception ex)
                                {
                                    hud.Show_Toast("网络不稳定，请检查您的网络设置");
                                    am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()执行am.OnCompletion异常:" + ex.Message);
                                    return;
                                }
                                #endregion
                            }
                            else
                            {
                                string message = j_arr[0]["Message"].ToString();
                                am.OnCancel(null, message);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            hud.Show_Toast("网络不稳定，请检查您的网络设置");
                            am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()获取JSON结构异常:" + ex.Message + "JSON:" + json);
                            return;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                hud.Show_Toast("网络不稳定，请检查您的网络设置");
                am.OnCancel(null, httpReq.RequestUri.ToString() + "之asynCallback()回调异常：" + ex.Message);
            }
        }

        public static Task<Newtonsoft.Json.Linq.JArray> GetJArrayByJson(string json)
        {
            if (json.StartsWith("["))
                json = json.Substring(1, json.Length - 1);
            if (json.EndsWith("]"))
                json = json.Substring(0, json.Length - 1);
            if (json.Replace(" ", "").Replace("\r", "").Replace("\n", "").StartsWith(@"{""Table"":["))
            {
                return Task.Factory.StartNew<Newtonsoft.Json.Linq.JArray>(() =>
                {
                    Newtonsoft.Json.Linq.JObject j_obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                    Newtonsoft.Json.Linq.JArray j_arr = Newtonsoft.Json.Linq.JArray.Parse(j_obj["Table"].ToString());
                    return j_arr;
                });
            }
            else
            {
                return Task.Factory.StartNew<Newtonsoft.Json.Linq.JArray>(() =>
                {
                    json = "{\"JObject\":[" + json + "]}";
                    Newtonsoft.Json.Linq.JObject j_obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                    Newtonsoft.Json.Linq.JArray j_arr = Newtonsoft.Json.Linq.JArray.Parse(j_obj["JObject"].ToString());
                    return j_arr;
                });
            }
        }

        public static Newtonsoft.Json.Linq.JArray JsonToJArray(string json)   //返回到所需格式
        {
            Newtonsoft.Json.Linq.JArray jar = null;
            Newtonsoft.Json.Linq.JObject j_obj = null;
            json = "{\"JObject\":[" + json + "]}";
            try
            {
                j_obj = Newtonsoft.Json.Linq.JObject.Parse(json);

                jar = Newtonsoft.Json.Linq.JArray.Parse(j_obj["JObject"].ToString());

                string Return_OK = jar[0]["OK"].ToString();
                if (Return_OK == "True" || Return_OK == "true")
                {
                    #region 正常返回
                    jar = Newtonsoft.Json.Linq.JArray.Parse(jar[0]["Tag"].ToString());
                    string s = jar.ToString().Trim();
                    if (jar.ToString().Replace(" ", "").Replace("\r", "").Replace("\n", "").Trim().Contains(@"""Table"":["))
                    {
                        jar = Newtonsoft.Json.Linq.JArray.Parse(jar[0]["Table"].ToString());
                        return jar;
                    }
                    else if (jar.ToString() == "[]")
                    {
                        return jar;
                    }
                    else
                    {
                        return null;
                    }
                    #endregion
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string tempMsg = ex.Message;
                return null;
            }

        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        public static List<T> GetItemList<T>(string tag) where T : class
        {
            List<T> lists = new List<T>();
            try
            {
                Newtonsoft.Json.Linq.JArray j_arr1 = HttpHelper.GetJArrayByJson1(tag);
                string aa = j_arr1.ToString();
                lists = HttpHelper.DeserializeJsonToList<T>(aa);

            }
            catch (Exception ex) { }
            return lists;
        }

        public static Newtonsoft.Json.Linq.JArray GetJArrayByJson1(string json)
        {
            if (json.StartsWith("["))
                json = json.Substring(1, json.Length - 1);
            if (json.EndsWith("]"))
                json = json.Substring(0, json.Length - 1);
            if (json.Replace(" ", "").Replace("\r", "").Replace("\n", "").StartsWith(@"{""Table"":["))
            {
                Newtonsoft.Json.Linq.JObject j_obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                Newtonsoft.Json.Linq.JArray j_arr = Newtonsoft.Json.Linq.JArray.Parse(j_obj["Table"].ToString());
                return j_arr;

            }
            else
            {
                json = "{\"JObject\":[" + json + "]}";
                Newtonsoft.Json.Linq.JObject j_obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                Newtonsoft.Json.Linq.JArray j_arr = Newtonsoft.Json.Linq.JArray.Parse(j_obj["JObject"].ToString());
                return j_arr;
            }
        }
    }
}
