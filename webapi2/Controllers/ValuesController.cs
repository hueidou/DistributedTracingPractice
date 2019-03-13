using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace webapi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITracer _tracer;
        private readonly ILogger _logger;

        public ValuesController(ITracer tracer, ILogger<ValuesController> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogInformation("GET api/values");

            using (var scope1 = _tracer.BuildSpan("Controller-GetAll").StartActive(true))
            {
                using (var scope2 = _tracer.BuildSpan("DataSource-GetAll").StartActive(true))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    return new string[] { "value1", "value2" };
                }
            }
        }
    }
}
