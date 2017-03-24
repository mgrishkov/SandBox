using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sample02.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class KeysController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "key1", "key2" };
        }
    }
}
