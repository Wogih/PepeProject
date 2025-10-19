using Domain.Interfaces.IUserRole;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.UserRole;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// Получение данных всех пользователь->роль
        /// </summary>

        // GET api/<UserRoleController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var result = await _userRoleService.GetAll();
            var respose = result.Adapt<List<GetUserRoleResponse>>();
            return Ok(respose);
        }

        /// <summary>
        /// Получение данных пользователь->роль по Id
        /// </summary>
        /// <param name="id">Id пользователь->роль</param>

        // GET api/<UserRoleController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userRoleService.GetById(id);
            var response = result.Adapt<GetUserRoleResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание нового пользователь->роль
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST
        ///     {
        ///        "userId": 1,
        ///        "roleId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="userRole">Добавляемые данные пользователь->роль</param>

        // POST api/<UserRoleController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRoleRequest userRole)
        {
            var userRoleDto = userRole.Adapt<UserRole>();
            await _userRoleService.Create(userRoleDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных пользователь->роль по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST
        ///     {
        ///        "userId": 1,
        ///        "roleId": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">Id v</param>
        /// <param name="userRoleRequest">Обновленные данные пользователь->роль</param>

        // DELETE api/<UserRoleController>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateUserRoleRequest userRoleRequest)
        {
            var existingUserRole = await _userRoleService.GetById(id);
            if (existingUserRole == null)
                return NotFound();
            if (existingUserRole.UserId != 0)
                existingUserRole.UserId = userRoleRequest.UserId;
            if (existingUserRole.RoleId != 0)
                existingUserRole.RoleId = userRoleRequest.RoleId;
            await _userRoleService.Update(existingUserRole);
            return Ok();
        }

        /// <summary>
        /// Удаление пользователь->роль по Id
        /// </summary>
        /// <param name="id">Id пользователь->роль</param>

        // DELETE api/<UserRoleController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRoleService.Delete(id);
            return Ok();
        }
    }
}