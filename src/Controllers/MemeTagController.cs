using Domain.Interfaces.IMemeTag;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.MemeTag;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemeTagController : ControllerBase
    {
        private IMemeTagService _memeTagService;
        public MemeTagController(IMemeTagService memeTagService)
        {
            _memeTagService = memeTagService;
        }

        /// <summary>
        /// Получение данных всех мем->тег
        /// </summary>

        // GET api/<MemeTagController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _memeTagService.GetAll();
            var response = result.Adapt<List<GetMemeTagResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных мем->тег по Id
        /// </summary>
        /// <param name="id">Id мем->тег</param>

        // GET api/<MemeTagController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _memeTagService.GetById(id);
            var response = result.Adapt<GetMemeTagResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание нового мем->тег
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "memeId": 1,
        ///        "tagId": 1
        ///     }
        ///
        /// </remarks>
        /// /// <param name="memeTag">Добавляемые данные мем->тег</param>

        // POST api/<MemeTagController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateMemeTagRequest memeTag)
        {
            var memeTagDto = memeTag.Adapt<MemeTag>();
            await _memeTagService.Create(memeTagDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных мем->тег по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT
        ///     {
        ///        "memeId": 1,
        ///        "tagId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id мем->тег</param>
        /// <param name="memeTagRequest">Обновленные данные мем->тег</param>

        // PUT api/<MemeTagController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateMemeTagRequest memeTagRequest)
        {
            var existingMemeTag = await _memeTagService.GetById(id);
            if (existingMemeTag == null)
                return NotFound();
            if (memeTagRequest.TagId != 0)
                existingMemeTag.TagId = memeTagRequest.TagId;
            if (memeTagRequest.MemeId != 0)
                existingMemeTag.MemeId = memeTagRequest.MemeId;
            await _memeTagService.Update(existingMemeTag);
            return Ok();
        }

        /// <summary>
        /// Удаление мем->тег по Id
        /// </summary>
        /// <param name="id">Id мем->тег</param>

        // DELETE api/<MemeTagController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memeTagService.Delete(id);
            return Ok();
        }
    }
}