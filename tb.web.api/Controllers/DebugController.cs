using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tb.actor.grainsInterfaces;

namespace tb.web.api.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILogger _logger;

        public DebugController(IClusterClient clusterClient, ILogger<DebugController> logger)
        {
            _clusterClient = clusterClient;
            _logger = logger;

        }
        [HttpGet]
        public async Task<Guid> Test()
        {
            var id = Guid.NewGuid();
            var grain = _clusterClient.GetGrain<IGameGrain>(id);
            return await grain.GetId();
        }
    }
}
