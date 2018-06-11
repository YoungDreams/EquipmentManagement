using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Foundation.Core
{
    public static class FileService
    {
        private static string _folder;

        public static void Initialize(string folder)
        {
            if (_folder != null)
            {
                throw new InvalidOperationException("FileService already has been initialized.");
            }

            _folder = folder;
        }

        public static void WriteFile(string filePath, byte[] content)
        {
            var fullPath = Path.Combine(_folder, filePath);

            File.WriteAllBytes(fullPath,content);
        }
    }
}
