export interface FourSquareSearchDTO {
  results: Result[];
  context: Context;
}

export interface Result {
  fsq_id: string;
  categories: Category[];
  chains: any[];  
  closed_bucket: string;
  distance: number;
  geocodes: Geocodes;
  link: string;
  location: Location;
  name: string;
  related_places: RelatedPlaces;
  timezone: string;
}

export interface Category {
  id: number;
  name: string;
  short_name: string;
  plural_name: string;
  icon: Icon;
}

export interface Icon {
  prefix: string;
  suffix: string;
}

export interface Geocodes {
  main: Main;
  roof: Roof;
}

export interface Main {
  latitude: number;
  longitude: number;
}

export interface Roof {
  latitude: number;
  longitude: number;
}

export interface Location {
  country: string;
  cross_street: string;
  formatted_address: string;
  locality: string;
  postcode: string;
  region: string;
}

export interface RelatedPlaces {
  
}

export interface Context {
  geo_bounds: GeoBounds;
}

export interface GeoBounds {
  circle: Circle;
}

export interface Circle {
  center: Center;
  radius: number;
}

export interface Center {
  latitude: number;
  longitude: number;
}
