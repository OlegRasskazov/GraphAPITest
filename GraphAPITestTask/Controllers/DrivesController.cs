using GraphAPIService;
using Microsoft.AspNetCore.Mvc;

namespace GraphAPITestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrivesController : ControllerBase
    {
        private readonly GraphClient graphClient;

        public DrivesController(GraphClient graphClient)
            => this.graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));

        /// <summary>Gets all drives the asynchronous.</summary>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken token)
            => Ok(await graphClient.GetDrivesAsync(token));
    }
}
