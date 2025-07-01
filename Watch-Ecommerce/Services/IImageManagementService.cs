namespace Watch_Ecommerce.Services
{
    public interface IImageManagementService
    {
        Task<List<string>> AddImageAsync(IFormFileCollection files, string src);
        Task<List<string>> SaveImagesAsync(List<IFormFile> files, string folderName);
      Task<bool> DeleteImageAsynce(int imgId);

    }
}
