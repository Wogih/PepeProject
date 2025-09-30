using Domain.Interfaces.IComment;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.Comment;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Получение данных всех коментариев
        /// </summary>

        // GET api/<CommentController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _commentService.GetAll();
            var response = result.Adapt<List<GetCommentResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных комментария по Id
        /// </summary>
        /// <param name="id">Id комментария</param>

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _commentService.GetById(id);
            var response = result.Adapt<GetCommentResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание нового комментария
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "memeId": 1,
        ///        "userId": 1,
        ///        "commentText": "Comment Text",
        ///        "parentCommentId": 1
        ///     }
        /// </remarks>
        /// <param name="comment">Данные нового комментария</param>

        // POST api/<CommentController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateCommentRequest comment)
        {
            var commentDto = comment.Adapt<Comment>();
            await _commentService.Create(commentDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных комментария по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "memeId": 1,
        ///        "userId": 1,
        ///        "commentText": "Comment Text",
        ///        "parentCommentId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id комментария</param>
        /// <param name="commentRequest">Обновленные данные комментария</param>

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCommentRequest commentRequest)
        {
            var existingComments = await _commentService.GetById(id);
            if (existingComments == null)
                return NotFound();
            if (commentRequest.MemeId != 0)
                existingComments.MemeId = commentRequest.MemeId;
            if (commentRequest.UserId != 0)
                existingComments.UserId = commentRequest.UserId;
            if (!string.IsNullOrEmpty(commentRequest.CommentText))
                existingComments.CommentText = commentRequest.CommentText;
            if (commentRequest.ParentCommentId != 0)
                existingComments.ParentCommentId = commentRequest.ParentCommentId;
            await _commentService.Update(existingComments);
            return Ok();
        }

        /// <summary>
        /// Удаление комментария по Id
        /// </summary>
        /// <param name="id">Id комментария</param>

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.Delete(id);
            return Ok();
        }
    }
}