using Domain.Interfaces.IUploadStat;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.Collection;
using PepeProject.Contracts.UploadStat;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadStatController : ControllerBase
    {
        private IUploadStatService _collectService;
        public UploadStatController(IUploadStatService uploadStatService)
        {
            _collectService = uploadStatService;
        }

        /// <summary>
        /// Получение данных всех статистики мема
        /// </summary>

        // GET api/<UploadStatController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _collectService.GetAll();
            var response = result.Adapt<List<GetUploadStatResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных статистики мема по Id
        /// </summary>
        /// <param name="id">Id статистики мема</param>

        // GET api/<UploadStatController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _collectService.GetById(id);
            var response = result.Adapt<GetUploadStatResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание новой статистики мема
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "memeId": 1,
        ///        "viewsCount": 1000,
        ///        "downloadCount": 100,
        ///        "shareCount": 25
        ///     }
        /// </remarks>
        /// <param name="uploadStat">Данные новой статистики мема</param>

        // POST api/<UploadStatController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateUploadStatRequest uploadStat)
        {
            var uploadStatDto = uploadStat.Adapt<UploadStat>();
            await _collectService.Create(uploadStatDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных статистики мема по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "memeId": 1,
        ///        "viewsCount": 1000,
        ///        "downloadCount": 100,
        ///        "shareCount": 25
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id статистики мема</param>
        /// <param name="uploadStatRequest">Обновленные данные статистики мема</param>

        // PUT api/<UploadStatController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateUploadStatRequest uploadStatRequest)
        {
            var existingUploadStat = await _collectService.GetById(id);
            if (existingUploadStat == null)
                return NotFound();
            if (uploadStatRequest.MemeId != 0)
                existingUploadStat.MemeId = uploadStatRequest.MemeId;
            if (uploadStatRequest.DownloadCount != 0)
                existingUploadStat.DownloadCount = uploadStatRequest.DownloadCount;
            if (uploadStatRequest.ShareCount != 0)
                existingUploadStat.ShareCount = uploadStatRequest.ShareCount;
            if (uploadStatRequest.ViewsCount != 0)
                existingUploadStat.ViewsCount = uploadStatRequest.ViewsCount;
            await _collectService.Update(existingUploadStat);
            return Ok();
        }

        /// <summary>
        /// Удаление статистики мема по Id
        /// </summary>
        /// <param name="id">Id статистики мема</param>

        // DELETE api/<UploadStatController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _collectService.Delete(id);
            return Ok();
        }
    }
}