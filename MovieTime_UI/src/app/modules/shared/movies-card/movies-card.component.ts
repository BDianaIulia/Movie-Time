import { Component, Input } from '@angular/core';
import { Movie } from '../../core/models/movie.model';

@Component({
  selector: 'app-movies-card',
  templateUrl: './movies-card.component.html',
  styleUrls: ['./movies-card.component.scss']
})
export class MoviesCardComponent {
  private readonly DEFAULT_IMAGE_URL = 'https://image.tmdb.org/t/p/original/vArgTEbIPVclxhfdLubLwOYfVKG.jpg';

  @Input() public model: Movie | undefined;

  constructor () {}

  public setDefaultImg(model: Movie) {
    if (!model) return;
    model.posterPath = this.DEFAULT_IMAGE_URL;
  }
}
