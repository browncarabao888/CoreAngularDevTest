export interface GoogleGeoResponseDTO {
  results: GoogleGeocodeResult[];
  status: string;
}

export interface GoogleGeocodeResult {
  geometry: Geometry;
}

export interface Geometry {
  location: Location;
}

export interface Location {
  lat: number;
  lng: number;
}
