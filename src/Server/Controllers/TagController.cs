using Domain.Interfaces.ITag;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.Tag;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Получение данных всех тегов
        /// </summary>

        // GET api/<TagController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tagService.GetAll();
            var response = result.Adapt<List<GetTagResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных тега по Id
        /// </summary>
        /// <param name="id">Id тега</param>

        // GET api/<TagController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tagService.GetById(id);
            var response = result.Adapt<GetTagResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание нового тега
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "tagName": "New Tag Name",
        ///     }
        ///
        /// </remarks>
        /// <param name="tag">Данные тега</param>

        // POST api/<TagController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateTagRequest tag)
        {
            var tagDto = tag.Adapt<Tag>();
            await _tagService.Create(tagDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных тега по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "tagName": "New Tag Name",
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id тега</param>
        /// <param name="tagRequest">Обновленные данные тега</param>

        // PUT api/<TagController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateTagRequest tagRequest)
        {
            var existingTag = await _tagService.GetById(id);
            if (existingTag == null)
                return NotFound();
            if (!string.IsNullOrEmpty(tagRequest.TagName))
                existingTag.TagName = tagRequest.TagName;
            await _tagService.Update(existingTag);
            return Ok();
        }

        /// <summary>
        /// Удаление тега по Id
        /// </summary>
        /// <param name="id">Id тега</param>

        // DELETE api/<TagController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.Delete(id);
            return Ok();
        }
    }
}