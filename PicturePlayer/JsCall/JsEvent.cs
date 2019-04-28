using System;
using System.Collections.Generic;
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

            string[] urlList = Directory.GetFiles(path);

            string url;
            string ext;
            List<string> urlList2 = new List<string>();

            for (int i=0; i<urlList.Length; i++)
            {
                url = urlList[i];
                ext = Path.GetExtension(url);

                if (ext == ".bmp" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".png")
                    urlList2.Add("http://" + ToLocalFileUrl(url));
            }

            return ToJson(urlList2);
        }

        private string ToLocalFileUrl(string path)
        {
            return "localfile/" + path.Replace(":", string.Empty).Replace("\\", "/");
        }

        private string ToJson(List<string> list)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            for (int i=0; i<list.Count; i++)
            {
                sb.Append("'");
                sb.Append(list[i]);
                sb.Append("',");
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
