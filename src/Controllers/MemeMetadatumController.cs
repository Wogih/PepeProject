using Domain.Interfaces.IMemeMetadatum;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.MemeMetadatum;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemeMetadatumController : ControllerBase
    {
        private IMemeMetadatumService _memeMetadatumService;
        public MemeMetadatumController(IMemeMetadatumService memeMetadatumService)
        {
            _memeMetadatumService = memeMetadatumService;
        }

        /// <summary>
        /// Получение данных всех методат 
        /// </summary>

        // GET api/<MemeMetadataController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _memeMetadatumService.GetAll();
            var response = result.Adapt<List<GetMemeMetadatumResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных методаты по Id
        /// </summary>
        /// <param name="id">Id методаты</param>

        // GET api/<MemeMetadataController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _memeMetadatumService.GetById(id);
            var response = result.Adapt<GetMemeMetadatumResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание новой методаты
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "memeId": 1,
        ///        "fileSize": 2048576,
        ///        "width": 1920,
        ///        "height": 1080,
        ///        "fileFormat": "JPEG",
        ///        "mimeType": "image/jpeg"
        ///     }
        /// </remarks>
        /// <param name="memeMetadatum">Данные новой методаты</param>

        // POST api/<MemeMetadataController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateMemeMetadatumRequest memeMetadatum)
        {
            var memeMetadatumDto = memeMetadatum.Adapt<MemeMetadatum>();
            await _memeMetadatumService.Create(memeMetadatumDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных методаты по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT
        ///     {
        ///        "memeId": 1,
        ///        "fileSize": 2048576,
        ///        "width": 1920,
        ///        "height": 1080,
        ///        "fileFormat": "JPEG",
        ///        "mimeType": "image/jpeg"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id методаты</param>
        /// <param name="memeMetadatumRequest">Обновленные данные методаты</param>

        // PUT api/<MemeMetadataController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateMemeMetadatumRequest memeMetadatumRequest)
        {
            var existingMemeMetadatum = await _memeMetadatumService.GetById(id);
            if (existingMemeMetadatum == null)
                return NotFound();
            if (memeMetadatumRequest.MemeId != 0)
                existingMemeMetadatum.MemeId = memeMetadatumRequest.MemeId;
            if (memeMetadatumRequest.FileSize != 0)
                existingMemeMetadatum.FileSize = memeMetadatumRequest.FileSize;
            if (memeMetadatumRequest.Height != 0)
                existingMemeMetadatum.Height = memeMetadatumRequest.Height;
            if (memeMetadatumRequest.Width != 0)
                existingMemeMetadatum.Width = memeMetadatumRequest.Width;
            if (!string.IsNullOrEmpty(memeMetadatumRequest.MimeType))
                existingMemeMetadatum.MimeType = memeMetadatumRequest.MimeType;
            if (!string.IsNullOrEmpty(memeMetadatumRequest.FileFormat))
                existingMemeMetadatum.FileFormat = memeMetadatumRequest.FileFormat;
            await _memeMetadatumService.Update(existingMemeMetadatum);
            return Ok();
        }

        /// <summary>
        /// Удаление методаты по Id
        /// </summary>
        /// <param name="id">Id методаты</param>

        // DELETE api/<MemeMetadataController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memeMetadatumService.Delete(id);
            return Ok();
        }
    }
}