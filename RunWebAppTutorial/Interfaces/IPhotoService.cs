using CloudinaryDotNet.Actions;

namespace RunWebAppTutorial.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
