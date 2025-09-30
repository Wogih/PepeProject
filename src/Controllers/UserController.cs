using Domain.Interfaces.IUser;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.User;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получение данных всех пользователей
        /// </summary>

        // GET api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var result = await _userService.GetAll();
            var respose = result.Adapt<List<GetUserResponse>>();
            return Ok(respose);
        }

        /// <summary>
        /// Получение данных пользователя по Id
        /// </summary>
        /// <param name="id">Id пользователя</param>

        // GET api/<UsersController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetById(id);
            var response = result.Adapt<GetUserResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST
        ///     {
        ///        "username": "testname",
        ///        "email": "test@gmail.com",
        ///        "passwordHash": "testhash"
        ///     }
        /// 
        /// </remarks>
        /// <param name="user">Добавляемые данные пользователя</param>

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequest user)
        {
            var userDto = user.Adapt<User>();
            await _userService.Create(userDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных пользователя по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST
        ///     {
        ///        "username": "testname",
        ///        "email": "test@gmail.com",
        ///        "passwordHash": "testhash"
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">Id пользователя</param>
        /// <param name="userRequest">Обновленные данные пользователя</param>

        // DELETE api/<UsersController>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateUserRequest userRequest)
        {
            int userId = id - 1;
            var existingUser = await _userService.GetById(userId);
            if (existingUser == null)
                return NotFound();

            if (!string.IsNullOrEmpty(userRequest.Username))
                existingUser.Username = userRequest.Username;

            if (!string.IsNullOrEmpty(userRequest.Email))
                existingUser.Email = userRequest.Email;

            if (!string.IsNullOrEmpty(userRequest.PasswordHash))
                existingUser.PasswordHash = userRequest.PasswordHash;

            await _userService.Update(existingUser);
            return Ok();
        }

        /// <summary>
        /// Удаление пользователя по Id
        /// </summary>
        /// <param name="id">Id пользователя</param>

        // DELETE api/<UsersController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok();
        }
    }
}