import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FourSquareSearchDTO } from '../models/foursquare-search.model';  
import { FlickrPhotoSearchDTO } from '../models/FlickrPhotoSearchDTO';
import { GoogleGeoResponseDTO } from '../models/GoogleGeoResponseDTO';

@Injectable({
  providedIn: 'root'
})
export class FoursquareService {
  private baseUrl = 'https://192.168.100.9:5001'; 
  

  constructor(private http: HttpClient) { }

  searchPlaces(place: string): Observable<FourSquareSearchDTO> {
    const url = `${this.baseUrl}/api/Foursquare/${encodeURIComponent(place)}`;
    return this.http.get<FourSquareSearchDTO>(url);
  }

  GetFourSquareAttractionbyPlace(lat: number, lng: number): Observable<FourSquareSearchDTO> {
    const url = `${this.baseUrl}/api/Foursquare?lat=${lat}&lng=${lng}`;
    return this.http.get<FourSquareSearchDTO>(url);
  }

  searchFlickrPhotos(query: string): Observable<FlickrPhotoSearchDTO> {
    const url = `${this.baseUrl}/api/Flickr/${encodeURIComponent(query)}`;
    return this.http.get<FlickrPhotoSearchDTO>(url);
  }

  reverseGeoCode(lat: number, lng: number): Observable<any> {
    const url = `${this.baseUrl}/api/Google?lat=${lat}&lng=${lng}`;
    return this.http.get<any>(url);
  }

  getGeoCode(location: string): Observable<GoogleGeoResponseDTO> {
    const url = `${this.baseUrl}/api/Google/${location}`;
    return this.http.get<GoogleGeoResponseDTO>(url);
  }




}
