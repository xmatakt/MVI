using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Interfaces
{
    public interface IFileController
    {
        void DownloadFileAsync(string url, string fileName);
        string GetFileString(string fileName);
        bool FileExists(string fileName);
    }
}
