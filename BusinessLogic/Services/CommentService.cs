using Domain.Interfaces;
using Domain.Interfaces.IComment;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public CommentService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Comment>> GetAll()
        {
            return await _repositoryWrapper.Comment.FindAll();
        }

        public async Task<Comment> GetById(int id)
        {
            var comments = await _repositoryWrapper.Comment
                .FindByCondition(x => x.CommentId == id);

            if (!comments.Any())
            {
                throw new InvalidOperationException("Comment not found.");
            }

            if (comments.Count > 1)
            {
                throw new InvalidOperationException("Multiple comments found with the same ID.");
            }

            return comments.First();
        }

        public async Task<List<Comment>> GetByMemeId(int memeId)
        {
            return await _repositoryWrapper.Comment
                .FindByCondition(x => x.MemeId == memeId);
        }

        public async Task<List<Comment>> GetReplies(int parentCommentId)
        {
            return await _repositoryWrapper.Comment
                .FindByCondition(x => x.ParentCommentId == parentCommentId);
        }

        public async Task Create(Comment model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (string.IsNullOrWhiteSpace(model.CommentText))
            {
                throw new ArgumentException("CommentText cannot be null, empty, or whitespace.", nameof(model.CommentText));
            }

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            if (model.ParentCommentId.HasValue)
            {
                var parentComment = await _repositoryWrapper.Comment
                    .FindByCondition(x => x.CommentId == model.ParentCommentId.Value);
                
                if (!parentComment.Any())
                {
                    throw new InvalidOperationException("Parent comment not found.");
                }
            }

            await _repositoryWrapper.Comment.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Comment model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (string.IsNullOrWhiteSpace(model.CommentText))
            {
                throw new ArgumentException("CommentText cannot be null, empty, or whitespace.", nameof(model.CommentText));
            }

            if (model.MemeId <= 0)
            {
                throw new ArgumentException("MemeId must be greater than 0.", nameof(model.MemeId));
            }

            if (model.UserId <= 0)
            {
                throw new ArgumentException("UserId must be greater than 0.", nameof(model.UserId));
            }

            await _repositoryWrapper.Comment.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var comments = await _repositoryWrapper.Comment
                .FindByCondition(x => x.CommentId == id);
            
            if (!comments.Any())
            {
                throw new InvalidOperationException("Comment not found.");
            }

            if (comments.Count > 1)
            {
                throw new InvalidOperationException("Multiple comments found with the same ID.");
            }

            var replies = await _repositoryWrapper.Comment
                .FindByCondition(x => x.ParentCommentId == id);
            
            if (replies.Any())
            {
                throw new InvalidOperationException("Cannot delete comment that has replies. Delete replies first.");
            }

            await _repositoryWrapper.Comment.Delete(comments.First());
            await _repositoryWrapper.Save();
        }
    }
}