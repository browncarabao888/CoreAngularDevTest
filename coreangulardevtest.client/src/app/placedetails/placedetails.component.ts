import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FoursquareService } from '../searchpage/service.service';
import { FlickrPhotoSearchDTO, Photo } from '../models/FlickrPhotoSearchDTO';
 
@Component({
  selector: 'app-placedetails',
  templateUrl: './placedetails.component.html',
  styleUrl: './placedetails.component.css'
})


export class PlacedetailsComponent {
  [x: string]: any;
  searchImages: any;
  photosObj!: FlickrPhotoSearchDTO;
  photos: any;
  loading: boolean = true;

  selectedPhoto: Photo | null = null;



  constructor(
    private foursquareService: FoursquareService,
    @Inject(MAT_DIALOG_DATA
    )
    public data: { title: string }
  ) { }


  ngOnInit() {
    this.searchPhotoOnFlickr(this.data);
  }



  async searchPhotoOnFlickr(data: any) {
    console.log('Data received in PlacedetailsComponent:', data);
    this.searchImages = data.title;
    console.log('Photos: ', this.searchImages);
    this.loading = true;

    this.foursquareService.searchFlickrPhotos(this.searchImages).subscribe(
      (details) => {
        this.photos = details.photos.photo;
        this.loading = false;
      },
      (error) => {
        console.error('Failed to load place details', error);
      }
    );
  

  }

  onSelectImage(photo: Photo) {
    this.selectedPhoto = photo;
  }




}

