using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using FEINews.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System.Net;

[assembly: Xamarin.Forms.Dependency(typeof(FEINews.Droid.FileController))]
namespace FEINews.Droid
{
    class FileController : IFileController
    {
        public void DownloadFileAsync(string url, string fileName)
        {
            //var documentsPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);

            //foreach (var file in new System.IO.DirectoryInfo(documentsPath).GetFiles())
            //{
            //    var tmpFile = file;
            //}

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.GetEncoding("Windows-1250");
                webClient.DownloadStringAsync(new Uri(url));
                webClient.DownloadStringCompleted += (s, e) => 
                {
                    var text = e.Result; // get the downloaded text
                    var localPath = GetFilePath(fileName);
                   File.WriteAllText(localPath, text, Encoding.UTF8); // writes to local storage
                };
            }
        }

        public string GetFileString(string fileName)
        {
            var path = GetFilePath(fileName);
            if (File.Exists(path))
                return File.ReadAllText(path);

            return null;
        }

        public bool FileExists(string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        private string GetFilePath(string fileName)
        {
            var documentsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);
            if(!new DirectoryInfo(documentsPath).Exists)
                documentsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);

            return Path.Combine(documentsPath, fileName);
        }
    }
}