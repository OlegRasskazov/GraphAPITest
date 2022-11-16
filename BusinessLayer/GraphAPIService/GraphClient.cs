using Microsoft.Graph;

namespace GraphAPIService
{
    public class GraphClient
    {
        private readonly GraphServiceClient client;

        private const int maxChunkSize = 320 * 1024;

        public GraphClient(MsalAuthenticationProvider provider)
        {
            client = new GraphServiceClient(provider);
        }
        public async Task<IList<User>> GetUsersAsync(CancellationToken token = default)
            => await client.Users.Request().GetAsync(token);

        public async Task<IList<Drive>> GetDrivesAsync(CancellationToken token = default)
            => await client.Drives.Request().GetAsync(token);
        public async Task<IList<DriveItem>> GetFilesAsync(Guid driveId, CancellationToken token = default)
            => (await client.Drives[driveId.ToString()].Root.Children.Request().GetAsync(token)).Where(e => e.File != null).ToList();

        public async Task<Stream> DownloadFileAsync(Guid driveId, Guid fileId, CancellationToken token = default)
            => await client.Drives[driveId.ToString()].Items[fileId.ToString()].Content.Request().GetAsync(token);

        public async Task UploadFileAsync(Guid driveId, string fileName, Stream stream)
            => await client.Drives[driveId.ToString()].Root.ItemWithPath(fileName).Content.Request().PutAsync<DriveItem>(stream);
    }
}