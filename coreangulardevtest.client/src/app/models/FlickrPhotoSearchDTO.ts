export interface FlickrPhotoSearchDTO {
  photos: Photos;
  stat: string;
}

export interface Photos {
  page: number;
  pages: number;
  perpage: number;
  total: number;
  photo: Photo[];
}

export interface Photo {
  id: string;
  owner: string;
  secret: string;
  server: string;
  farm: number;
  title: string;
  ispublic: number;
  isfriend: number;
  isfamily: number;
  url_m: string;
  height_m: number;
  width_m: number;
  description: string;
  ownerrealname: string;
  dateuploaded: string;
}
