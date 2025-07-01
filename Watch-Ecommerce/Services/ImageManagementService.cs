using Castle.Components.DictionaryAdapter.Xml;
using ECommerce.Core.model;
using Microsoft.Extensions.FileProviders;

namespace Watch_Ecommerce.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider _fileProvider;
        private readonly TikrContext con;

        public ImageManagementService(IFileProvider fileProvider,TikrContext con)
        {
            _fileProvider = fileProvider;
            this.con = con;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            List<string> SaveImageSrc = new List<string>();
            var ImageDirectory = Path.Combine("wwwroot", "Images", src);
            if (Directory.Exists(ImageDirectory) is not true)
            {
                Directory.CreateDirectory(ImageDirectory);
            }

            foreach (IFormFile item in files)
            {
                if (item.Length > 0)
                {
                    string ImageName = item.FileName;
                    string ImageSrc = $"/Images/{src}/{ImageName}";
                    string root = Path.Combine(ImageDirectory, ImageName);

                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImageSrc);
                }
            }
            return SaveImageSrc;
        }
        //multiable images
        public async Task<List<string>> SaveImagesAsync(List<IFormFile> files, string folderName)
        {
            List<string> savedPaths = new List<string>();
            var uploadsFolder = Path.Combine("wwwroot", "Images", folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                savedPaths.Add($"/Images/{folderName}/{fileName}");
            }

            return savedPaths;
        }
        //delete image
        public async Task<bool> DeleteImageAsynce(int imgId)
        {
            var image = await con.Images.FindAsync(imgId);
            if (image == null) return false;
            var filePath = Path.Combine("wwwroot", image.Url.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            con.Images.Remove(image);
            await con.SaveChangesAsync();
            return true;
        }
    }
}
