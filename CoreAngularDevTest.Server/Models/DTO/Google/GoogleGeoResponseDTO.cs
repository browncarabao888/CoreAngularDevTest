namespace CoreAngularDevTest.Server.Models.DTO.Google
{
    public class GoogleGeoResponseDTO
    {
        public List<GoogleGeocodeResult>? Results { get; set; }
        public string? Status { get; set; }
    }

    public class GoogleGeocodeResult
    {
        public Geometry? Geometry { get; set; }
    }

    public class Geometry
    {
        public Location? Location { get; set; }
    }

    public class Location
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }

}
