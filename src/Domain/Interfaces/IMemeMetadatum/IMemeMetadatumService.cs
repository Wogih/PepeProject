using Domain.Models;

namespace Domain.Interfaces.IMemeMetadatum
{
    public interface IMemeMetadatumService
    {
        Task<List<MemeMetadatum>> GetAll();
        Task<MemeMetadatum> GetById(int id);
        Task Create(MemeMetadatum model);
        Task Update(MemeMetadatum model);
        Task Delete(int id);
    }
}