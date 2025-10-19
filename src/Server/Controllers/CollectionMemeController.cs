using Domain.Interfaces.ICollectionMeme;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.CollectionMeme;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionMemeController : ControllerBase
    {
        private ICollectionMemeService _collectService;
        public CollectionMemeController(ICollectionMemeService collectionMemeService)
        {
            _collectService = collectionMemeService;
        }

        /// <summary>
        /// Получение данных всех коллекции->мем
        /// </summary>

        // GET api/<CollectionMemeController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _collectService.GetAll();
            var response = result.Adapt<List<GetCollectionMemeResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных коллекции->мем по Id
        /// </summary>
        /// <param name="id">Id коллекции->мем</param>

        // GET api/<CollectionMemeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _collectService.GetById(id);
            var response = result.Adapt<GetCollectionMemeResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание новой коллекции->мем
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "collectionId": 1,
        ///        "memeId": 1,
        ///     }
        /// </remarks>
        /// <param name="collectionMeme">Данные новой коллекции->мем</param>

        // POST api/<CollectionMemeController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateCollectionMemeRequest collectionMeme)
        {
            var collectionMemeDto = collectionMeme.Adapt<CollectionMeme>();
            await _collectService.Create(collectionMemeDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных коллекции->мем по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "collectionId": 1,
        ///        "memeId": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id коллекции->мем</param>
        /// <param name="collectionRequest">Обновленные данные коллекции->мем</param>

        // PUT api/<CollectionMemeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCollectionMemeRequest collectionRequest)
        {
            var existingCollection = await _collectService.GetById(id);
            if (existingCollection == null)
                return NotFound();
            if (collectionRequest.CollectionId != 0)
                existingCollection.CollectionId = collectionRequest.CollectionId;
            if (collectionRequest.MemeId != 0)
                existingCollection.MemeId = collectionRequest.MemeId;
            await _collectService.Update(existingCollection);
            return Ok();
        }

        /// <summary>
        /// Удаление коллекции->мем по Id
        /// </summary>
        /// <param name="id">Id коллекции->мем</param>

        // DELETE api/<CollectionMemeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _collectService.Delete(id);
            return Ok();
        }
    }
}