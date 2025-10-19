using Domain.Interfaces.IRole;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using PepeProject.Contracts.Role;

namespace PepeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Получение данных всех ролей
        /// </summary>

        // GET api/<RoleController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAll();
            var response = result.Adapt<List<GetRoleResponse>>();
            Console.Write(response);
            return Ok(response);
        }

        /// <summary>
        /// Получение данных роли по Id
        /// </summary>
        /// <param name="id">Id роли</param>

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roleService.GetById(id);
            var response = result.Adapt<GetRoleResponse>();
            return Ok(response);
        }

        /// <summary>
        /// Создание новой роли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST
        ///     {
        ///        "roleName": "New Role Name",
        ///        "description": "New Description"
        ///     }
        ///
        /// </remarks>
        /// <param name="role">Данные роли</param>

        // POST api/<RoleController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateRoleRequest role)
        {
            var roleDto = role.Adapt<Role>();
            await _roleService.Create(roleDto);
            return Ok();
        }

        /// <summary>
        /// Обновление данных роли по Id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT 
        ///     {
        ///        "roleName": "New Role Name",
        ///        "description": "New Description"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id роли</param>
        /// <param name="roleRequest">Обновленные данные роли</param>

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateRoleRequest roleRequest)
        {
            var existingRole = await _roleService.GetById(id);
            if (existingRole == null)
                return NotFound();
            if (!string.IsNullOrEmpty(roleRequest.RoleName))
                existingRole.RoleName = roleRequest.RoleName;
            if (!string.IsNullOrEmpty(roleRequest.Description))
                existingRole.Description = roleRequest.Description;
            await _roleService.Update(existingRole);
            return Ok();
        }

        /// <summary>
        /// Удаление роли по Id
        /// </summary>
        /// <param name="id">Id роли</param>

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.Delete(id);
            return Ok();
        }
    }
}