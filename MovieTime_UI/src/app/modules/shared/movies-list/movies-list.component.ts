import { Component, Input } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Movie } from '../../core/models/movie.model';
import { MoviesService } from '../../core/services/movies.service';

@Component({
  selector: 'app-movies-list',
  templateUrl: './movies-list.component.html',
  styleUrls: ['./movies-list.component.scss']
})
export class MoviesListComponent {
  private readonly DEFAULT_IMAGE_URL = './assets/img/default-img.jpg';

  @Input() public movies = new Array<Movie>();

  constructor(private router: Router, private moviesService: MoviesService) {}

  public setDefaultImg(model: Movie) {
    if (!model) return;
    model.posterPath = this.DEFAULT_IMAGE_URL;
  }

  public onAddToWishlist(model: Movie) {
    if (!model) return;
    this.moviesService.addToWishlist(model);
  }
  
  public onCardClicked(model: Movie) {
    this.router.navigateByUrl(`/movie/${model.id}`);
  }
}
