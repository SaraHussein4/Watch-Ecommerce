using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.FileProviders;

namespace Watch_Ecommerce.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider _fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
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

    }
}
