namespace EFContext.Entities
{
    public class File
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public Guid DriveId { get; set; }
        public Drive Drive { get; set; }
    }
}
