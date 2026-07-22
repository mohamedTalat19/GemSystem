using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Contracts
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsync(Stream stream , string fileName , string folderName , CancellationToken ct = default); 

        bool Delete(string fileName , string folderName);

        (Stream stream, string contentType)? GetFile(string fileName , string folderName);
    }
}
