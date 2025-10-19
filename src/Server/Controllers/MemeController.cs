using Domain.Interfaces.IMeme;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.Meme;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemeController : ControllerBase
    {
        private IMemeService _memeService;
        public MemeController(IMemeService memeService)
        {
            _memeService = memeService;
        }

        /// <summary>
        /// Получение данных всех мемов
        /// </summary>

        // GET api/<MemeController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _memeService.GetAll();
            var response = result.Adapt<List<GetMemeResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных мема по Id
        /// </summary>
        /// <param name="id">Id мема</param>

        // GET api/<MemeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _memeService.GetById(id);
            var response = result.Adapt<GetMemeResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание нового мема
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "userId": 1,
        ///        "title": "Funny Meme",
        ///        "description": "This is a description",
        ///        "imageUrl": "https://example.com/image.jpg",
        ///        "isPublic": true
        ///     }
        ///
        /// </remarks>
        /// /// <param name="meme">Добавляемые данные мема</param>

        // POST api/<MemeController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateMemeRequest meme)
        {
            var memeDto = meme.Adapt<Meme>();
            await _memeService.Create(memeDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных мема по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT
        ///     {
        ///        "userId": 1,
        ///        "title": "Updated Funny Meme",
        ///        "description": "Updated description",
        ///        "imageUrl": "https://example.com/updated_image.jpg",
        ///        "isPublic": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id мема</param>
        /// <param name="memeRequest">Обновленные данные мема</param>

        // PUT api/<MemeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateMemeRequest memeRequest)
        {
            var existingMeme = await _memeService.GetById(id);
            if (existingMeme == null)
                return NotFound();
            if (memeRequest.UserId != 0)
                existingMeme.UserId = memeRequest.UserId;
            if (!string.IsNullOrEmpty(memeRequest.Title))
                existingMeme.Title = memeRequest.Title;
            if (!string.IsNullOrEmpty(memeRequest.Description))
                existingMeme.Description = memeRequest.Description;
            if (!string.IsNullOrEmpty(memeRequest.ImageUrl))
                existingMeme.ImageUrl = memeRequest.ImageUrl;
            if (memeRequest.IsPublic != null)
                existingMeme.IsPublic = memeRequest.IsPublic;
            await _memeService.Update(existingMeme);
            return Ok();
        }

        /// <summary>
        /// Удаление мема по Id
        /// </summary>
        /// <param name="id">Id мема</param>

        // DELETE api/<MemeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memeService.Delete(id);
            return Ok();
        }
    }
}