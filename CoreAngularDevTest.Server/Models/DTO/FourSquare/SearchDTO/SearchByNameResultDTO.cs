namespace CoreAngularDevTest.Server.Models.DTO.FourSquare.SearchDTO
{
    //public class SearchByNameResultDTO
    //{
    //    public List<Result> results { get; set; }
    //}

    //public class Geocodes
    //{
    //    public Main main { get; set; }
    //}

    //public class Location
    //{
    //    public string formatted_address { get; set; }
    //}

    //public class Main
    //{
    //    public double latitude { get; set; }
    //    public double longitude { get; set; }
    //}

    //public class Result
    //{
    //    public string fsq_id { get; set; }
    //    public string name { get; set; }
    //    public Location location { get; set; }
    //    public Geocodes geocodes { get; set; }
    //}


    

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Center
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }

    public class Circle
    {
        public Center? center { get; set; }
        public int? radius { get; set; }
    }

    public class Context
    {
        public GeoBounds? geo_bounds { get; set; }
    }

    public class GeoBounds
    {
        public Circle? circle { get; set; }
    }

    public class SearchByNameResultDTO
    {
        public List<object>? results { get; set; }
        public Context? context { get; set; }
    }



}
