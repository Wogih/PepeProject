using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace PepeProject.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        public User User => (User)HttpContext.Items["User"];
    }
}