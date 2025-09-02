using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace PepeProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> Summaries = new()
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll(int? sortIndex)
        {
            if (sortIndex == null)
            {
                return Ok(Summaries);
            }

            if (sortIndex != 1 && sortIndex != -1)
            {
                return BadRequest("Некорректное значение параметра sortStrategy");
            }

            var sortedList = Summaries.ToList();

            if (sortIndex == 1)
            {
                sortedList.Sort();
            }
            else if (sortIndex == -1)
            {
                sortedList.Sort();
                sortedList.Reverse();
            }

            return Ok(sortedList);
        }

        [HttpGet("{index:int}")]
        public IActionResult GetByIndex(int index)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return NotFound($"Элемент с индексом {index} не найден");
            }

            return Ok(Summaries[index]);
        }

        [HttpGet("find-by-name")]
        public IActionResult FindByName(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Имя не может быть пустым");
            }

            return Ok(Summaries.Count(s => s == name));
        }

        [HttpPost]
        public IActionResult Add(string? name)
        {
            if (string.IsNullOrEmpty(name))
            { 
                return BadRequest("Имя не может быть пустым");
            }

            Summaries.Add(name);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(int index, string? name)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return BadRequest("Такой индекс неверный");
            }

            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Имя не может быть пустым");
            }

            Summaries[index] = name;
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int index)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return BadRequest("Данный индекс неверный");
            }

            Summaries.RemoveAt(index);
            return Ok();
        }

        [HttpGet("version")]
        public IActionResult GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            Console.WriteLine($"ASP.NET Core version: {version}");
            return Ok($"Version: {version}");
        }
    }
}
