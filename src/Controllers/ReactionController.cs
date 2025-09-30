using Domain.Interfaces.IReaction;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.Reaction;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private IReactionService _reactionService;
        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        /// <summary>
        /// Получение данных всех реакций
        /// </summary>

        // GET api/<ReactionController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reactionService.GetAll();
            var response = result.Adapt<List<GetReactionResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных реакции по Id
        /// </summary>
        /// <param name="id">Id реакции</param>

        // GET api/<ReactionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _reactionService.GetById(id);
            var response = result.Adapt<GetReactionResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание новой реакции
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "memeId": 1,
        ///        "userId": 1,
        ///        "ReactionType": "dislike ('like', 'dislike', 'love', 'laugh', 'angry')",
        ///     }
        /// </remarks>
        /// <param name="reaction">Данные новой реакции</param>

        // POST api/<ReactionController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateReactionRequest reaction)
        {
            var reactionDto = reaction.Adapt<Reaction>();
            await _reactionService.Create(reactionDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных реакции по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "memeId": 1,
        ///        "userId": 1,
        ///        "ReactionType": "dislike ('like', 'dislike', 'love', 'laugh', 'angry')", 
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id реакции</param>
        /// <param name="commentRequest">Обновленные данные реакции</param>

        // PUT api/<ReactionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateReactionRequest commentRequest)
        {
            var existingReaction = await _reactionService.GetById(id);
            if (existingReaction == null)
                return NotFound();
            if (commentRequest.MemeId != 0)
                existingReaction.MemeId = commentRequest.MemeId;
            if (commentRequest.UserId != 0)
                existingReaction.UserId = commentRequest.UserId;
            if (!string.IsNullOrEmpty(commentRequest.ReactionType))
                existingReaction.ReactionType = commentRequest.ReactionType;
            await _reactionService.Update(existingReaction);
            return Ok();
        }

        /// <summary>
        /// Удаление реакции по Id
        /// </summary>
        /// <param name="id">Id реакции</param>

        // DELETE api/<ReactionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _reactionService.Delete(id);
            return Ok();
        }
    }
}