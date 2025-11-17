using API.DTOs;
using API.Entities;

namespace API.Interface
{
    public interface IPhotoRepository
    {
        Task<IReadOnlyList<PhotoForApprovalDto>> GetUnapprovedPhotos();
        Task<Photo?> GetPhotoById(int id);
        void RemovePhoto(Photo photo);
    }
}
