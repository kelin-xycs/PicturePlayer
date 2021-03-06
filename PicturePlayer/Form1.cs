﻿using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Xilium.CefGlue;
using Xilium.CefGlue.WindowsForms;

namespace PicturePlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private CefWebBrowser browser;

        private void Form1_Load(object sender, EventArgs e)
        {

            string url = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), @"Htmls\Main.html");

            url = "local" + url.Replace(":", string.Empty);

            string debug = System.Configuration.ConfigurationManager.AppSettings["Debug"];

            if (!string.IsNullOrWhiteSpace(debug) && bool.Parse(debug) == true)
            {
                txtUrl.Text = url;
                panel1.Visible = true;
            }

            browser = new CefWebBrowser();

            browser.StartUrl = "localapp/" + this.GetType().Namespace + "/Htmls/Main.html";
            browser.Dock = DockStyle.Fill;

            browser.BackColor = Color.White;

            browser.ProcessMessageReceived += Browser_ProcessMessageReceived;

            panel2.Controls.Add(browser);
        }

        private void Browser_ProcessMessageReceived(Xilium.CefGlue.CefBrowser browser, Xilium.CefGlue.CefProcessId sourceProcess, Xilium.CefGlue.CefProcessMessage message)
        {
            if (message.Name == "OpenFolderDialog")
            {
                BeginInvoke(new Action(ShowFolderDialog));
            }
            else
            {
                this.BeginInvoke(new Action(() =>
                    { MessageBox.Show("未知的消息 from Render Process ，消息名字“" + message.Name + "”。"); }));
            }
        }

        private void ShowFolderDialog()
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;

            var path = folderBrowserDialog1.SelectedPath;

            path = path.Replace(@"\", @"\\");

            browser.Browser.GetMainFrame().ExecuteJavaScript("OpenFolder('" + path + "');", null, 0);
            
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var path = folderBrowserDialog1.SelectedPath;

            path = path.Replace(@"\", @"\\");

            browser.Browser.GetMainFrame().ExecuteJavaScript("OpenFolder('" + path + "');", null, 0);

            //  这里要把 FileName 重置一下，不然下次打开 对话框 时显示的是 上次带路径的文件名
            //folderBrowserDialog1. = "新建文本文档.txt";
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;

            browser.Browser.GetMainFrame().LoadUrl(url);
        }

        private void BtnDevTools_Click(object sender, EventArgs e)
        {
            var host = browser.Browser.GetHost();
            var wi = CefWindowInfo.Create();
            wi.SetAsPopup(IntPtr.Zero, "DevTools");
            host.ShowDevTools(wi, new DevToolsWebClient(), new CefBrowserSettings(), new CefPoint(0, 0));
        }

        private class DevToolsWebClient : CefClient
        {
        }

        private bool isBarExpanded = true;

        private void BtnManual_Click(object sender, EventArgs e)
        {
            if (isBarExpanded)
            {
                panel1.Height = 40;

                btnManual.Text = "︾ 展开";

                isBarExpanded = false;

                return;
            }

            panel1.Height = 212;

            btnManual.Text = "︽ 折叠";

            isBarExpanded = true;
        }

    }
}
