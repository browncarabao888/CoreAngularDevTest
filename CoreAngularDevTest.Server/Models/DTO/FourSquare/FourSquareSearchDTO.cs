﻿namespace CoreAngularDevTest.Server.Models.DTO.FourSquare
{
    public class FourSquareSearchDTO
    {
        public List<Result>? results { get; set; }
        public Context? context { get; set; }
    }

    public class Category
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? short_name { get; set; }
        public string? plural_name { get; set; }
        public Icon? icon { get; set; }
    }

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

    public class Geocodes
    {
        public Main? main { get; set; }
        public Roof? roof { get; set; }
    }

    public class Icon
    {
        public string? prefix { get; set; }
        public string? suffix { get; set; }
    }

    public class Location
    {
        public string? country { get; set; }
        public string? cross_street { get; set; }
        public string? formatted_address { get; set; }
        public string? locality { get; set; }
        public string? postcode { get; set; }
        public string? region { get; set; }
    }

    public class Main
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }

    public class RelatedPlaces
    {
    }

    public class Result
    {
        public string? fsq_id { get; set; }
        public List<Category>? categories { get; set; }
        public List<object>? chains { get; set; }
        public string? closed_bucket { get; set; }
        public int? distance { get; set; }
        public Geocodes? geocodes { get; set; }
        public string? link { get; set; }
        public Location? location { get; set; }
        public string? name { get; set; }
        public RelatedPlaces? related_places { get; set; }
        public string? timezone { get; set; }
    }

    public class Roof
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }

   


}
