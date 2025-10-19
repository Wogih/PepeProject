using Domain.Models;

namespace Domain.Interfaces.IUploadStat
{
    public interface IUploadStatService
    {
        Task<List<UploadStat>> GetAll();
        Task<UploadStat> GetById(int id);
        Task Create(UploadStat model);
        Task Update(UploadStat model);
        Task Delete(int id);
    }
}