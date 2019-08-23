using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace eCommerceDotNet.Helpers
{

    public static class FileHelper
    {
        public static string UPLOAD_ROOT = "wwwroot";
        public static string UPLOAD_PATH = "images";

        public static string UploadFile(IFormFile file)
        {
            if (file == null) return null;

            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = Convert.ToString(Guid.NewGuid()) + fileExtension;

            string relativePath = Path.Combine(UPLOAD_PATH, uniqueFileName);
            string fullPath = Path.Combine(UPLOAD_ROOT,UPLOAD_PATH, uniqueFileName);
            using (FileStream fs = File.Create(fullPath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return relativePath;
        }

        public static bool CheckFileType(IFormFile file, params string[] expectedExtensions)
        {
            if (file == null) return true;
            string extension = Path.GetExtension(file.FileName);
            foreach (var ex in expectedExtensions)
            {
                if (extension == string.Format(".{0}", ex)) return true;
            }
            return false;
        }

        public static void DeleteFile(string relativePath)
        {
            string fullPath = Path.Combine(UPLOAD_ROOT, relativePath);
            File.Delete(fullPath);
        }
    }
}
