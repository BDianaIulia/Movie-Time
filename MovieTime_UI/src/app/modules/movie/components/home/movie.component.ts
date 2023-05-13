import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs';
import { Movie } from 'src/app/modules/core/models/movie.model';
import { MoviesService } from 'src/app/modules/core/services/movies.service';
import { onMainContentChange } from 'src/app/modules/shared/animations/animations';
import { MovieRatingsComponent } from 'src/app/modules/shared/components/movie-rating/movie-ratings/movie-ratings.component';

@Component({
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.scss'],
  animations: [onMainContentChange]
})
export class MovieComponent implements OnInit {
  public model: Movie | undefined;

  public recommendedDetailsMovies = new Array<Movie>();
  public recommendedDescripitonMovies = new Array<Movie>();

  public loadingRecommendedDetailsMovies = false;
  public loadingRecommendedDescripitonMovies = false;

  private readonly DEFAULT_IMAGE_URL = './assets/img/default-img.jpg';

  constructor(private moviesService: MoviesService, private route: ActivatedRoute, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.route.params.pipe(take(1)).subscribe(params => {
      const id = params['id'];
      this.moviesService.getMovie(id).pipe(take(1)).subscribe((movie: Movie) => {
        this.model = movie;
        this.loadRecommendations();
      });
    });
  }
  private loadRecommendations() {
    if (!this.model) return;
    this.loadingRecommendedDescripitonMovies = true;
    this.loadingRecommendedDetailsMovies = true;
    this.moviesService.getRecommendedDetailsBasedMovies(this.model.title).pipe(take(1)).subscribe((movies: Array<Movie>) => {
      this.recommendedDetailsMovies = movies;
      this.loadingRecommendedDetailsMovies = false;
    });
    this.moviesService.getRecommendedDescriptionBasedMovies(this.model.title).pipe(take(1)).subscribe((movies: Array<Movie>) => {
      this.recommendedDescripitonMovies = movies;
      this.loadingRecommendedDescripitonMovies = false;
    });
  }

  public setDefaultImg(model: Movie) {
    if (!model) return;
    model.posterPath = this.DEFAULT_IMAGE_URL;
  }

  public onAddToWishlist() {
    if (!this.model) return;
    this.moviesService.addToWishlist(this.model);
  }

  public onRateMovie() {
    const dialogRef = this.dialog.open(MovieRatingsComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (!this.model) return;
      if(result === undefined) return;
      this.moviesService.addRating(this.model, result.data);
    });
  }

  public moviePercentage(model: Movie) {
    if (!model) return;
    this.moviesService.moviePercentage(model);
  }
}
