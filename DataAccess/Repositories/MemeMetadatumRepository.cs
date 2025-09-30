using Domain.Interfaces.IMemeMetadatum;
using Domain.Models;

namespace DataAccess.Repositories
{
    internal class MemeMetadatumRepository : RepositoryBase<MemeMetadatum>, IMemeMetadatumRepository
    {
        public MemeMetadatumRepository(MisContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}