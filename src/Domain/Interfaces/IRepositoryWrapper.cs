using Domain.Interfaces.ICollection;
using Domain.Interfaces.ICollectionMeme;
using Domain.Interfaces.IComment;
using Domain.Interfaces.IMeme;
using Domain.Interfaces.IMemeMetadatum;
using Domain.Interfaces.IMemeTag;
using Domain.Interfaces.IReaction;
using Domain.Interfaces.IRole;
using Domain.Interfaces.ITag;
using Domain.Interfaces.IUploadStat;
using Domain.Interfaces.IUser;
using Domain.Interfaces.IUserRole;

namespace Domain.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IMemeRepository Meme { get; }
        ITagRepository Tag { get; }
        IRoleRepository Role { get; }
        ICommentRepository Comment { get; }
        IReactionRepository Reaction { get; }
        ICollectionRepository Collection { get; }
        IMemeMetadatumRepository MemeMetadatum { get; }
        IUploadStatRepository UploadStat { get; }
        ICollectionMemeRepository CollectionMeme { get; }
        IMemeTagRepository MemeTag { get; }
        IUserRoleRepository UserRole { get; }
        Task Save();
    }
}