import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-movie-rating',
  templateUrl: './movie-rating.component.html',
  styleUrls: ['./movie-rating.component.scss']
})
export class MovieRatingComponent {
  @Input() score: number | undefined = 0;
  @Input() isSmall: boolean = false;
}
