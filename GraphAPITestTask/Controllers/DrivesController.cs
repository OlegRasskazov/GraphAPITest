using GraphAPIService;
using Microsoft.AspNetCore.Mvc;

namespace GraphAPITestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrivesController:ControllerBase
    {
        private readonly GraphClient graphClient;

        public DrivesController(GraphClient graphClient)
            => this.graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken token) 
            => Ok(await graphClient.GetDrivesAsync(token));
    }
}
