using Domain.Models;

namespace Domain.Interfaces.IComment
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAll();
        Task<Comment> GetById(int id);
        Task Create(Comment model);
        Task Update(Comment model);
        Task Delete(int id);
    }
}