import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs';
import { Movie } from 'src/app/modules/core/models/movie.model';
import { MoviesService } from 'src/app/modules/core/services/movies.service';
import { onMainContentChange } from 'src/app/modules/shared/animations/animations';

@Component({
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.scss'],
  animations: [onMainContentChange]
})
export class GenreComponent implements OnInit {  
  public movies = new Array<Movie>();
  public pageIndex = 0;
  public isLoading: boolean = false;

  public MOVIES_PER_ROW = 4;
  public ROWS_FETCHING = 3;

  private genreName: string = '';
  private readonly DEFAULT_IMAGE_URL = './assets/img/default-img.jpg';

  constructor(private moviesService: MoviesService, private route: ActivatedRoute) {
  }

  @HostListener('window:scroll')
  scrollEvent() {
    let pos = (document.documentElement.scrollTop || document.body.scrollTop) + document.documentElement.offsetHeight;
    let max = document.documentElement.scrollHeight;
    if (pos == max && !this.isLoading) {
      this.requestNextMovies();
    }
  }

  ngOnInit(): void {
    this.route.params.pipe(take(1)).subscribe(params => {
      this.genreName = params['name'];
      this.requestNextMovies();
    });
  }

  public setDefaultImg(model: Movie) {
    if (!model) return;
    model.posterPath = this.DEFAULT_IMAGE_URL;
  }

  private requestNextMovies() {
    debugger;
    this.isLoading = true;
    this.moviesService.getMoviesForGenre(this.genreName, this.pageIndex, this.MOVIES_PER_ROW * this.ROWS_FETCHING).pipe(take(1)).subscribe((movies: Array<Movie>) => {
      this.movies.push(...movies);
      this.isLoading = false;
    });
    this.pageIndex++;
  }
}
