/* eslint-disable no-debugger */
import { devOnlyGuardedExpression } from '@angular/compiler';
import { Component, ElementRef, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { Router } from '@angular/router';
import { take, takeUntil } from 'rxjs';
import { Movie } from 'src/app/modules/core/models/movie.model';
import { MoviesService } from 'src/app/modules/core/services/movies.service';
import { onMainContentChange } from 'src/app/modules/shared/animations/animations';
import { SidenavService } from 'src/app/modules/shared/services/sidenav.service';
import { environment } from 'src/environments/environment';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  animations: [onMainContentChange]
})
export class HomeComponent implements OnInit {
  @ViewChild('moviesListElement', { read: ElementRef })
  public moviesListElement!: ElementRef<any>;

  public appVersion = environment.appSettings.version;
  public onSideNavChange: boolean = false;

  public overviewList = new Array<Movie>();
  public recommendedMovies = new Array<Movie>();

  constructor(private _sidenavService: SidenavService, private moviesService: MoviesService, private router: Router) {
    this._sidenavService.sideNavState$.subscribe( res => {
      this.onSideNavChange = res;
    })
  }

  ngOnInit(): void {
    this.moviesService.getOverview().pipe(take(1)).subscribe((overviewList) => {
      this.overviewList = overviewList;
    });
    this.moviesService.getRecommendedUserActivityBasedMovies().subscribe((recommendedMovies) => {
      this.recommendedMovies = recommendedMovies;
    });
  }

  public onMovieClicked(movie: Movie): void {
    this.router.navigateByUrl(`/movie/${movie.id}`);
  }
}
