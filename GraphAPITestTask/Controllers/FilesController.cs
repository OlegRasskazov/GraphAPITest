using EFContext;
using EFContext.Entities;
using GraphAPIService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphAPITestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly GraphClient graphClient;
        private readonly ApplicationContext db;

        public FilesController(GraphClient graphClient,
                               ApplicationContext db)
        {
            this.graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>Gets the files asynchronous.</summary>
        /// <param name="driveId">The drive identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("{driveId}")]
        public async Task<IActionResult> GetFilesAsync(Guid driveId, CancellationToken token)
            => Ok(await graphClient.GetFilesAsync(driveId, token));


        /// <summary>Downloads the file asynchronous.</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost("download")]
        public async Task<IActionResult> DownloadFileAsync(DownloadFileRequest request)
        {

            var fileStream = await graphClient.DownloadFileAsync(request.DriveId, request.FileId);
            if (fileStream == null) return NoContent();

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var drive = await db.Drives.Include(e => e.Files).SingleOrDefaultAsync(e => e.Id == request.DriveId);
            if (drive == null)
            {
                var driveInfo = (await graphClient.GetDrivesAsync()).Single(e => e.Id == request.DriveId.ToString());
                drive = new Drive() { Id = request.DriveId, Name = driveInfo.Name };
                db.Drives.Add(drive);
            }
            var file = drive.Files.SingleOrDefault(e => e.Id == request.FileId);
            if (file == null)
            {
                var fileInfo = (await graphClient.GetFilesAsync(request.DriveId)).Single(e => e.Id == request.FileId.ToString());
                file = new() { Id = request.FileId, Name = fileInfo.Name, Data = fileBytes };
                drive.Files.Add(file);
            }
            else
            {
                file.Data = fileBytes;
            }

            db.SaveChanges();
            return Ok();
        }


        /// <summary>Uploads the file asyng.</summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsyng(UploadFileRequest request)
        {
            var dbFile = await db.ODFiles.SingleOrDefaultAsync(e => e.Id == request.FileId);
            if (dbFile == null) return NoContent();

            using var stream = new MemoryStream(dbFile.Data);
            await graphClient.UploadFileAsync(dbFile.DriveId, dbFile.Name, stream);
            return Ok();
        }

        #region Request
        public class DownloadFileRequest
        {
            public Guid DriveId { get; set; }
            public Guid FileId { get; set; }
        }
        public class UploadFileRequest
        {
            public Guid FileId { get; set; }
        }
        #endregion
    }
}