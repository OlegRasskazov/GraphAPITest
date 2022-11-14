namespace EFContext.Entities
{
    public class Drive
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<File> Files { get; set; }
    }
}
