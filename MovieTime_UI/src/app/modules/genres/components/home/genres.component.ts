import { CdkVirtualScrollViewport, ScrollDispatcher } from '@angular/cdk/scrolling';
import { AfterViewInit, Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { filter, take } from 'rxjs';
import { Movie } from 'src/app/modules/core/models/movie.model';
import { GenresService } from 'src/app/modules/core/services/genres.service';
import { onMainContentChange } from 'src/app/modules/shared/animations/animations';

export interface GenreMovies {
  genreName: string;
  moviesList: Array<Movie>;
}

@Component({
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.scss'],
  animations: [onMainContentChange]
})
export class GenresComponent implements OnInit {  
  public genreMovies = new Array<GenreMovies>();
  public isLoading = false;

  private genreNames = new Array<string>();
  private counter = 0;
  private NO_ITEMS_SCROLL = 3;

  constructor(private genresService: GenresService, private router: Router) {
  }

  @HostListener('window:scroll')
  scrollEvent() {
    let pos = (document.documentElement.scrollTop || document.body.scrollTop) + document.documentElement.offsetHeight;
    let max = document.documentElement.scrollHeight;
    if (pos == max ) {
      this.requestNewGenreMovies();
    }
  }

  ngOnInit(): void {
    this.genresService.getGenreNames().pipe(take(1)).subscribe((genreNames: Array<string>) => {
      this.genreNames = genreNames;
      this.requestNewGenreMovies();
    });
  }

  public onOpenCategory(genreName: string) : void {
    this.router.navigateByUrl(`/genre/${genreName}`);
  }

  private requestNewGenreMovies(): void {
    this.isLoading = true;
    const positions = Array(this.NO_ITEMS_SCROLL).fill(0).map((x, i) => this.counter + i);
    for (let position of positions) {
      if (this.genreNames.length <= position)
      {
        this.isLoading = false;
        break;
      }

      let counter = 0;
      this.genresService.getGenreRecommendations(this.genreNames[position]).pipe(take(1)).subscribe((movies: Array<Movie>) => {
        const object = {genreName: this.genreNames[position], moviesList: movies} as GenreMovies
        this.genreMovies.push(object);
        if (++counter === this.NO_ITEMS_SCROLL) 
          this.isLoading = false;
      });
    }
    this.counter += this.NO_ITEMS_SCROLL;
  }
}
