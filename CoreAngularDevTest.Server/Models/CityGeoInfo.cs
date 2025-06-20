namespace CoreAngularDevTest.Server.Models
{

    public class LocationGeoInfo
    {
        public int Id { get; set; }
        public List<Result> Results { get; set; } = new();
        public string? Status { get; set; }
    }

    public class AddressComponent
    {
        public int Id { get; set; }
        public string? Long_name { get; set; }
        public string? Short_name { get; set; }
        public List<string?> Types { get; set; } = new List<string?>();
    }

    public class Bounds : BaseDirections { }
    public class Location : BaseCoordinates { }

    public class Southwest : BaseCoordinates { }

    public class Viewport : BaseDirections { }

    public class Northeast : BaseCoordinates { }
    public class Geometry
    {
        public int Id { get; set; }
        public Bounds Bounds { get; set; } = new();
        public Location Location { get; set; } = new();
        public string? Location_type { get; set; }  
        public Viewport Viewport { get; set; } = new();
    }



    public class Result
    {
        public int Id { get; set; }
        public List<AddressComponent> address_components { get; set; } = new();
        public string? formatted_address { get; set; }
        public Geometry geometry { get; set; } = new();
        public string? place_id { get; set; }
        public List<string?> types { get; set; } = new();
    }


    public class BaseDirections
    {
        public int Id { get; set; }
        public Northeast northeast { get; set; } = new();
        public Southwest southwest { get; set; } = new();
    }

    public class BaseCoordinates
    {
        public int Id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

}
