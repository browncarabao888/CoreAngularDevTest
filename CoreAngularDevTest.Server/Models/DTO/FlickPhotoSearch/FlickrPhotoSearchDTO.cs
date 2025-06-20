namespace CoreAngularDevTest.Server.Models.DTO.FlickPhotoSearch
{
    public class FlickrPhotoSearchDTO
    {
        public Photos? photos { get; set; }
        public string? stat { get; set; }
        public string? PlaceName { get; set; } = string.Empty;
    }

 
    public class Photo
    {
        internal string? filename;

        public string? id { get; set; }
        public string? owner { get; set; }
        public string? secret { get; set; }
        public string? server { get; set; }
        public int? farm { get; set; }
        public string? title { get; set; }
        public int? ispublic { get; set; }
        public int? isfriend { get; set; }
        public int? isfamily { get; set; }
        public string? url_m { get; set; }
        public int? height_m { get; set; }
        public int? width_m { get; set; }
        public string? description { get; set; }
        public string? dateuploaded { get; set; }
        public string? OwnerRealName { get; set; }
        public string? OwnerUserName { get; set; }
        public string? Tags { get; set; }

    }

    public class Photos
    {
        public int? page { get; set; }
        public int? pages { get; set; }
        public int? perpage { get; set; }
        public int? total { get; set; }
        public List<Photo>? photo { get; set; }
    }
 


}
