import { Component, NgZone, ViewChild } from '@angular/core';
import { FoursquareService } from './service.service';
import { FourSquareSearchDTO, Result } from '../models/foursquare-search.model';
import { GoogleMap } from '@angular/google-maps';
import { PlacedetailsComponent } from '../placedetails/placedetails.component';
import { MatDialog } from '@angular/material/dialog';
import { GoogleGeoResponseDTO } from '../models/GoogleGeoResponseDTO';
import { FlickrPhotoSearchDTO } from '../models/FlickrPhotoSearchDTO';
import { Subject, takeUntil } from 'rxjs';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';
import { FormControl } from '@angular/forms';
import { Observable, startWith, map } from 'rxjs';

@Component({
  selector: 'app-searchpage',
  templateUrl: './searchpage.component.html',
  styleUrl: './searchpage.component.css'
})
export class SearchpageComponent {
  private destroy$ = new Subject<void>();

  searchControl = new FormControl('');
  selectedOption: number = 1;
  lat: number = 0;
  lng: number = 0;
  location: string = "";

  @ViewChild(GoogleMap, { static: false }) map!: GoogleMap;

  suggestions: string[] = ['London', 'Paris', 'Tokyo', 'Milan', 'Germany', 'Durban, South Africa', 'Korea'];
  filteredSuggestions: string[] = [];

  mapCenter: google.maps.LatLngLiteral = { lat: 0, lng: 0 };
  zoom: number = 12;
   
  selectedCategoryId: number | null = null;
  results: GoogleGeoResponseDTO | null = null;
  places: Result[] = [];
  private snkbarRef: MatSnackBarRef<TextOnlySnackBar> | null = null;

  categories = [
    { id: 13032, name: 'Coffee Shop' },
    { id: 13002, name: 'Bakery' },
    { id: 13065, name: 'Restaurant' },
    { id: 13145, name: 'Fast Food' },
    { id: 19014, name: 'Hotel' },
    { id: 13035, name: 'Bar' },
  ];
  constructor(
    private foursquareService: FoursquareService,
    private ngZone: NgZone,
    private dialog: MatDialog,
    private snkbar: MatSnackBar)
  { }

  ngOnInit() {

    this.searchControl.valueChanges.pipe(startWith(''),
      map(value => this._filterSuggestions(value || ''))
    ).subscribe(filtered => {
      this.filteredSuggestions = filtered;
    });
    //console.log('Filtered:', this.filteredSuggestions);
    this.getCurrentLocation();
  }

  zoomTo(lat: number, lng: number) {
    const target = { lat, lng };

    this.lat = lat;
    this.lng = lng;
    this.mapCenter = target;

    setTimeout(() => {
      if (this.map && this.map.googleMap) {
        this.map.googleMap.panTo(target);
        this.map.googleMap.setZoom(16);
      }
    }, 100);
  }

  animateZoom(targetZoom: number) {
    const step = this.zoom < targetZoom ? 1 : -1;
    const interval = setInterval(() => {
      if (this.zoom === targetZoom) {
        clearInterval(interval);
      } else {
        this.zoom += step;
      }
    }, 100);
  }

  onEnterKeySearch(value: string): void {
    this.location = value;
     
    this.foursquareService.getGeoCode(this.location).pipe(takeUntil(this.destroy$)).subscribe({
      next: (response: GoogleGeoResponseDTO) => {
        this.results = response;
        this.openSnackbar();

        const center = this.results?.results?.[0]?.geometry?.location;
        if (center?.lat !== undefined && center?.lng !== undefined) {
          this.lat = center.lat;
          this.lng = center.lng;

          this.ngZone.run(() => {
            this.mapCenter = { lat: this.lat, lng: this.lng };
          });

          this.getAttractions(this.lat, this.lng);
           

        } else {
          console.warn('No center coordinate found.');
        }
      },
      error: (err) => {
        console.error('Error retrieving location data:', err);
      }
    });
  }

  getAttractions(lat: number, lng: number): void {
    this.foursquareService.GetFourSquareAttractionbyPlace(lat, lng).pipe(takeUntil(this.destroy$)).subscribe({
      next: (response: FourSquareSearchDTO) => {
        this.places = response.results || [];

        if (this.places.length === 0) {
          this.snkbar.open('Sorry, No available data based on your selected category.', 'Close', {
            duration: 6000,
            panelClass: ['snackbar-error']
          });
        } else {
          this.closeSnackbar();
        }
         
      },
      error: (err) => {
        console.error('Error retrieving attractions:', err);
      }
    });
  };


  getImage(place: any): string { //unused - for foursquare endpoint only
    const icon = place.categories?.[0]?.icon;
    var imgIcon = icon ? `${icon.prefix}64${icon.suffix}` : 'assets/img/default-location.png';
   // console.log('Icon : ', imgIcon);
    return imgIcon;
  }

  ////onOpenModal(title: string): void {
  ////  this.foursquareService.getPlaceDetailsByTitle(title).subscribe(
  ////    (details) => {
  ////      this.dialog.open(PlacedetailsComponent, {
  ////        maxWidth: '95vw',
  ////        maxHeight: '90vh',
  ////        width: '80%',
  ////        height: 'auto',
  ////        data: {
  ////          title: title,
  ////          details: details // pass the fetched data
  ////        },
  ////        panelClass: 'custom-dialog-container'
  ////      });
  ////    },
  ////    (error) => {
  ////      console.error('Failed to load place details', error);
  ////    }
  ////  );
  ////}

  onOpenModal(title: string): void {
    this.dialog.open(PlacedetailsComponent, {
      maxWidth: '95vw',
      maxHeight: '90vh',
      width: '80%',
      height: 'auto',
      data: { title },
      panelClass: 'custom-dialog-container'
    });
  }

  testMoveMap() {
    this.mapCenter = { lat: 10.3157, lng: 123.8854 };
  }

  get hasPlaces(): boolean {
    return this.places && this.places.length > 0;
  }

  openSnackbar() {
    this.snkbarRef = this.snkbar.open('Processing data. please wait..', 'Close this', {
      duration: undefined  
    });
  }

  closeSnackbar() {
    if (this.snkbarRef) {
      this.snkbarRef.dismiss(); 
      this.snkbarRef = null;
    }
  }


  async getCurrentLocation(): Promise<void> {
    if (!navigator.geolocation) {
      console.error('Geolocation is not supported by this browser.');
      return;
    }

    let position: GeolocationPosition;

    try {
      position = await new Promise<GeolocationPosition>((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject);
      });
    } catch (geoError) {
      console.error('Geolocation failed:', geoError);
      return;
    }

    const lat = position.coords.latitude;
    const lng = position.coords.longitude;

    let response;
    this.mapCenter = { lat, lng };
    this.lat = lat;
    this.lng = lng;

    try {
      response = await this.foursquareService.reverseGeoCode(lat, lng).pipe(takeUntil(this.destroy$)).toPromise();
    } catch (fsqError) {
      console.error('Foursquare reverse geocding failed:', fsqError);
      return;
    }

      if (response?.city || response?.state) {
        const city = response?.city ?? '';
        const state = response?.state ?? '';

        const parts = [city, state].filter(p => p.trim());
        this.location = parts.join(', ');

        this.onEnterKeySearch(this.location);
      } else {
        console.warn('location error', response);
      }

    //} catch(error) {
    //  console.error('Error getting location or reverse geocoding:', error);
    //}
  }

  private _filterSuggestions(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.suggestions.filter(option => option.toLowerCase().includes(filterValue));
  }

  onSelectOption(value: string): void {
    this.onEnterKeySearch(value);  
    this.searchControl.setValue(''); 
  }
}
