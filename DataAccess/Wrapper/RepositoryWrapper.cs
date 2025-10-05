using DataAccess.Repositories;
using Domain.Interfaces;
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

namespace DataAccess.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private MisContext _repoContext;

        private IUserRepository _user;
        public IUserRepository User
        {
            get
            {
                _user ??= new UserRepository(_repoContext);
                return _user;
            }
        }

        private IMemeRepository _meme;
        public IMemeRepository Meme
        {
            get
            {
                _meme ??= new MemeRepository(_repoContext);
                return _meme;
            }
        }

        private ITagRepository _tag;
        public ITagRepository Tag
        {
            get
            {
                _tag ??= new TagRepository(_repoContext);
                return _tag;
            }
        }

        private IRoleRepository _role;
        public IRoleRepository Role
        {
            get
            {
                _role ??= new RoleRepository(_repoContext);
                return _role;
            }
        }

        private ICommentRepository _comment;
        public ICommentRepository Comment
        {
            get
            {
                _comment ??= new CommentRepository(_repoContext);
                return _comment;
            }
        }

        private IReactionRepository _reaction;
        public IReactionRepository Reaction
        {
            get
            {
                _reaction ??= new ReactionRepository(_repoContext);
                return _reaction;
            }
        }

        private ICollectionRepository _collection;
        public ICollectionRepository Collection
        {
            get
            {
                _collection ??= new CollectionRepository(_repoContext);
                return _collection;
            }
        }

        private IMemeMetadatumRepository _memeMetadatum;

        public IMemeMetadatumRepository MemeMetadatum
        {
            get
            {
                _memeMetadatum ??= new MemeMetadatumRepository(_repoContext);
                return _memeMetadatum;
            }
        }

        private IUploadStatRepository _uploadStats;
        public IUploadStatRepository UploadStat
        {
            get
            {
                _uploadStats ??= new UploadStatRepository(_repoContext);
                return _uploadStats;
            }
        }

        private ICollectionMemeRepository _collectionMeme;

        public ICollectionMemeRepository CollectionMeme
        {
            get
            {
                _collectionMeme ??= new CollectionMemeRepository(_repoContext);
                return _collectionMeme;
            }
        }

        private IMemeTagRepository _memeTag;
        public IMemeTagRepository MemeTag
        {
            get
            {
                _memeTag ??= new MemeTagRepository(_repoContext);
                return _memeTag;
            }
        }

        private IUserRoleRepository _userRole;

        public IUserRoleRepository UserRole
        {
            get
            {
                _userRole ??= new UserRoleRepository(_repoContext);
                return _userRole;
            }
        }

        public RepositoryWrapper(MisContext repoContext)
        {
            _repoContext = repoContext;
        }
        public async Task Save()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}