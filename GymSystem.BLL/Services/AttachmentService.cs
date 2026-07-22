using GymSystem.BLL.Contracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services
{
    public class AttachmentService :IAttachmentService
    {
        private readonly int _maxImageSize = 5 *1024 * 1024;
        private readonly string[] _validExtensions = [".png", ".jpg", ".jpeg"];
        private IWebHostEnvironment _env { get; }

        public AttachmentService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> UploadAsync(Stream stream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (stream == null || !stream.CanRead) return null;
            if (stream.Length == 0 || stream.Length > _maxImageSize) return null;

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension) || !_validExtensions.Contains(extension)) return null;

            var uploadfolder = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory(uploadfolder);

            var storedFileName = $"{Guid.NewGuid()}{fileName}";
            var filePath = Path.Combine(uploadfolder, storedFileName);

            try
            {
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                await stream.CopyToAsync(fs , ct);
                return storedFileName;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            var filePath = Path.Combine(_env.ContentRootPath, folderName , fileName);

            try
            {
                if (!File.Exists(filePath)) return false;
                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName)) return null;

            var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(filePath)) return null;
            var stream = new FileStream(filePath, FileMode.Open , FileAccess.Read);

            var extension = Path.GetExtension(filePath).ToLower();
            var contenttype = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"   //bytes
            };
            return(stream, contenttype);
            
        }
    }
}
