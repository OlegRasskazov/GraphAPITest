using GraphAPIService;
using Microsoft.AspNetCore.Mvc;

namespace GraphAPITestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly GraphClient graphClient;

        public UsersController(GraphClient graphClient) => this.graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(CancellationToken token)
            => Ok(await graphClient.GetUsersAsync(token));
    }
}
