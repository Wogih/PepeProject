using Domain.Interfaces.IUploadStat;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class UploadStatRepository : RepositoryBase<UploadStat>, IUploadStatRepository
    {
        public UploadStatRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}