using Domain.Interfaces.ICollection;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.Collection;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private ICollectionService _collectService;
        public CollectionController(ICollectionService collectionService)
        {
            _collectService = collectionService;
        }

        /// <summary>
        /// Получение данных всех коллекций
        /// </summary>

        // GET api/<CollectionController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _collectService.GetAll();
            var response = result.Adapt<List<GetCollectionResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных коллекции по Id
        /// </summary>
        /// <param name="id">Id коллекции</param>

        // GET api/<CollectionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _collectService.GetById(id);
            var response = result.Adapt<GetCollectionResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание новой коллекции
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "userId": 1,
        ///        "collectionName": "New Collection Name",
        ///        "description": "New Collection Description",
        ///        "isPublic": true
        ///     }
        /// </remarks>
        /// <param name="collection">Данные новой коллекции</param>

        // POST api/<CollectionController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateCollectionRequest collection)
        {
            var collectionDto = collection.Adapt<Collection>();
            await _collectService.Create(collectionDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных коллекции по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "userId": 1,
        ///        "collectionName": "New Collection Name",
        ///        "description": "New Collection Description",
        ///        "isPublic": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id коллекции</param>
        /// <param name="collectionRequest">Обновленные данные коллекции</param>

        // PUT api/<CollectionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCollectionRequest collectionRequest)
        {
            var existingCollection = await _collectService.GetById(id);
            if (existingCollection == null)
                return NotFound();
            if (collectionRequest.UserId != 0)
                existingCollection.UserId = collectionRequest.UserId;
            if (!string.IsNullOrEmpty(collectionRequest.CollectionName))
                existingCollection.CollectionName = collectionRequest.CollectionName;
            if (!string.IsNullOrEmpty(collectionRequest.Description))
                existingCollection.Description = collectionRequest.Description;
            if (collectionRequest.IsPublic != null)
                existingCollection.IsPublic = collectionRequest.IsPublic;
            await _collectService.Update(existingCollection);
            return Ok();
        }

        /// <summary>
        /// Удаление коллекции по Id
        /// </summary>
        /// <param name="id">Id коллекции</param>

        // DELETE api/<CollectionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _collectService.Delete(id);
            return Ok();
        }
    }
}