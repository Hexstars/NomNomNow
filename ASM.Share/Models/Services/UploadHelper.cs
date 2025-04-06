using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models.Services
{
    public interface IUploadHelper
    {
        Task UploadImage(IFormFile file, string rootPath, string phanloai);
    }
    public class UploadHelper : IUploadHelper
    {
        public async Task UploadImage(IFormFile file, string rootPath, string phanloai)
        {
            // Ensure the root path exists
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            string dirPath = Path.Combine(rootPath, phanloai);

            // Ensure the directory for the category exists
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Define the path to save the uploaded file
            string filePath = Path.Combine(dirPath, file.FileName);

            // Check if the file already exists, if not, create the file
            if (!File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    // Use OpenReadStream to get the stream from IBrowserFile
                    await file.OpenReadStream().CopyToAsync(stream);
                }
            }
        }
    }
}
