namespace CoreAngularDevTest.Server.Models
{
    public class ImageEntity
    {
        public string? FileName { get; set; }
        public byte[]? ImageData { get; set; } 
    }

    public class ImageDTO
    {
        public string? FileName { get; set; }
        public string? Base64Image { get; set; }
    }
}
