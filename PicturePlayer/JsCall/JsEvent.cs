using System;
using System.Text;
using System.IO;

using Xilium.CefGlue;

namespace PicturePlayer.JsCall
{
    public class JsEvent
    {
        public void OpenFolderDialog()
        {
            CefProcessMessage m = CefProcessMessage.Create("OpenFolderDialog");

            DemoRenderProcessHandler._browser.SendProcessMessage(CefProcessId.Browser, m);
        }

        public string GetImages(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception("path 不能为 空 。");

            string[] urlList = Directory.GetFiles(path);//, "(*.bmp|*.jpg|*.jpeg|*.gif|*.png)");

            for (int i=0; i<urlList.Length; i++)
            {
                urlList[i] = "http://" + ToLocalFileUrl(urlList[i]);
            }

            return ToJson(urlList);
        }

        private string ToLocalFileUrl(string path)
        {
            return "localfile/" + path.Replace(":", string.Empty).Replace("\\", "/");
        }

        private string ToJson(string[] arr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            for (int i=0; i<arr.Length; i++)
            {
                sb.Append("'");
                sb.Append(arr[i]);
                sb.Append("',");
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
