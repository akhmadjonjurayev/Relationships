using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Many_To_Many.Model;
namespace Many_To_Many.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly SalesDbContext _salesDb;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,SalesDbContext salesDb)
        {
            _salesDb = salesDb;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("getEmp/{id:int}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var emp = await _salesDb.Employees.Where(i => i.Id == id)
                .Include(i => i.salesE)
                .Select(group => new
                {
                    Id = group.Id,
                    ismi = group.FullName,
                    tel = group.PhoneNumber,
                    tovar = group.salesE.Select(i => new
                    {
                        vaqti = i.product.CreateDate,
                        turi = i.product.Type,
                        narxi = i.product.Cost
                    })
                }).FirstOrDefaultAsync();
            return Ok(emp);
        }
    }
}
