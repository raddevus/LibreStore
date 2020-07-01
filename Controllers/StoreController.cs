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

        [HttpPost("Get")]
        public Store Get(Store store)
        {
            if (store.MainTokenKey == null || store.MainTokenKey == String.Empty){
                // Since the user provides no MainTokenKey we cannot return
                // Returning null returns an HTTP 204 code
                // which means the request was processed but is returning nothing
                return null;
            }
            if (store.ID == null || store.ID <= 0){
                // same as null or empty MainTokenKey
                return null;
            }
            // 1. look up store
            return new Store(store.ID,store.MainTokenKey);
        }

        [HttpPost("Save")]
        public void Save(Store store){
            Console.WriteLine($"data: {store.Data}");
            Console.WriteLine($"key: {store.MainTokenKey}");
        }


    }
}
