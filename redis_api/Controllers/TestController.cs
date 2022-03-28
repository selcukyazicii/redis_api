using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using redis_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redis_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        public TestController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var models = await _cacheService.GetAsync<List<TestModel>>("models") ?? new List<TestModel>();
            var model = new TestModel(Faker.Name.First(), Faker.Name.Last());
            models.Add(model);
            await _cacheService.SetAsync("models", models);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var models = await _cacheService.GetAsync<List<TestModel>>("models");
            if (models != null)
            {
                try
                {
                    var guidId = new Guid(id);
                    var model = models.FirstOrDefault(p => p.Id == guidId);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                }
                catch (System.Exception)
                {
                }
            }
            return BadRequest("Üye bulunamadı");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cache = await _cacheService.GetAsync<List<TestModel>>("models");
            return Ok(cache);
        }
    }
}
