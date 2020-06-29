using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibreStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;

        public StoreController(ILogger<StoreController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<MainToken> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new MainToken
            {
                Key = rng.Next(-20, 55).ToString(),
                Created = DateTime.Now.AddDays(index),
                Active = true
            })
            .ToArray();
        }
    }
}
