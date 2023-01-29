import { AfterViewInit, Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { Movie } from 'src/app/modules/core/models/movie.model';
import { MoviesService } from 'src/app/modules/core/services/movies.service';
import { onMainContentChange } from 'src/app/modules/shared/animations/animations';


@Component({
  templateUrl: './advanced-search.component.html',
  styleUrls: ['./advanced-search.component.scss'],
  animations: [onMainContentChange]
})
export class AdvancedSearchComponent implements OnInit {
  public simpleSearchedMovies = new Array<Movie>();
  public advancedSearchedMovies = new Array<Movie>();

  public keywords = "";
  public isLoadingSimple = false;
  public isLoadingAdv = false;

  constructor(private route: ActivatedRoute, private moviesService: MoviesService) {
  }

  ngOnInit(): void {
    this.route.params.pipe(take(1)).subscribe(params => {
      this.keywords = params['keywords'];
      this.requestMovies(this.keywords);
    });
  }

  public requestMovies(keywords: string) {
    this.isLoadingSimple = true;
    this.moviesService.getMoviesBySimpleSearch(keywords).subscribe(movies => {
      this.simpleSearchedMovies = movies;
      this.isLoadingSimple = false;
    })
    
    this.isLoadingAdv = true;
    this.moviesService.getMoviesByAdvancedSearch(keywords).subscribe(movies => {
      this.advancedSearchedMovies = movies;
      this.isLoadingAdv = false;
    })
  }
}
