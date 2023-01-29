import {Component, Input} from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-movie-ratings',
  templateUrl: './movie-ratings.component.html',
  styleUrls: ['./movie-ratings.component.scss', "../movie-rating.component.scss"]
})
export class MovieRatingsComponent {

  // constructor(public dialogRef: MatDialogRef<MovieRatingsComponent>) {}

  public onAddRating(rating: number) {
    //this.dialogRef.close({succedeed: true, data: rating});
  }
}
